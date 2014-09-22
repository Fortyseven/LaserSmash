using UnityEngine;
using System.Collections;
using Game;

public class Asteroid_Large : EnemyType
{   
    private const float Y_SPAWN_OFFSET = 15.5f;
    private const float MAX_X_OFFSET = -12f;
    private const float MIN_X_OFFSET = 12f;

    private Vector3 SURFACE_HIT_OFFSET = new Vector3(0.0f,-1.75f,0.0f);

    [Range(0f,100f)]
    public float PercentChanceOfLRock = 50.0f;
    [Range(0f,100f)]
    public float PercentChanceOfRRock = 50.0f;
    
    private float MinSplitRockForce = 0.1f;    
    private float MaxSplitRockForce = 0.5f;

    public AudioClip SoundHitSurface = null;
    private AudioSource _audio = null;
    
    public GameObject ExplosionPrefab = null;
    public GameObject HitSurfacePrefab = null;
    public GameObject PlayerObject = null;
    public GameObject AsteroidSmallPrefab = null;

    public GameObject ParticleEmitterPrefab = null;

    private float _gravity_multiplier = 0.0f;

    private GameObject _particle_trail = null;

    bool _hit_surface;

    private float _base_gravityscale;
    ObjectPool _ast_small_objectpool;
    
    /*****************************/
    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _base_gravityscale = rigidbody2D.gravityScale;
        _ast_small_objectpool = GameController.instance.WaveCon.GetPoolForName("Asteroid Small");
        _is_ready = false;
    }

    /*****************************/
    public override void Respawn()
    {
        Vector3 start_pos = new Vector3(Random.Range(MIN_X_OFFSET, MAX_X_OFFSET), Y_SPAWN_OFFSET, 0);
        transform.position = start_pos;
        
        _gravity_multiplier = Random.Range(1.0f, 15.0f);
        rigidbody2D.gravityScale = _base_gravityscale * _gravity_multiplier;
        
        _particle_trail = Instantiate(ParticleEmitterPrefab, transform.position, Quaternion.identity) as GameObject;
        
        int dir = Random.Range(0,3);
        switch(dir) {
            case 0: rigidbody2D.AddForce(new Vector2(Random.Range(MinSplitRockForce, MaxSplitRockForce) * 250.0f, 0.0f)); break;
            case 1: break;
            case 2: rigidbody2D.AddForce(-new Vector2(Random.Range(MinSplitRockForce, MaxSplitRockForce) * 250.0f, 0.0f)); break;
        }
        
        _hit_surface = false;
        _is_ready = true;
    }

        
    /*****************************/
    void Update()
    {
        if (!_is_ready) return;

        // Did we go off screen? Sweep it under the rug.
        if (Mathf.Abs(transform.position.x) > GameConstants.SCREEN_X_BOUNDS ) {
            Done(false);
            return;
        }
        
        // Did we hit the ground? Punish player, make noises, explode
        if ( transform.position.y < GameConstants.SCREEN_Y_FLOOR ) {
            GameController.instance.State.AdjustScore(-(GameConstants.SCORE_ASTEROID_LG/2));
            _hit_surface = true;
            Done(false);
            return;
        }

        if (_particle_trail != null)
            _particle_trail.transform.position = this.transform.position;
    }

    /*****************************/
    public void HitByLaser( Laserbeam laser )
    {
        GameController.instance.State.AdjustScore(GameConstants.SCORE_ASTEROID_LG);
        Destroy( laser.gameObject );
        Fragment();
    }

    /*****************************/
    public void Fragment()
    {
        // There's a chance to split off one or two small rocks
        GameObject sm_rock;
        
        if (Random.Range(0,100) >= PercentChanceOfLRock) {
            float sm_rock_driftforce_x = Random.Range(MinSplitRockForce, MaxSplitRockForce) * 150.0f;
            float sm_rock_driftforce_y = Random.Range(MinSplitRockForce, MaxSplitRockForce) * 50.0f;
            
            sm_rock = _ast_small_objectpool.GetInstance( transform.position, Quaternion.identity, false ) as GameObject;   

            if(sm_rock != null) {
                sm_rock.rigidbody2D.AddForce(new Vector2(sm_rock_driftforce_x, 0.0f));
                sm_rock.rigidbody2D.AddForce(-new Vector2(0.0f, sm_rock_driftforce_y));
                sm_rock.SendMessage("RespawnFragment");
            }
        }
        
        if (Random.Range(0,100) >= PercentChanceOfRRock) {
            float sm_rock_driftforce_x = Random.Range(MinSplitRockForce, MaxSplitRockForce) * 150.0f;
            float sm_rock_driftforce_y = Random.Range(MinSplitRockForce, MaxSplitRockForce) * 50.0f;
            
            //sm_rock = Instantiate( AsteroidSmallPrefab, transform.position, Quaternion.identity ) as GameObject;   
            sm_rock = _ast_small_objectpool.GetInstance( transform.position, Quaternion.identity, false ) as GameObject;   

            if(sm_rock != null) {
                sm_rock.rigidbody2D.AddForce(-new Vector2(sm_rock_driftforce_x, 0.0f));
                sm_rock.rigidbody2D.AddForce(new Vector2(0.0f, sm_rock_driftforce_y));
                sm_rock.SendMessage("RespawnFragment");
            }
        }

        Done();
    }

    /*****************************/
    private void Done(bool explode = true)
    {
        rigidbody2D.velocity =  new Vector2(0,0);

        if (_particle_trail != null) {
            Destroy(_particle_trail);
        }

        if (explode) Instantiate( ExplosionPrefab, transform.position, Quaternion.identity );

        if (_hit_surface) Instantiate( HitSurfacePrefab, transform.position + SURFACE_HIT_OFFSET, Quaternion.identity );

        Hibernate();
    }

    /*****************************/
    public new void InstaKill ()
    {
        Done(false);
    }
}
