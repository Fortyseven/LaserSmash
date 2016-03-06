using UnityEngine;

namespace Game
{
    public abstract class BaseAsteroid : GenericEnemy
    {
        protected override float SpawnMaxX { get { return GameConstants.SCREEN_X_BOUNDS; } }
        protected override float SpawnMinX { get { return -GameConstants.SCREEN_X_BOUNDS; } }

        protected override int BaseScore { get { return GameConstants.SCORE_ASTEROID_SM; } }

        //protected virtual Vector3 SurfaceHitOffset { get { return new Vector3( 0.0f, -0.75f, 0.0f ); } }
        protected virtual Vector3 SurfaceHitOffset { get { return new Vector3( 0.0f, -1.75f, 0.0f ); } }

        public GameObject HitSurfacePrefab = null;
        public GameObject ParticleEmitterPrefab = null;

        protected GameObject _particle_trail = null;
        protected bool _hit_surface;
        protected Rigidbody _rigidbody;
        private Material _my_material;

        /*****************************/
        public new void Awake()
        {
            base.Awake();

            int enemy_layer = LayerMask.NameToLayer( "Enemy" );

            Physics.IgnoreLayerCollision( enemy_layer, enemy_layer, true );

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

            IsReady = false;

            _my_material = new Material( gameObject.GetComponentInChildren<Renderer>().sharedMaterial );
            gameObject.GetComponentInChildren<Renderer>().sharedMaterial = _my_material;
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
                GameController.instance.GameEnv.AdjustScore( BaseScore );
                ExplodeAndHibernate();
                return;
            }

            if ( _hit_surface ) {
                Instantiate( HitSurfacePrefab, transform.position + SurfaceHitOffset, Quaternion.identity );
            }

            Hibernate();
        }

        /*****************************/
        public new void Update()
        {
            base.Update();

            if ( !IsReady )
                return;

            // Did we go off screen? Zap the barrier
            if ( Mathf.Abs( transform.position.x ) > GameConstants.SCREEN_X_BOUNDS ) {
                GameController.instance.Barriers.Zapped( transform.position );
                CleanUpAndHibernate( false );
                return;
            }

            // Did we hit the ground? Punish player, make noises, explode
            if ( transform.position.y < GameConstants.SCREEN_Y_FLOOR ) {
                OnSurfaceKill();
            }
        }

        /*****************************/
        protected virtual void OnSurfaceKill()
        {            
            GameController.instance.GameEnv.AdjustScore( -( BaseScore / 2 ) );
            _hit_surface = true;
            CleanUpAndHibernate( false );
        }

        /*****************************/
        public override void Respawn()
        {
            base.Respawn();

            RecolorMaterial();

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
        protected void RecolorMaterial()
        {
            _my_material.color = Utils.HSVtoRGB( Random.Range( 0, 1.0f ), 0.53f, 0.65f );
            gameObject.GetComponentInChildren<Renderer>().sharedMaterial = _my_material;
        }

        /*****************************/
        protected void SpawnParticleTrail()
        {
            _particle_trail = Instantiate( ParticleEmitterPrefab, transform.position, Quaternion.identity ) as GameObject;
            _particle_trail.transform.SetParent( this.transform );
            _particle_trail.name = "Trail from " + name;
        }

        /*****************************/
        protected override void InstaKill()
        {
            CleanUpAndHibernate( false );
            base.InstaKill();
        }
    }
}