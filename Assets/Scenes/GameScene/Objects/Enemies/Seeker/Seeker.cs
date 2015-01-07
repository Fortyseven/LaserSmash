using Game;
using UnityEngine;

public class Seeker : EnemyType
{
    public GameObject ExplosionPrefab;
    private const float MAX_X_OFFSET = -12.0f;
    private const float MIN_X_OFFSET = 12.0f;
    private const float Y_OFFSET = 16.0f;
    private const float SURFACE_Y = 0.2f;
    private const float SPEED = 5.0f;

    void Awake()
    {
        Respawn();
    }

    /************************/
    float _c = 0;
    float _base_rot;
    bool _lock_mode;

    /************************/
    void Update()
    {
        Transform player = GameController.instance.PlayerShip.transform;
        Vector3 pos = transform.position;

        if ( pos.y > SURFACE_Y && !_lock_mode ) {
            pos = Vector3.MoveTowards( pos, player.position, Time.deltaTime * 0.25f );
            pos += transform.forward * Time.deltaTime * 4f;
            transform.position = pos;

            Vector3 foo = player.position - transform.position;
            Quaternion bar = Quaternion.LookRotation( foo );
            transform.rotation = Quaternion.Lerp( transform.rotation, bar, Time.deltaTime * 5 );
        }

        //TODO: This should lerp into position, not snap, but whatever...
        if ( pos.y <= SURFACE_Y && !_lock_mode ) {
            transform.LookAt( player.position + new Vector3( 0, 0.8f, 0 ) );
            _lock_mode = true;

            Vector3 rot = transform.rotation.eulerAngles;
            rot.x = 0f;

            if ( transform.rotation.eulerAngles.y > 135 ) {
                rot.y = 270.0f;

            }
            else {
                rot.y = 90.0f;
            }
            transform.rotation = Quaternion.Euler( rot );
        }

        if ( _lock_mode ) {
            pos += transform.forward * Time.deltaTime * 4f;
            transform.position = pos;


        }

        _c++;
    }

    /************************/
    public void OnTriggerEnter2D( Collider2D col )
    {
        Done();
    }

    /************************/
    public void HitByLaser( Laserbeam laser )
    {
        GameController.instance.State.AdjustScore( GameConstants.SCORE_KILLSAT );
        Destroy( laser.gameObject );
        Explode();
    }

    /************************/
    void Explode()
    {
        Instantiate( ExplosionPrefab, transform.position, Quaternion.identity );
        Done();
    }

    /************************/
    void Done()
    {
        Hibernate();
    }

    /************************/
    public override void Respawn()
    {
        Debug.Log( "Resp" );
        transform.LookAt( Vector3.down );
        transform.position.Set( -10.0f, Y_OFFSET, 0 );
        _lock_mode = false;
        _base_rot = transform.rotation.eulerAngles.y;
    }
}
