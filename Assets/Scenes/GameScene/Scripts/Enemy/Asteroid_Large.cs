using System;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid_Large : BaseAsteroid
{
    protected override int BaseScore { get { return GameConstants.SCORE_ASTEROID_LG; } }
    protected override Vector3 SurfaceHitOffset { get { return new Vector3( 0.0f, -1.75f, 0.0f ); } }

    private const float FORCE_DOWNWARD_LIMIT = 30000.0f;

    [Range( 0f, 100f )]
    public float PercentChanceOfLRock = 60.0f;
    [Range( 0f, 100f )]
    public float PercentChanceOfRRock = 60.0f;

    private const float MIN_SPLIT_ROCK_FORCE = 0.1f;
    private const float MAX_SPLIT_ROCK_FORCE = 0.5f;

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
        base.HitByLaser( laser );
        Fragment();
        Done();
    }

    /*****************************/
    public void Fragment()
    {
        // There's a chance to split off one or two small rocks

        // Left Fragment
        if ( Random.Range( 0, 100 ) < PercentChanceOfLRock ) {
            SpawnSmalLRock( true );

            if ( Random.Range( 0, 100 ) < 10 ) { // Tiny chance of further splintering
                SpawnSmalLRock( true );
            }
        }

        // Right Fragment
        if ( Random.Range( 0, 100 ) < PercentChanceOfRRock ) {
            SpawnSmalLRock( false );
            if ( Random.Range( 0, 100 ) < 10 ) { // Tiny chance of further splintering
                SpawnSmalLRock( false );
            }
        }
    }

    /*****************************/
    private void SpawnSmalLRock( bool drifts_left )
    {
        GameObject sm_rock = _ast_small_objectpool.GetInstance( transform.position, Quaternion.identity, false );

        if ( sm_rock != null ) {
            int dir = drifts_left ? -1 : 1;
            float force_downward = Random.Range( -FORCE_DOWNWARD_LIMIT, FORCE_DOWNWARD_LIMIT );

            Asteroid_Small as_small = sm_rock.GetComponent<Asteroid_Small>();
            as_small.IsFragment = true;
            as_small.Respawn();
            sm_rock.rigidbody.AddForce( new Vector3( dir * 40000, force_downward, 0.0f ) );
        }
    }
}
