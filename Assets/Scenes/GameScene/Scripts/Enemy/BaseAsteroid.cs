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

        /*****************************/
        public virtual void Awake()
        {
            //_base_gravityscale = rigidbody.mass;
            int enemy_layer = LayerMask.NameToLayer( "Enemy" );
            Physics.IgnoreLayerCollision( enemy_layer, enemy_layer, true );

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            IsReady = false;
        }

        /*****************************/
        public override void HitByLaser( Laserbeam laser )
        {
            Done();
        }

        /*****************************/
        protected void Done( bool explode = true )
        {
            GetComponent<Rigidbody>().velocity = new Vector3( 0, 0, 0 );

            if ( _particle_trail != null ) {
                Destroy( _particle_trail );
            }

            if ( _hit_surface )
                Instantiate( HitSurfacePrefab, transform.position + SurfaceHitOffset, Quaternion.identity );

            if ( explode ) {
                ExplodeAndRecycle();
                GameController.instance.State.AdjustScore( BaseScore );
            }

            Hibernate();
        }

        /*****************************/
        protected virtual void Update()
        {
            if ( !IsReady )
                return;

            // Did we go off screen? Sweep it under the rug.
            if ( Mathf.Abs( transform.position.x ) > GameConstants.SCREEN_X_BOUNDS ) {
                //Debug.Log( "Exceeded screen bounds" );
                Done( false );
                return;
            }

            // Did we hit the ground? Punish player, make noises, explode
            if ( transform.position.y < GameConstants.SCREEN_Y_FLOOR ) {
                //Debug.Log( "Hit surface" );
                GameController.instance.State.AdjustScore( -( BaseScore / 2 ) );
                _hit_surface = true;
                Done( false );
            }
        }

        /*****************************/
        public override void Respawn()
        {
            base.Respawn();

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            float xpush = Random.Range( -1.0f, 1.0f ) * 150.0f;
            //float ypush = -Random.Range( 50.0f, 400.0f );
            float ypush = -Random.Range( 75.0f, 300.0f );

            GetComponent<Rigidbody>().AddForce( xpush, ypush, 0.0f );

            SpawnParticleTrail();
        }

        /*****************************/
        protected void SpawnParticleTrail()
        {
            _particle_trail = Instantiate( ParticleEmitterPrefab, transform.position, Quaternion.identity ) as GameObject;
            if ( _particle_trail )
                _particle_trail.transform.parent = this.transform;
        }
    }
}