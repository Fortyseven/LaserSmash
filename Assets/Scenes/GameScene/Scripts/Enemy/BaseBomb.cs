using Game;
using UnityEngine;

//TODO: bring down pitch slightly for larger bomb
//TODO: geiger counter sound?

namespace Assets.Scenes.GameScene.Scripts.Enemy
{
    [RequireComponent( typeof( Rigidbody ) )]
    [RequireComponent( typeof( AudioSource ) )]
    public abstract class BaseBomb : GenericEnemy
    {
        protected override float SpawnMaxX { get { return 11.0f; } }
        protected override float SpawnMinX { get { return -11.0f; } }
        
        protected abstract float MinForce { get; }
        protected abstract float MaxForce { get; }

        private const float Y_OFFSET_FLOOR = 0f;

        public GameObject NukePrefab;

        protected float _base_mass_mult;
        protected bool _hit_surface;

        protected AudioSource _audio;
        protected Rigidbody _rigidbody;

        /******************************************************************/
        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _base_mass_mult = _rigidbody.mass;
            IsReady = false;
            _audio = GetComponent<AudioSource>();
        }

        /******************************************************************/
        public void Update()
        {
            if ( !IsReady )
                return;

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
            //if (_hit_surface)

            _rigidbody.velocity = new Vector3( 0, 0, 0 );

            if ( explode )
                Instantiate( ExplosionPrefab, transform.position, Quaternion.identity );

            _audio.Stop();
            Hibernate();
        }

        /******************************************************************/
        private void OnGroundHit()
        {
            Instantiate( NukePrefab, transform.position, Quaternion.identity );
            GameController.instance.PlayerComponent.Kill();
            _hit_surface = true;
            Done();
        }

        /******************************************************************/
        public override void Respawn()
        {
            base.Respawn();
            _audio.Play();
            _hit_surface = false;
            IsReady = true;
            _rigidbody.AddForce( new Vector3( 0, -( Random.Range( MinForce, MaxForce ) ), 0 ) );
        }
    }
}