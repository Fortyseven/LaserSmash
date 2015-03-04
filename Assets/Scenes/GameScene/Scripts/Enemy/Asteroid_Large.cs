using Game;
using UnityEngine;

public class Asteroid_Large : BaseAsteroid
{
    protected override int BaseScore { get { return GameConstants.SCORE_ASTEROID_LG; } }
    protected override Vector3 SurfaceHitOffset { get { return new Vector3( 0.0f, -1.75f, 0.0f ); } }

    private const float FRAGMENT_Y_FORCE_MAX = 400.0f;
    private const float FRAGMENT_Y_FORCE_MIN = 100.0f;


    [Range( 0f, 100f )]
    public float PercentChanceOfLRock = 60.0f;
    [Range( 0f, 100f )]
    public float PercentChanceOfRRock = 60.0f;

    private const int PERCENT_CHANCE_OF_BONUS_LROCK = 10;
    private const int PERCENT_CHANCE_OF_BONUS_RROCK = 10;

    // We'll spawn our children from this pool
    ObjectPool _ast_small_objectpool;

    /*****************************/
    public override void Awake()
    {
        base.Awake();
        _ast_small_objectpool = GameController.instance.WaveCon.GetPoolForName( "Asteroid Small" );
    }

    /*****************************/
    public override void Respawn()
    {
        base.Respawn();

        //rigidbody.AddForce( 20000.0f, 0.0f, 0.0f );

        _hit_surface = false;
        IsReady = true;
    }

    /*****************************/
    public override void HitByLaser( Laserbeam laser )
    {
        //base.HitByLaser( laser );
        Fragment();
        Done();
    }

    /*****************************/
    private enum Direction
    {
        DIRECTION_LEFT,
        DIRECTION_RIGHT
    }

    /*****************************/
    public void Fragment()
    {
        // There's a chance to split off one or two small rocks

        // Left Fragment
        if ( Random.Range( 0, 100 ) < PercentChanceOfLRock ) {
            SpawnSmalLRock( Direction.DIRECTION_LEFT );
            if ( Random.Range( 0, 100 ) < PERCENT_CHANCE_OF_BONUS_LROCK ) { // Tiny chance of further splintering
                SpawnSmalLRock( Direction.DIRECTION_LEFT );
            }
        }

        // Right Fragment
        if ( Random.Range( 0, 100 ) < PercentChanceOfRRock ) {
            SpawnSmalLRock( Direction.DIRECTION_RIGHT );
            if ( Random.Range( 0, 100 ) < PERCENT_CHANCE_OF_BONUS_RROCK ) { // Tiny chance of further splintering
                SpawnSmalLRock( Direction.DIRECTION_RIGHT );
            }
        }
    }

    /*****************************/
    private void SpawnSmalLRock( Direction drift_direction )
    {
        GameObject sm_rock = _ast_small_objectpool.GetInstance( transform.position, Quaternion.identity, false );

        if ( sm_rock != null ) {
            int dir = ( drift_direction == Direction.DIRECTION_LEFT ) ? -1 : 1;
            float force_downward = -Random.Range( FRAGMENT_Y_FORCE_MIN, FRAGMENT_Y_FORCE_MAX );

            Asteroid_Small as_small = sm_rock.GetComponent<Asteroid_Small>();
            as_small.IsFragment = true;
            as_small.Respawn();
            float force_sideways = dir * Random.Range( 0.0f, 1.0f ) * 300;

            sm_rock.GetComponent<Rigidbody>().AddForce( new Vector3( force_sideways, force_downward, 0.0f ) );
        }
    }
}
