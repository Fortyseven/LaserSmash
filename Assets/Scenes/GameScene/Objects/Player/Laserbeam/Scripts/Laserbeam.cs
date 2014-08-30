using UnityEngine;
using System.Collections;
using Game;
public class Laserbeam : MonoBehaviour
{
    const float Speed = 50.0f;

    public void OnTriggerEnter2D( Collider2D other )
    {
        Debug.Log("I hit something: " + other.name);
        other.SendMessage( "HitByLaser", this );
    }
        
    void Update()
    {
        transform.Translate( new Vector3( 0.0f, Speed * Time.deltaTime, 0.0f ), transform );
                
        if ( transform.position.y > GameConstants.SCREEN_Y_GEN_OFFSET )
            Destroy( this.gameObject );
    }
}
