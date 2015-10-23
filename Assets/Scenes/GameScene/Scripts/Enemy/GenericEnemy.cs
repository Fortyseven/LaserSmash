using UnityEngine;

namespace Game
{
    public abstract class GenericEnemy : StateMachineMB
    {
        protected abstract float SpawnMaxX { get; }
        protected abstract float SpawnMinX { get; }
        protected virtual float SpawnYOffset { get { return 15.5f; } }

        protected abstract int BaseScore { get; }

        public bool IsReady { get; set; }

        public GameObject ExplosionPrefab;

        public virtual void Awake()
        {
            tag = "Enemy";
        }

        /// <summary>
        /// By default, if an enemy collides with the player ship. Player dies. We die.
        /// </summary>
        /// <param name="col">Who colided with us.</param>
        public void OnTriggerEnter( Collider col )
        {
            if ( col.tag.Equals( "Player" ) ) {
                KillPlayer();
            }
        }

        public virtual void OnPause()
        {
            ;
        }

        public virtual void OnResume()
        {
            ;
        }

        public virtual void HitByLaser( Laserbeam laser )
        {
            ExplodeAndHibernate();
            GameController.instance.GameEnv.AdjustScore( BaseScore );
        }

        protected void KillPlayer()
        {
            //_game_environment.PlayerComponent.ChangeState( Player.PlayerState.KILLED );
            GameController.instance.KillPlayer();
            Hibernate();
        }

        /// <summary>
        /// Spawns an explosion prefab object and hibernates object for later recycling in the object pool.
        /// ExplosionPrefab should be self-terminating.
        /// </summary>
        protected void ExplodeAndHibernate()
        {
            if ( ExplosionPrefab != null ) {
                Instantiate( ExplosionPrefab, transform.position, Quaternion.identity );
            }
            Hibernate();
        }

        /// <summary>
        ///  This is called when the object is recycled out of the object pool and put back into active use.
        /// </summary>
        public virtual void Respawn()
        {
            transform.position = new Vector3( Random.Range( SpawnMinX, SpawnMaxX ), SpawnYOffset, 0 );
        }

        /// <summary>
        /// Ordinarily we'd just directly call Hibernate, and get the hell out of the way, but if 
        /// there's any special cleanup required first, you can overload this.  This is mostly used 
        /// for keeping the UFO on screen as it's firing, since killing the player InstaKills 
        /// everything on the screen.
        /// </summary>

        protected virtual void InstaKill()
        {
            Hibernate();
        }

        /// <summary>
        /// Immediately deactivates object and puts it back into the pool for recycling.
        /// </summary>
        public void Hibernate()
        {

            gameObject.SetActive( false );
            transform.position.Set( 0, 0, 0 );
            IsReady = false;
        }
    }
}
