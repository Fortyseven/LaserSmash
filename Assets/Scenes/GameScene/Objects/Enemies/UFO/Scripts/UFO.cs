using UnityEngine;
using System.Collections;
using Game;

/* TODO: Player should have split second ability to dodge laser; maybe 0.15ms delay? */

public class UFO : MonoBehaviour
{
    public GameObject ExplosionPrefab = null;
    public GameObject ExplosionLaserGroundPrefab = null;

    const float MAX_Y_SPAWN = 14.0f;
    const float MIN_Y_SPAWN = 8.7f;
    const float X_RANGE_RIGHT = 15.0f;
    const float X_RANGE_LEFT = -16.0f;
    const int DIR_RIGHT = 1; // r-t-l
    const int DIR_LEFT = -1; // l-t-r
    const float CHARGING_PITCH = 0.75f;
    const float PASSIVE_PITCH = -0.8f;
    const float LASER_FADE_TIME = 0.5f;
    const float LASER_FADE_GRANULARITY = 0.05f;

    const float CHARGING_TIME = 1.0f;

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
        float y = Random.Range(MIN_Y_SPAWN, MAX_Y_SPAWN);
        if (Random.Range(0,2) == 0) {
            _direction = DIR_RIGHT;
            _newpos = new Vector3( X_RANGE_LEFT, y, 0 );
        } else {
            _direction = DIR_LEFT;
            _newpos = new Vector3( X_RANGE_RIGHT, y, 0 );
        }
        transform.position = _newpos;

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
        switch(_state) {
            case State.PASSIVE:
                // Occasionally fire down hot death
                if (Random.Range(0, 250) == 0) {
                    _state = State.CHARGING;
                    _charging_flare_sprite.enabled = true;
                    _charging_light.enabled = true;
                    _time_started_charging = Time.time;
                    _audio.pitch = CHARGING_PITCH;
                }
                break;
            case State.CHARGING:
                if (Time.time - _time_started_charging  > CHARGING_TIME) {
                    _player_target_position = GameController.instance.PlayerShip.transform.position;
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
    IEnumerator Fire()
    {
        _state = State.FIRING;
        _laser.gameObject.SetActive(true);

        Instantiate(ExplosionLaserGroundPrefab, _player_target_position, Quaternion.identity);

        // Fade out the beam over LASER_FADE_TIME seconds
        Color col = _laser.material.GetColor("_TintColor");
        float del = LASER_FADE_TIME * LASER_FADE_GRANULARITY;

        for(float i = 0; i < 1.0f; i+=LASER_FADE_GRANULARITY) {
            Debug.Log(i);
            col.a = 1.0f - i;
            _laser.material.SetColor("_TintColor", col);
            yield return new WaitForSeconds(del);
        }

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
                Destroy(this.gameObject);
            }
        } else if (_direction == DIR_LEFT) {
            if (_newpos.x <= X_RANGE_LEFT ) {
                Destroy(this.gameObject);
            }
        }
    }

    /*****************************/
    void Explode()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    /*****************************/
    public void HitByLaser( Laserbeam laser )
    {
        GameController.instance.AdjustScore(GameConstants.SCORE_ASTEROID_BYLASER);
        Destroy( laser.gameObject );
        Explode();
    }
}