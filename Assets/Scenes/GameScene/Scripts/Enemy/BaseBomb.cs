using Game;
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

        protected abstract float MinSpeed { get; }
        protected abstract float MaxSpeed { get; }

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
        public override void OnPause()
        {
            _audio.Pause();
        }

        /******************************************************************/
        public override void OnResume()
        {
            _audio.Play();
        }

        /******************************************************************/
        public new void Update()
        {
            if ( !IsReady )
                return;

            Vector3 pos = transform.position;

            //TODO: clean this up
            float drop_sp = _drop_speed;

            float mult = Mathf.Sin(GameController.instance.GameEnv.Multiplier/6.0f);
            Debug.Log( mult );
            drop_sp *= mult;

            pos.y -= drop_sp * Time.deltaTime;

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
        }
    }
}