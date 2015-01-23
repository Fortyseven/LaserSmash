using Game;
using UnityEngine;

//TODO: bring down pitch slightly for larger bomb
//TODO: geiger counter sound?

public class Bomb : GenericEnemy
{
    protected override float SpawnMaxX { get { return 11.0f; } }
    protected override float SpawnMinX { get { return -11.0f; } }

    protected override int BaseScore { get { return GameConstants.SCORE_KILLSAT; } }

    const float Y_OFFSET_SM = 15.0f;
    const float Y_OFFSET_LG = 16.0f;
    const float Y_OFFSET_FLOOR = 1.0f;
    //const float X_OFFSET_MAX = 11.0f;

    const float MIN_WEIGHT_LG = 15.0f;
    const float MAX_WEIGHT_LG = 40.0f;
    const float MIN_WEIGHT_SM = 20.0f;
    const float MAX_WEIGHT_SM = 40.0f;

    public GameObject NukePrefab;

    public bool IsLarge = false;

    AudioSource _audio;

    float _base_gravity_mult;
    bool _hit_surface;

    /******************************************************************/
    void Awake()
    {
        _base_gravity_mult = rigidbody2D.gravityScale;
        IsReady= false;
        _audio = GetComponent<AudioSource>();
    }

    /******************************************************************/
    void Update()
    {
        if ( !IsReady )
            return;

        // Did we go off screen? Sweep it under the rug.
        if ( Mathf.Abs( transform.position.x ) > GameConstants.SCREEN_X_BOUNDS ) {
            Done( false );
            return;
        }

        // Did we hit the ground? Punish player, make noises, explode
        if ( transform.position.y < Y_OFFSET_FLOOR ) {
            OnGroundHit();
            return;
        }

        _audio.pitch = transform.position.y / Y_OFFSET_LG;
    }

    /******************************************************************/
    private void Done( bool explode = true )
    {
        //if (_hit_surface)

        rigidbody2D.velocity = new Vector2( 0, 0 );

        if ( explode )
            Instantiate( ExplosionPrefab, transform.position, Quaternion.identity );

        _audio.Stop();
        Hibernate();
    }

    /******************************************************************/
    public void HitByLaser( Laserbeam laser )
    {
        if ( IsLarge ) {
            GameController.instance.State.AdjustScore( GameConstants.SCORE_BOMB_LG );
        }
        else {
            GameController.instance.State.AdjustScore( GameConstants.SCORE_BOMB_SM );
        }
        Destroy( laser.gameObject );
        Done();
    }

    /******************************************************************/
    void OnGroundHit()
    {
        Instantiate( NukePrefab, transform.position, Quaternion.identity );
        GameController.instance.PlayerComponent.Kill();
        _hit_surface = true;
        Done();
    }

    /******************************************************************/
    public override void Respawn()
    {
        //FIXME: bombsm/bomblg yoffs
        Vector3 spawn_pos = new Vector3( Random.Range( SpawnMinX, SpawnMaxX ),
                ( IsLarge ? Y_OFFSET_LG : Y_OFFSET_SM ), 0 );
        transform.position = spawn_pos;

        // Larger bombs move slower, less of threat
        if ( IsLarge ) {
            rigidbody2D.gravityScale = _base_gravity_mult * Random.Range( MIN_WEIGHT_LG, MAX_WEIGHT_LG );
        }
        else {
            rigidbody2D.gravityScale = _base_gravity_mult * Random.Range( MIN_WEIGHT_SM, MAX_WEIGHT_LG );
        }

        _audio.Play();
        _hit_surface = false;
        IsReady = true;
    }
}
