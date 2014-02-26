using UnityEngine;
using System.Collections;

public class Laserbeam : MonoBehaviour
{
    const float Speed = 5.0f;

    public void OnTriggerEnter2D( Collider2D other )
    {
        other.SendMessage( "HitByLaser", this );
    }
        
    void Update()
    {
        transform.Translate( new Vector3( 0.0f, Speed * Time.deltaTime, 0.0f ), transform );
                
        if ( transform.position.y > 1.5f )
            Destroy( this.gameObject );
    }
}
