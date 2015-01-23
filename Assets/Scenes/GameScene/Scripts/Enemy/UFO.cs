using UnityEngine;
using System.Collections;
using Game;

public class UFO : GenericEnemy
{
    protected override float SpawnMaxX { get { return 18.0f; } }
    protected override float SpawnMinX { get { return -18.0f; } }

    protected override int BaseScore { get { return GameConstants.SCORE_KILLSAT; } }

    //public GameObject ExplosionPrefab = null;
    public GameObject ExplosionLaserGroundPrefab = null;

    const float MAX_Y_SPAWN = 13.0f;
    const float MIN_Y_SPAWN = 6.0f;

    private enum Direction
    {
        DIR_RIGHT, // r-t-l
        DIR_LEFT   // l-t-r
    }

    private const float CHARGING_PITCH = 0.75f;
    private const float PASSIVE_PITCH = -0.8f;
    private const float LASER_FADE_TIME = 0.5f;
    private const float LASER_FADE_GRANULARITY = 0.05f;

    private const float CHARGING_TIME = 1.0f;
    private const float TARGET_LOCK_TIME = 0.75f;

    private Direction _direction = 0;
    private float _speed = 10.0f;
    private float _charging_speed = 3.0f;
    private Vector3 _newpos;
    private Vector3 _player_target_position;

    private LineRenderer _laser;
    private Light _charging_light;
    private SpriteRenderer _charging_flare_sprite;
    private AudioSource _audio;

    private enum State
    {
        PASSIVE,
        CHARGING,
        FIRING
    };

    float _time_started_charging;
    State _state = State.PASSIVE;

    /*****************************/
    public void Awake()
    {
        // Pick a side of the screen to fly out of
        _laser = GetComponentInChildren<LineRenderer>();
        _laser.gameObject.SetActive( false );

        _charging_light = GetComponentInChildren<Light>();
        _charging_light.enabled = false;

        _charging_flare_sprite = GetComponentInChildren<SpriteRenderer>();
        _charging_flare_sprite.enabled = false;

        _audio = GetComponent<AudioSource>();
    }

    /*****************************/
    public void Update()
    {
        if ( !IsReady )
            return;

        switch ( _state ) {
            case State.PASSIVE:
                // Occasionally fire down hot death
                if ( GameController.instance.State.Mode == GameState.GameMode.RUNNING && Random.Range( 0, 250 ) == 0 ) {
                    _state = State.CHARGING;
                    _charging_flare_sprite.enabled = true;
                    _charging_light.enabled = true;
                    _time_started_charging = Time.time;
                    StartCoroutine( "AcquireTargetLock" );
                    _audio.pitch = CHARGING_PITCH;
                }
                break;
            case State.CHARGING:
                if ( Time.time - _time_started_charging > CHARGING_TIME ) {
                    StartCoroutine( "Fire" );
                }
                break;
            case State.FIRING:
                _laser.SetPosition( 1, transform.position );
                _laser.SetPosition( 0, _player_target_position );
                break;
        }

        UpdateMovement();
    }

    /*****************************/
    protected IEnumerator AcquireTargetLock()
    {
        yield return new WaitForSeconds( TARGET_LOCK_TIME );
        _player_target_position = GameController.instance.PlayerShip.transform.position;
    }

    /*****************************/
    protected IEnumerator Fire()
    {
        _state = State.FIRING;
        _laser.gameObject.SetActive( true );

        Instantiate( ExplosionLaserGroundPrefab, _player_target_position, Quaternion.identity );

        // Check for collision
        Ray2D r = new Ray2D( transform.position, ( _player_target_position - transform.position ) * 15 );

        /* We COULD check if this is the player being hit, but all the enemies are on layer 8, and
           nothing else with a collider exists on any other layer but the player. */

        if ( GameController.instance.State.Mode == GameState.GameMode.RUNNING ) {
            if (/*hit = */Physics2D.Raycast( r.origin, r.direction, Mathf.Infinity, 1 | 8 ) ) {
                KillPlayer();
            }
        }

        // Fade out the beam over LASER_FADE_TIME seconds
        Color col = _laser.material.GetColor( "_TintColor" );

        for ( float i = 0; i < 1.0f; i += LASER_FADE_GRANULARITY ) {
            col.a = 1.0f - i;
            _laser.material.SetColor( "_TintColor", col );
            yield return new WaitForSeconds( LASER_FADE_TIME * LASER_FADE_GRANULARITY );
        }

        ShutDownLaser();
    }

    /*****************************/
    private void ShutDownLaser()
    {
        _laser.gameObject.SetActive( false );

        _charging_flare_sprite.enabled = false;
        _charging_light.enabled = false;

        _state = State.PASSIVE;
        _audio.pitch = PASSIVE_PITCH;
    }

    /*****************************/
    private void UpdateMovement()
    {
        float speed;
        switch ( _state ) {
            case State.CHARGING:
                speed = _charging_speed;
                break;
            default:
                speed = _speed;
                break;
        }
        _newpos = transform.position;
        _newpos.x += ( _direction == Direction.DIR_LEFT ? -1 : 1 ) * speed * Time.deltaTime;

        transform.position = _newpos;

        // Did we fly off the screen?
        if ( _direction == Direction.DIR_RIGHT ) {
            if ( _newpos.x >= SpawnMaxX ) {
                Done();
            }
        }
        else if ( _direction == Direction.DIR_LEFT ) {
            if ( _newpos.x <= SpawnMinX ) {
                Done();
            }
        }
    }

    /*****************************/
    void Done()
    {
        ShutDownLaser();
        Hibernate();
    }

    /*****************************/
    protected override void InstaKill()
    {
        StartCoroutine( "InstaKillDelay" );
    }

    /*****************************/
    public IEnumerator InstaKillDelay()
    {
        yield return new WaitForSeconds( 3.0f );
        Done();
    }

    /*****************************/
    public override void Respawn()
    {
        float y_offs = Random.Range( MIN_Y_SPAWN, MAX_Y_SPAWN );

        if ( Random.Range( 0, 2 ) == 0 ) {
            _direction = Direction.DIR_RIGHT;
            _newpos = new Vector3( SpawnMinX, y_offs, 0 );
        }
        else {
            _direction = Direction.DIR_LEFT;
            _newpos = new Vector3( SpawnMaxX, y_offs, 0 );
        }
        transform.position = _newpos;
        _state = State.PASSIVE;
        IsReady = true;
    }
}