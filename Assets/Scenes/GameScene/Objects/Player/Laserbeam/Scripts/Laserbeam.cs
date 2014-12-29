using Game;
using UnityEngine;

public class Laserbeam : MonoBehaviour
{
    const float SPEED = 50.0f;

    public void OnTriggerEnter2D( Collider2D other )
    {
        other.transform.root.BroadcastMessage( "HitByLaser", this );
    }

    void Update()
    {
        transform.Translate( new Vector3( 0.0f, SPEED * Time.deltaTime, 0.0f ), transform );

        if ( transform.position.y > GameConstants.SCREEN_Y_GEN_OFFSET )
            Destroy( this.gameObject );
    }
}
