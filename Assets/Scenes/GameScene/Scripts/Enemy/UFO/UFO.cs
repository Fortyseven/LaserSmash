using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace Game
{
    public class UFO : GenericEnemy
    {
        protected override float SpawnMaxX { get { return 18.0f; } }
        protected override float SpawnMinX { get { return -18.0f; } }

        protected override int BaseScore { get { return GameConstants.SCORE_KILLSAT; } }

        public GameObject ExplosionLaserGroundPrefab = null;
        public GameObject EnergyBallPrefab = null;

        protected const float MAX_Y_SPAWN = 13.0f;
        protected const float MIN_Y_SPAWN = 7.5f; //6.0f;

        protected enum Direction
        {
            DIR_RIGHT, // r-t-l
            DIR_LEFT   // l-t-r
        }

        public const float CHARGING_TIME = 1.0f;
        public const float CHARGING_PITCH = 0.75f;
        public const float PASSIVE_PITCH = -0.8f;
        public const float LASER_FADE_TIME = 0.5f;
        public const float TARGET_LOCK_TIME = 0.75f;

        internal float Speed { get; set; }

        protected Direction _direction = 0;

        internal Vector3 _newpos;
        internal LineRenderer _laser;
        internal Light _charging_light;
        internal SpriteRenderer _charging_flare_sprite;
        internal AudioSource _audio;

        internal enum UFOState { PASSIVE, ATTACKING };

        /*****************************/
        public new void Awake()
        {
            _laser = GetComponentInChildren<LineRenderer>();
            _laser.gameObject.SetActive( false );

            _charging_light = GetComponentInChildren<Light>();
            _charging_light.enabled = false;

            _charging_flare_sprite = GetComponentInChildren<SpriteRenderer>();
            _charging_flare_sprite.enabled = false;

            _audio = GetComponent<AudioSource>();

            AddState( new UFO_State_PASSIVE() );
            AddState( new UFO_State_ATTACKING() );

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
                    Hibernate();
                }
            }
            else if ( _direction == Direction.DIR_LEFT ) {
                if ( _newpos.x <= SpawnMinX ) {
                    Hibernate();
                }
            }
        }

        /*****************************/
        internal void ShutDownLaser()
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
        }

        /*****************************/
        public IEnumerator InstaKillDelay()
        {
            yield return new WaitForSeconds( 3.0f );
            Hibernate();
        }

        /*****************************/
        protected override void OnHibernateCallback()
        {
            GameController.instance.GameEnv.WaveCon.UFOsISpawned = false;
            ChangeState( UFOState.PASSIVE );
            ShutDownLaser();
        }

        /*****************************/
        public override void Respawn()
        {
            // Decide the height
            float y_offs = Random.Range( MIN_Y_SPAWN, MAX_Y_SPAWN );

            // Pick a side of the screen to fly out of
            if ( Random.Range( 0, 2 ) == 0 ) {
                _direction = Direction.DIR_RIGHT;
                _newpos = new Vector3( SpawnMinX, y_offs, 0 );
            }
            else {
                _direction = Direction.DIR_LEFT;
                _newpos = new Vector3( SpawnMaxX, y_offs, 0 );
            }

            transform.position = _newpos;
            GameController.instance.GameEnv.WaveCon.UFOsISpawned = true;
            IsReady = true;

            ChangeState( UFOState.PASSIVE );

            //Debug.Log( "UFO RESPAWNED" );
        }
    }
}