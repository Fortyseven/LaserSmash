using UnityEngine;

//TODO: bring down pitch slightly for larger bomb
//TODO: geiger counter sound?

namespace Game
{
    [RequireComponent( typeof( Rigidbody ) )]
    [RequireComponent( typeof( AudioSource ) )]
    public abstract class BaseBomb : GenericEnemy
    {
        protected override float SpawnMaxX { get { return GameConstants.SCREEN_X_BOUNDS; } }
        protected override float SpawnMinX { get { return -GameConstants.SCREEN_X_BOUNDS; } }

        //protected abstract float MinSpeed { get; }
        //protected abstract float MaxSpeed { get; }

        protected float MinSpeed { get { return 1.0f; } }
        protected float MaxSpeed { get { return 7.0f; } }


        private const float Y_OFFSET_FLOOR = 0f;

        public GameObject NukePrefab;

        protected AudioSource _audio;
        protected float _drop_speed;

        /******************************************************************/
        public new void Awake()
        {
            base.Awake();
            IsReady = false;
            _audio = GetComponent<AudioSource>();
        }

        /******************************************************************/
        protected override void OnPause()
        {
            _audio.Pause();
        }

        /******************************************************************/
        protected override void OnResume()
        {
            _audio.Play();
        }

        //private const float MAX_DROP_SPEED = 1.0f;
        /******************************************************************/
        public new void Update()
        {
            if ( !IsReady )
                return;

            Vector3 pos = transform.position;

            //TODO: clean this up
            //float drop_sp = _drop_speed;

            //float mult = Mathf.Sin(GameController.instance.GameEnv.Multiplier/12.0f);
            //Debug.Log( mult );
            //drop_sp *= mult;

            pos.y -= _drop_speed * Time.deltaTime;

            transform.position = pos;

            // Did we go off screen? Sweep it under the rug.
            if ( Mathf.Abs( transform.position.x ) > GameConstants.SCREEN_X_BOUNDS ) {
                Done( false );
                return;
            }

            // Did we hit the ground? Punish player, make noises, explode
            if ( transform.position.y < Y_OFFSET_FLOOR ) {
                OnGroundHit();
                return;
            }

            _audio.pitch = transform.position.y / SpawnYOffset;
        }

        /******************************************************************/
        private void Done( bool explode = true )
        {
            if ( explode ) {
                Instantiate( ExplosionPrefab, transform.position, Quaternion.identity );
            }

            _audio.Stop();
            Hibernate();
        }

        /******************************************************************/
        private void OnGroundHit()
        {
            Instantiate( NukePrefab, transform.position, Quaternion.identity );
            GameController.instance.KillPlayer();
            Done();
        }

        /******************************************************************/
        public override void Respawn()
        {
            base.Respawn();
            _audio.Play();
            IsReady = true;
            _drop_speed = Random.Range( MinSpeed, MaxSpeed );
            //Debug.Log( "Spawning bomb with " + _drop_speed );
        }
    }
}