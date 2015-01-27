using Game;
using UnityEngine;

public class Asteroid_Large : BaseAsteroid
{
    protected override int BaseScore { get { return GameConstants.SCORE_ASTEROID_LG; } }

    protected override Vector3 SurfaceHitOffset { get { return new Vector3( 0.0f, -1.75f, 0.0f ); } }

    [Range( 0f, 100f )]
    public float PercentChanceOfLRock = 50.0f;
    [Range( 0f, 100f )]
    public float PercentChanceOfRRock = 50.0f;

    private const float MIN_SPLIT_ROCK_FORCE = 0.1f;
    private const float MAX_SPLIT_ROCK_FORCE = 0.5f;

    // We'll spawn our children from this pool
    ObjectPool _ast_small_objectpool;

    /*****************************/
    protected override void Awake()
    {
        //_audio = GetComponent<AudioSource>();
        base.Awake();
        _ast_small_objectpool = GameController.instance.WaveCon.GetPoolForName( "Asteroid Small" );
    }

    /*****************************/
    public override void Respawn()
    {
        base.Respawn();
        //Vector3 start_pos = new Vector3( Random.Range( SpawnMinX, SpawnMaxX ), Y_SPAWN_OFFSET, 0 );
        //        transform.position = start_pos;

        //_gravity_multiplier = Random.Range( 1.0f, 15.0f );
        //rigidbody2D.gravityScale = _base_gravityscale * _gravity_multiplier;

        _particle_trail = Instantiate( ParticleEmitterPrefab, transform.position, Quaternion.identity ) as GameObject;

        int dir = Random.Range( 0, 3 );
        switch ( dir ) {
            case 0:
                rigidbody.AddForce( new Vector3( Random.Range( MIN_SPLIT_ROCK_FORCE, MAX_SPLIT_ROCK_FORCE ) * 250.0f, 0.0f, 0.0f ) );
                break;
            case 1:
                break;
            case 2:
                rigidbody.AddForce( -new Vector3( Random.Range( MIN_SPLIT_ROCK_FORCE, MAX_SPLIT_ROCK_FORCE ) * 250.0f, 0.0f, 0.0f ) );
                break;
        }

        _hit_surface = false;
        IsReady = true;
    }


    /*****************************/
    //protected void Update()
    //{
    //    if ( !IsReady )
    //        return;

    //    // Did we go off screen? Sweep it under the rug.
    //    if ( Mathf.Abs( transform.position.x ) > GameConstants.SCREEN_X_BOUNDS ) {
    //        Done( false );
    //        return;
    //    }

    //    // Did we hit the ground? Punish player, make noises, explode
    //    if ( transform.position.y < GameConstants.SCREEN_Y_FLOOR ) {
    //        GameController.instance.State.AdjustScore( -( BaseScore / 2 ) );
    //        _hit_surface = true;
    //        Done( false );
    //        return;
    //    }

    //    if ( _particle_trail != null )
    //        _particle_trail.transform.position = transform.position;
    //}

    /*****************************/
    public override void HitByLaser( Laserbeam laser )
    {
        base.HitByLaser( laser );
        Fragment();
    }

    /*****************************/
    public void Fragment()
    {
        // There's a chance to split off one or two small rocks
        GameObject sm_rock;

        if ( Random.Range( 0, 100 ) >= PercentChanceOfLRock ) {
            float sm_rock_driftforce_x = Random.Range( MIN_SPLIT_ROCK_FORCE, MAX_SPLIT_ROCK_FORCE ) * 150.0f;
            float sm_rock_driftforce_y = Random.Range( MIN_SPLIT_ROCK_FORCE, MAX_SPLIT_ROCK_FORCE ) * 50.0f;

            sm_rock = _ast_small_objectpool.GetInstance( transform.position, Quaternion.identity, false );

            if ( sm_rock != null ) {
                sm_rock.rigidbody.AddForce( new Vector3( sm_rock_driftforce_x, 0.0f, 0.0f ) );
                sm_rock.rigidbody.AddForce( -new Vector3( 0.0f, sm_rock_driftforce_y, 0.0f ) );
                sm_rock.SendMessage( "RespawnFragment" );
            }
        }

        if ( Random.Range( 0, 100 ) >= PercentChanceOfRRock ) {
            float sm_rock_driftforce_x = Random.Range( MIN_SPLIT_ROCK_FORCE, MAX_SPLIT_ROCK_FORCE ) * 150.0f;
            float sm_rock_driftforce_y = Random.Range( MIN_SPLIT_ROCK_FORCE, MAX_SPLIT_ROCK_FORCE ) * 50.0f;

            //sm_rock = Instantiate( AsteroidSmallPrefab, transform.position, Quaternion.identity ) as GameObject;   
            sm_rock = _ast_small_objectpool.GetInstance( transform.position, Quaternion.identity, false );

            if ( sm_rock != null ) {
                sm_rock.rigidbody.AddForce( -new Vector3( sm_rock_driftforce_x, 0.0f, 0.0f ) );
                sm_rock.rigidbody.AddForce( new Vector3( 0.0f, sm_rock_driftforce_y, 0.0f ) );
                sm_rock.SendMessage( "RespawnFragment" );
            }
        }

        Done();
    }
}
