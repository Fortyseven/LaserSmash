using UnityEngine;

namespace Game
{
    public abstract class BaseAsteroid : GenericEnemy
    {
        protected override float SpawnMaxX { get { return GameConstants.SCREEN_X_BOUNDS; } }
        protected override float SpawnMinX { get { return -GameConstants.SCREEN_X_BOUNDS; } }

        protected override int BaseScore { get { return GameConstants.SCORE_ASTEROID_SM; } }

        protected virtual Vector3 SurfaceHitOffset { get { return new Vector3( 0.0f, -0.75f, 0.0f ); } }

        //public AudioClip SoundHitSurface = null;

        public GameObject HitSurfacePrefab = null;
        //public GameObject PlayerObject = null;
        //public GameObject AsteroidSmallPrefab = null;
        public GameObject ParticleEmitterPrefab = null;

        //protected float _gravity_multiplier = 0.0f;
        protected GameObject _particle_trail = null;
        protected bool _hit_surface;
        protected Rigidbody _rigidbody;

        /*****************************/
        public virtual void Awake()
        {
            int enemy_layer = LayerMask.NameToLayer( "Enemy" );
            Physics.IgnoreLayerCollision( enemy_layer, enemy_layer, true );
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
            IsReady = false;
        }

        /*****************************/
        public override void HitByLaser( Laserbeam laser )
        {
            CleanUpAndHibernate();
        }

        /*****************************/
        /// <summary>
        /// This call performs pre-hibernation shutdown chores for the object, such as stopping 
        /// movement, spawning ground puffs, destroying the particle trail, and optionally exploding.
        /// </summary>
        /// <param name="destroyed_by_player">If true, explode and add to the player score.</param>
        protected void CleanUpAndHibernate( bool destroyed_by_player = true )
        {
            _rigidbody.velocity = new Vector3( 0, 0, 0 );

            if ( _particle_trail ) {
                Destroy( _particle_trail, 0.05f );
            }

            if ( destroyed_by_player ) {
                GameController.instance.State.AdjustScore( BaseScore );
                ExplodeAndHibernate();
                return;
            }

            if ( _hit_surface ) {
                Instantiate( HitSurfacePrefab, transform.position + SurfaceHitOffset, Quaternion.identity );
            }

            Hibernate();
        }

        /*****************************/
        protected void Update()
        {
            if ( !IsReady )
                return;

            // Did we go off screen? Sweep it under the rug.
            if ( Mathf.Abs( transform.position.x ) > GameConstants.SCREEN_X_BOUNDS ) {
                //Debug.Log( "Exceeded screen bounds" );
                CleanUpAndHibernate( false );
                return;
            }

            // Did we hit the ground? Punish player, make noises, explode
            if ( transform.position.y < GameConstants.SCREEN_Y_FLOOR ) {
                //Debug.Log( "Hit surface" );
                GameController.instance.State.AdjustScore( -( BaseScore / 2 ) );
                _hit_surface = true;
                CleanUpAndHibernate( false );
            }
        }

        /*****************************/
        public override void Respawn()
        {
            base.Respawn();

            _rigidbody.velocity = Vector3.zero;
            float xpush = Random.Range( -1.0f, 1.0f ) * 150.0f;
            //float ypush = -Random.Range( 50.0f, 400.0f );
            float ypush = -Random.Range( 75.0f, 300.0f );

            _rigidbody.AddForce( xpush, ypush, 0.0f );
            if ( _particle_trail ) {
                // I could delete it here, but let's dig deeper to find the REAL cause
            }
            SpawnParticleTrail();
        }

        /*****************************/
        protected void SpawnParticleTrail()
        {
            _particle_trail = Instantiate( ParticleEmitterPrefab, transform.position, Quaternion.identity ) as GameObject;
            _particle_trail.transform.SetParent( this.transform );
            _particle_trail.name = "Trail from " + name;
        }

        protected override void InstaKill()
        {
            CleanUpAndHibernate( false );
            base.InstaKill();
        }
    }
}