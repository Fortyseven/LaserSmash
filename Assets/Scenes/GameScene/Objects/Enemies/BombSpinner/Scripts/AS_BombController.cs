using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour
{
    public GameObject explosionNuke = null;
    public float BaseGravityScale = 0.01f;

    void Start()
    {
        rigidbody2D.gravityScale = BaseGravityScale * Random.Range( 0.0f, 3.0f );
    }
    
    void Update()
    {
        if (transform.position.y < -1.0f) {
            Instantiate(explosionNuke, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
