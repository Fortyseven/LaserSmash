using Game;
using UnityEngine;

public class Seeker : GenericEnemy
{
    protected override float SpawnMaxX { get { return 12.0f; } }
    protected override float SpawnMinX { get { return -12.0f; } }

    protected override int BaseScore { get { return GameConstants.SCORE_KILLSAT; } }

    private const float SURFACE_Y = 0.2f;

    private const float SPEED = 5.0f;

    //private const float LOCK_POINT_Y = 2.0f;

    private float _base_rot;
    private bool _lock_to_surface;

    /************************/
    public void Awake()
    {
        Respawn();
    }

    /************************/
    public void Update()
    {
        Transform player = GameEnvironment.PlayerShip.transform;
        Vector3 pos = transform.position;

        if ( pos.y > SURFACE_Y && !_lock_to_surface ) {
            pos = Vector3.MoveTowards( pos, player.position, Time.deltaTime * 0.25f );
            pos += transform.forward * Time.deltaTime * 4f;
            transform.position = pos;

            Vector3 foo = player.position - transform.position;
            Quaternion bar = Quaternion.LookRotation( foo );
            transform.rotation = Quaternion.Lerp( transform.rotation, bar, Time.deltaTime * 5 );
        }

        //TODO: This should lerp into position, not snap, but whatever...
        if ( pos.y <= SURFACE_Y && !_lock_to_surface ) {
            transform.LookAt( player.position + new Vector3( 0, 0.8f, 0 ) );
            _lock_to_surface = true;

            Vector3 rot = transform.rotation.eulerAngles;
            rot.x = 0f;
            rot.y = ( transform.rotation.eulerAngles.y > 135 ) ? 270.0f : 90.0f;

            transform.rotation = Quaternion.Euler( rot );
        }

        if ( _lock_to_surface ) {
            pos += transform.forward * Time.deltaTime * 4f;
            transform.position = pos;
        }

        if ( IsOffScreen() ) {
            InstaKill();
        }
        //_c++;
    }

    /************************/
    private bool IsOffScreen()
    {
        return ( ( transform.position.x < ( SpawnMinX - 10.0f ) ) ||
                 ( transform.position.x > ( SpawnMaxX + 10.0f ) ) );
    }

    /************************/
    public override void Respawn()
    {
        base.Respawn();
        transform.LookAt( Vector3.down );

        _lock_to_surface = false;
        _base_rot = transform.rotation.eulerAngles.y;
    }
}
