using Game;
using UnityEngine;

public class Laserbeam : MonoBehaviour
{
    const float SPEED = 50.0f;

    public void OnTriggerEnter( Collider other )
    {
        Debug.Log( "laser collide with " + other.gameObject.name + " -- " + other.gameObject.GetType() );
        //other.transform.root.BroadcastMessage( "HitByLaser", this );
        GenericEnemy ge = other.gameObject.GetComponent<GenericEnemy>();
        if (ge != null) {
            ge.HitByLaser(this);
            Destroy(this.gameObject);
        }
    }

    public void Update()
    {
        transform.Translate( new Vector3( 0.0f, SPEED * Time.deltaTime, 0.0f ), transform );

        if ( transform.position.y > GameConstants.SCREEN_Y_GEN_OFFSET )
            Destroy( gameObject );
    }
}
