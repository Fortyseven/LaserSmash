using System;
using UnityEngine;
using System.Collections;
using Game;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

public class UFO : GenericEnemy
{
    protected override float SpawnMaxX
    { get { return 18.0f; } }
    protected override float SpawnMinX
    { get { return -18.0f; } }

    protected override int BaseScore
    { get { return GameConstants.SCORE_KILLSAT; } }

    public GameObject ExplosionLaserGroundPrefab = null;

    protected const float MAX_Y_SPAWN = 13.0f;
    protected const float MIN_Y_SPAWN = 6.0f;

    protected enum Direction
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

    protected Direction _direction = 0;
    protected float _speed = 10.0f;
    protected float _charging_speed = 3.0f;
    protected Vector3 _newpos;

    private LineRenderer _laser;
    private Light _charging_light;
    private SpriteRenderer _charging_flare_sprite;
    private AudioSource _audio;

    private StateMachine _state_machine;

    private enum State
    {
        PASSIVE,
        ATTACK
    };

    /***************************************************************/
    private class State_PASSIVE : StateBehavior
    {
        private const float PERCENT_CHANCE_OF_FIRING = 1.0f;
        public override void OnUpdate()
        {
            // Occasionally fire down hot death
            if ( GameController.instance.CurrentState.Name.Equals( GameController.NewGameState.RUNNING ) &&
                 ( Random.Range( 0, 100 ) <= PERCENT_CHANCE_OF_FIRING ) ) {
                Machine.SwitchStateTo( State.ATTACK );
            }
        }
    }

    /***************************************************************/
    private class State_CHARGING : StateBehavior
    {
        private float _time_started_charging;
        private Vector3 _player_target_position;
        private bool _has_fired;

        public override void OnEnter( Enum changing_from )
        {
            ( (UFO)Parent )._charging_flare_sprite.enabled = true;
            ( (UFO)Parent )._charging_light.enabled = true;
            ( (UFO)Parent )._audio.pitch = CHARGING_PITCH;

            _time_started_charging = Time.time;
            _has_fired = false;

#if DEVELOPMENT_BUILD
            if ( CHARGING_TIME < TARGET_LOCK_TIME ) {
                throw new UnityException( "TARGET_LOCK_TIME must be less than CHARGING_TIME" );
            }
#endif
            StartCoroutine( "AcquireTargetLock" );
        }

        public override void OnUpdate()
        {
            if ( ( Time.time - _time_started_charging ) > CHARGING_TIME && !_has_fired ) {
                StartCoroutine( "Fire" );
            }
            else {
                // Laser source should follow ship's slow movement
                ( (UFO)Parent )._laser.SetPosition( 1, transform.position );
            }

        }

        /// <summary>
        /// Wait for TARGET_LOCK_TIME seconds and then log where the player was; fire at that position.
        /// This is used so that there's a chance for the player to dodge the shot.
        /// </summary>
        [UsedImplicitly]
        public IEnumerator AcquireTargetLock()
        {
            yield return new WaitForSeconds( TARGET_LOCK_TIME );
            _player_target_position = ( (UFO)Parent ).GameEnvironment.PlayerShip.transform.position;
        }

        /// <summary>
        /// I'm-a-firin' muh laz0r.
        /// </summary>
        [UsedImplicitly]
        public IEnumerator Fire()
        {
            _has_fired = true;

            ( (UFO)Parent )._laser.SetPosition( 1, transform.position );
            ( (UFO)Parent )._laser.SetPosition( 0, _player_target_position );

            ( (UFO)Parent )._laser.gameObject.SetActive( true );


            Instantiate( ( (UFO)Parent ).ExplosionLaserGroundPrefab, _player_target_position, Quaternion.identity );

            // Check for collision
            Ray2D r = new Ray2D( transform.position, ( _player_target_position - transform.position ) * 15 );

            /* We COULD check if this is the player being hit, but all the enemies are on layer 8, and
               nothing else with a collider exists on any other layer but the player. */


            //FIXME
            //if ( GameController.instance.Status.Mode == GameStatus.GameMode.RUNNING ) {
            //    if (/*hit = */Physics2D.Raycast( r.origin, r.direction, Mathf.Infinity, 1 | 8 ) ) {
            //        ( (UFO)Parent ).KillPlayer();
            //    }
            //}

            // Fade out the beam over LASER_FADE_TIME seconds
            Color col = ( (UFO)Parent )._laser.material.GetColor( "_TintColor" );

            for ( float i = 0; i < 1.0f; i += LASER_FADE_GRANULARITY ) {
                col.a = 1.0f - i;
                ( (UFO)Parent )._laser.material.SetColor( "_TintColor", col );
                Debug.Log( "foo " + i );
                yield return new WaitForSeconds( LASER_FADE_TIME * LASER_FADE_GRANULARITY );
            }

            Machine.SwitchStateTo( State.PASSIVE );
        }

        public override void OnExit( Enum changing_to )
        {
            ( (UFO)Parent ).ShutDownLaser();
            _has_fired = false;
        }
    }

    /*****************************/
    public override void Awake()
    {
        // Pick a side of the screen to fly out of
        _laser = GetComponentInChildren<LineRenderer>();
        _laser.gameObject.SetActive( false );

        _charging_light = GetComponentInChildren<Light>();
        _charging_light.enabled = false;

        _charging_flare_sprite = GetComponentInChildren<SpriteRenderer>();
        _charging_flare_sprite.enabled = false;

        _audio = GetComponent<AudioSource>();

        _state_machine = gameObject.AddComponent<StateMachine>();
        _state_machine.Init( this );

        _state_machine.AddState<State_PASSIVE>( State.PASSIVE );
        _state_machine.AddState<State_CHARGING>( State.ATTACK );

        _state_machine.SwitchStateTo( State.PASSIVE );
    }

    /*****************************/
    public void Update()
    {
        if ( !IsReady )
            return;

        UpdateMovement();
    }

    /*****************************/
    private void UpdateMovement()
    {
        float speed;
        switch ( (State)_state_machine.ActiveState ) {
            case State.ATTACK:
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
        _state_machine.SwitchStateTo( State.PASSIVE );
        ShutDownLaser();
        Hibernate();
    }

    /*****************************/
    private void ShutDownLaser()
    {
        _laser.gameObject.SetActive( false );

        _charging_flare_sprite.enabled = false;
        _charging_light.enabled = false;

        _audio.pitch = PASSIVE_PITCH;
    }

    /*****************************/
    protected override void InstaKill()
    {
        //StartCoroutine( "InstaKillDelay" );
        Done();
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

        IsReady = true;
    }
}