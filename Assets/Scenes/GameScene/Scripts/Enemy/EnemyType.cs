using UnityEngine;

//public abstract class EnemyType : MonoBehaviour
//{
//    public abstract void Respawn();


//    /* Ordinarily we'd just hibernate, and get the hell out of the way, but if there's any special
//     * cleanup required first, you can overload this.  This is mostly used for keeping the UFO on 
//     * screen as it's firing, since killing the player InstaKills everything on the screen. */

//    public void InstaKill()
//    {
//        Hibernate();
//    }

//    protected bool _is_ready = false;

//    public void Hibernate()
//    {
//        gameObject.SetActive( false );
//        transform.position.Set( 0, 0, 0 );
//        _is_ready = false;
//    }
//}
