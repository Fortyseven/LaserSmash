using UnityEngine;

namespace Game
{
    public abstract class BaseAsteroid : GenericEnemy
    {
        protected override float SpawnMaxX { get { return 12.0f; } }
        protected override float SpawnMinX { get { return -12.0f; } }

        protected override int BaseScore { get { return GameConstants.SCORE_ASTEROID_SM; } }

        protected virtual Vector3 SurfaceHitOffset { get { return new Vector3( 0.0f, -0.75f, 0.0f ); } }

        public AudioClip SoundHitSurface = null;

        public GameObject HitSurfacePrefab = null;
        public GameObject PlayerObject = null;
        public GameObject AsteroidSmallPrefab = null;
        public GameObject ParticleEmitterPrefab = null;

        protected float _gravity_multiplier = 0.0f;
        protected GameObject _particle_trail = null;
        protected bool _hit_surface;
        protected float _base_gravityscale;

        /*****************************/
        protected virtual void Awake()
        {
            _base_gravityscale = rigidbody2D.gravityScale;
            IsReady = false;
        }

        /*****************************/
        protected void Done( bool explode = true )
        {
            rigidbody2D.velocity = new Vector2( 0, 0 );

            if ( _particle_trail != null ) {
                Destroy( _particle_trail );
            }

            if ( explode )
                Instantiate( ExplosionPrefab, transform.position, Quaternion.identity );

            if ( _hit_surface )
                Instantiate( HitSurfacePrefab, transform.position + SurfaceHitOffset, Quaternion.identity );

            Hibernate();
        }

        /*****************************/
        protected virtual void Update()
        {
            if ( !IsReady )
                return;

            // Did we go off screen? Sweep it under the rug.
            if ( Mathf.Abs( transform.position.x ) > GameConstants.SCREEN_X_BOUNDS ) {
                Done( false );
                return;
            }

            // Did we hit the ground? Punish player, make noises, explode
            if ( transform.position.y < GameConstants.SCREEN_Y_FLOOR ) {
                GameController.instance.State.AdjustScore( -( BaseScore / 2 ) );
                _hit_surface = true;
                Done( false );
                return;
            }

            if ( _particle_trail != null )
                _particle_trail.transform.position = transform.position;
        }
    }
}