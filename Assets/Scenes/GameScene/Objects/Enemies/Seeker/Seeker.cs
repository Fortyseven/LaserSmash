using UnityEngine;
using System.Collections;

public class Seeker : EnemyType
{
    private const float SPEED = 5.0f;

    void Awake()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        //Vector3 pos = transform.position;
        //pos.y += Time.deltaTime * SPEED;
        //transform.Translate(transform.forward);
        if (transform.position.y > 0.5f ) {
            transform.position = Vector3.MoveTowards(transform.position, GameController.instance.PlayerShip.transform.position, Time.deltaTime * 8 );
        } else {
            Destroy(this.gameObject);
        }

        Respawn();
    }

    public override void Respawn ()
    {
        if (transform.position.y > 0.5f ) {
            transform.LookAt( GameController.instance.PlayerShip.transform );
        }
    }
}
