using System;
using UnityEngine;
using System.Collections;
using Game;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

public class UFO : GenericEnemy
{
    protected override float SpawnMaxX { get { return 18.0f; } }
    protected override float SpawnMinX { get { return -18.0f; } }

    protected override int BaseScore { get { return GameConstants.SCORE_KILLSAT; } }

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
    private const float CHARGING_TIME = 1.0f;
    private const float TARGET_LOCK_TIME = 0.75f;

    private float Speed { get; set; }

    protected Direction _direction = 0;
    protected Vector3 _newpos;

    private LineRenderer _laser;
    private Light _charging_light;
    private SpriteRenderer _charging_flare_sprite;
    private AudioSource _audio;

    private enum UFOState { PASSIVE, ATTACKING };

    /*******************************************************************************/
    private class State_PASSIVE : State
    {
        private const float PASSIVE_SPEED = 10.0f;
        private const float PERCENT_CHANCE_OF_FIRING = 1.0f;

        public override Enum Name { get { return UFOState.PASSIVE; } }

        public override void OnStateEnter( State from_state )
        {
            ( (UFO)Owner ).Speed = PASSIVE_SPEED;
        }

        public override void OnUpdate()
        {
            // Occasionally fire down hot death
            if ( GameController.instance.CurrentState.Name.Equals( GameController.GameState.RUNNING ) &&
                 ( Random.Range( 0, 100 ) <= PERCENT_CHANCE_OF_FIRING ) ) {
                Owner.ChangeState( UFOState.ATTACKING );
            }
        }
    }

    /*******************************************************************************/
    private class State_ATTACKING : State
    {
        private const float CHARGING_SPEED = 3.0f;
        private float _time_started_charging;
        private Vector3 _player_target_position;
        private bool _has_fired;

        public override Enum Name { get { return UFOState.ATTACKING; } }

        public override void OnStateEnter( State from_state )
        {
            ( (UFO)Owner ).Speed = CHARGING_SPEED;

            ( (UFO)Owner )._charging_flare_sprite.enabled = true;
            ( (UFO)Owner )._charging_light.enabled = true;
            ( (UFO)Owner )._audio.pitch = CHARGING_PITCH;

            _time_started_charging = Time.time;
            _has_fired = false;

#if DEVELOPMENT_BUILD
            if ( CHARGING_TIME < TARGET_LOCK_TIME ) {
                throw new UnityException( "TARGET_LOCK_TIME must be less than CHARGING_TIME" );
            }
#endif
            // Immediately acquire target lock, leading to firing
            OwnerMB.StartCoroutine( AcquireTargetLock() );
        }

        public override void OnStateExit( State to_state )
        {
            ( (UFO)Owner ).ShutDownLaser();
            _has_fired = false;
        }

        public override void OnUpdate()
        {
            // Laser SOURCE should always follow ship's slow movement, otherwise beam will freeze in space
            ( (UFO)Owner )._laser.SetPosition( 1, OwnerMB.transform.position );

            if ( _has_fired ) return;

            if ( ( Time.time - _time_started_charging ) > CHARGING_TIME ) {
                OwnerMB.StartCoroutine( Fire() );
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
            _player_target_position = GameController.instance.GameEnv.PlayerShip.transform.position;
        }

        /// <summary>
        /// I'm-a-firin' muh laz0r.
        /// </summary>
        [UsedImplicitly]
        public IEnumerator Fire()
        {
            _has_fired = true;

            ( (UFO)Owner )._laser.SetPosition( 1, OwnerMB.transform.position );
            ( (UFO)Owner )._laser.SetPosition( 0, _player_target_position );

            ( (UFO)Owner )._laser.gameObject.SetActive( true );


            Instantiate( ( (UFO)Owner ).ExplosionLaserGroundPrefab, _player_target_position, Quaternion.identity );

            // Check for collision
            var r = new Ray(OwnerMB.transform.position, (_player_target_position - OwnerMB.transform.position) * 2);


            Debug.DrawRay( OwnerMB.transform.position, ( _player_target_position - OwnerMB.transform.position ) * 2 );

            /* We COULD check if this is the player being hit, but all the enemies are on layer 8, and
               nothing else with a collider exists on any other layer but the player. */


            // UFOs can be flying about during a Game Over state, but we only care about killing
            // the player when we're actually in a game
            if ( GameController.instance.CurrentState == GameController.GameState.RUNNING ) {
                if ( Physics.Raycast( r.origin, r.direction, Mathf.Infinity, 1 << 12 ) ) {
                    Debug.Log( "KABOOM" );
                    ( (UFO)Owner ).KillPlayer();
                }
            }

            Debug.Log( "Beginning fade" );

            // Fade out the beam over LASER_FADE_TIME seconds
            Color col = ((UFO) Owner)._laser.material.GetColor("_TintColor");

            float timer = LASER_FADE_TIME;

            while ( timer > 0 ) {
                Debug.Log( "Fade: " + timer );
                timer -= Time.deltaTime;

                //alpha -= ( 1.0f / LASER_FADE_TIME ) * Time.deltaTime;
                col.a = ( ( 1.0f / LASER_FADE_TIME ) * timer );

                ( (UFO)Owner )._laser.material.SetColor( "_TintColor", col );

                yield return null;
            }

            Debug.Log( "Finished fade, leaving state" );
            Owner.ChangeState( UFOState.PASSIVE );
        }
    }

    /*****************************/
    public new void Start()
    {
        // Pick a side of the screen to fly out of
        _laser = GetComponentInChildren<LineRenderer>();
        _laser.gameObject.SetActive( false );

        _charging_light = GetComponentInChildren<Light>();
        _charging_light.enabled = false;

        _charging_flare_sprite = GetComponentInChildren<SpriteRenderer>();
        _charging_flare_sprite.enabled = false;

        _audio = GetComponent<AudioSource>();

        AddState( new State_PASSIVE() );
        AddState( new State_ATTACKING() );

        ChangeState( UFOState.PASSIVE );
    }

    /*****************************/
    [UsedImplicitly]
    public new void Update()
    {
        if ( !IsReady )
            return;

        base.Update();

        UpdateMovement();
    }

    /*****************************/
    private void UpdateMovement()
    {
        _newpos = transform.position;
        _newpos.x += ( _direction == Direction.DIR_LEFT ? -1 : 1 ) * Speed * Time.deltaTime;

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
        ChangeState( UFOState.PASSIVE );
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