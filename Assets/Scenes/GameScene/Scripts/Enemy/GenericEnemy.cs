using UnityEngine;

namespace Game
{
    public abstract class GenericEnemy : MonoBehaviour
    {
        protected abstract float SpawnMaxX { get; }
        protected abstract float SpawnMinX { get; }
        protected virtual float SpawnYOffset { get { return 15.5f; } }

        protected abstract int BaseScore { get; }

        public GameObject ExplosionPrefab;

        /*
         * By default, if an enemy collides with the player ship. Player dies. We die.
         */
        public void OnTriggerEnter( Collider col )
        {
            if ( col.name.Equals( "PlayerShip" ) ) {
                KillPlayer();
            }
        }

        public virtual void HitByLaser(Laserbeam laser)
        {
            GameController.instance.State.AdjustScore( BaseScore );
            Destroy( laser.gameObject );
            ExplodeAndRecycle();
        }

        protected void KillPlayer()
        {
            GameController.instance.PlayerComponent.Kill();
            Hibernate();
        }

        /// <summary>
        /// Spawns an explosion prefab object and hibernates object for later recycling in the object pool.
        /// ExplosionPrefab should be self-terminating.
        /// </summary>
        protected void ExplodeAndRecycle()
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

        /* Ordinarily we'd just directly call Hibernate, and get the hell out of the way, but if 
         * there's any special cleanup required first, you can overload this.  This is mostly used 
         * for keeping the UFO on screen as it's firing, since killing the player InstaKills 
         * everything on the screen. */

        protected virtual void InstaKill()
        {
            Hibernate();
        }

        public bool IsReady { get; set; }

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
