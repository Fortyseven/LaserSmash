using UnityEngine;
using System.Collections;
using Game;

/* TODO: Player should have split second ability to dodge laser; maybe 0.15ms delay? */

public class UFO : EnemyType
{
    public GameObject ExplosionPrefab = null;
    public GameObject ExplosionLaserGroundPrefab = null;

    const float MAX_Y_SPAWN = 13.0f;
    const float MIN_Y_SPAWN = 6.0f;
    const float X_RANGE_RIGHT = 18.0f;
    const float X_RANGE_LEFT = -18.0f;
    const int DIR_RIGHT = 1; // r-t-l
    const int DIR_LEFT = -1; // l-t-r
    const float CHARGING_PITCH = 0.75f;
    const float PASSIVE_PITCH = -0.8f;
    const float LASER_FADE_TIME = 0.5f;
    const float LASER_FADE_GRANULARITY = 0.05f;

    const float CHARGING_TIME = 1.0f;
    const float TARGET_LOCK_TIME = 0.75f;

    int _direction = 0;
    float _speed = 10.0f;
    float _charging_speed = 3.0f;
    Vector3 _newpos;
    Vector3 _player_target_position;

    LineRenderer _laser;
    Light _charging_light;
    SpriteRenderer _charging_flare_sprite;
    AudioSource _audio;

    private enum State {
        PASSIVE,
        CHARGING,
        FIRING
    };

    float _time_started_charging;
    State _state = State.PASSIVE;

    /*****************************/
    void Awake()
    {
        // Pick a side of the screen to fly out of
        _laser = GetComponentInChildren<LineRenderer>();
        _laser.gameObject.SetActive(false);

        _charging_light = GetComponentInChildren<Light>();
        _charging_light.enabled = false;

        _charging_flare_sprite = GetComponentInChildren<SpriteRenderer>();
        _charging_flare_sprite.enabled = false;

        _audio = GetComponent<AudioSource>();
    }
    
    /*****************************/
    void Update()
    {
        if (!_is_ready) return;

        switch(_state) {
            case State.PASSIVE:
                // Occasionally fire down hot death
                if (!GameController.instance.State.GameOver && Random.Range(0, 250) == 0) {
                    _state = State.CHARGING;
                    _charging_flare_sprite.enabled = true;
                    _charging_light.enabled = true;
                    _time_started_charging = Time.time;
                    StartCoroutine("AcquireTargetLock");
                    _audio.pitch = CHARGING_PITCH;
                }
                break;
            case State.CHARGING:
                if (Time.time - _time_started_charging  > CHARGING_TIME) {
                    StartCoroutine("Fire");
                }
                break;             
            case State.FIRING:
                _laser.SetPosition(1, transform.position);
                _laser.SetPosition(0, _player_target_position);
                break;
        }

        updateMovement();
    }

    /*****************************/
    IEnumerator AcquireTargetLock()
    {
        yield return new WaitForSeconds(TARGET_LOCK_TIME);
        _player_target_position = GameController.instance.PlayerShip.transform.position;
    }

    /*****************************/
    IEnumerator Fire()
    {
        _state = State.FIRING;
        _laser.gameObject.SetActive(true);

        Instantiate(ExplosionLaserGroundPrefab, _player_target_position, Quaternion.identity);

        // Check for collision
        Ray2D r = new Ray2D(transform.position, (_player_target_position - transform.position) * 15);

        /* We COULD check if this is the player being hit, but all the enemies are on layer 8, and
           nothing else with a collider exists on any other layer but the player. */

        if (/*hit = */Physics2D.Raycast(r.origin, r.direction, Mathf.Infinity, 1|8)) {
            GameController.instance.PlayerComponent.PlayerKilled();
        }

        // Fade out the beam over LASER_FADE_TIME seconds
        Color col = _laser.material.GetColor("_TintColor");
        float del = LASER_FADE_TIME * LASER_FADE_GRANULARITY;

        for (float i = 0; i < 1.0f; i+=LASER_FADE_GRANULARITY) {
            col.a = 1.0f - i;
            _laser.material.SetColor("_TintColor", col);
            yield return new WaitForSeconds(del);
        }

        ShutDownLaser();
    }

    /*****************************/
    void ShutDownLaser()
    {
        _laser.gameObject.SetActive(false);
        
        _charging_flare_sprite.enabled = false;
        _charging_light.enabled = false;
        
        _state = State.PASSIVE;
        _audio.pitch = PASSIVE_PITCH;
    }

    /*****************************/
    void updateMovement()
    {
        float speed;
        switch(_state) {
            case State.CHARGING:
                speed = _charging_speed;
                break;
            default:
                speed = _speed;
                break;
        }
        _newpos = transform.position;
        _newpos.x += _direction * speed * Time.deltaTime;
        
        transform.position = _newpos;
        
        // Did we fly off the screen?
        if (_direction == DIR_RIGHT) {
            if (_newpos.x >= X_RANGE_RIGHT ) {
                Done();
            }
        } else if (_direction == DIR_LEFT) {
            if (_newpos.x <= X_RANGE_LEFT ) {
                Done();
            }
        }
    }

    /*****************************/
    void Explode()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Hibernate();
    }

    /*****************************/
    public void HitByLaser( Laserbeam laser )
    {
        GameController.instance.State.AdjustScore(GameConstants.SCORE_UFO);
        Destroy( laser.gameObject );
        Explode();
    }

    /*****************************/
    void Done()
    {
        ShutDownLaser();
        Hibernate();
    }

    /*****************************/
    public new void InstaKill()
    {
        StartCoroutine("InstaKillDelay");
    }

    /*****************************/
    public IEnumerator InstaKillDelay()
    {
        yield return new WaitForSeconds(3.0f);
        Done();
    }

    /*****************************/
    public override void Respawn ()
    {
        float y = Random.Range(MIN_Y_SPAWN, MAX_Y_SPAWN);
        if (Random.Range(0,2) == 0) {
            _direction = DIR_RIGHT;
            _newpos = new Vector3( X_RANGE_LEFT, y, 0 );
        } else {
            _direction = DIR_LEFT;
            _newpos = new Vector3( X_RANGE_RIGHT, y, 0 );
        }
        transform.position = _newpos;
        _state = State.PASSIVE;
        _is_ready = true;
    }
}