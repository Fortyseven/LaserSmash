using UnityEngine;
using System.Collections;
using Game;

public class AsteroidLGControl : EnemyType
{   
    public bool isLargeRock = false;

    private const float Y_SPAWN_OFFSET = 15.5f;
    private const float MAX_X_OFFSET = -13f;
    private const float MIN_X_OFFSET = 13f;

    [Range(0f,100f)]
    public float PercentChanceOfLRock = 50.0f;
    [Range(0f,100f)]
    public float PercentChanceOfRRock = 50.0f;
    
    private float MinSplitRockForce = 0.1f;    
    private float MaxSplitRockForce = 0.5f;

    public AudioClip SoundHitSurface = null;
    private AudioSource mAudioSource = null;
    
    public GameObject ExplosionPrefab = null;
    public GameObject HitSurfacePrefab = null;
    public GameObject PlayerObject = null;
    public GameObject AsteroidSmallPrefab = null;

    public GameObject ParticleEmitterPrefab = null;

    private Player _player = null;

    private float _gravity_multiplier = 0.0f;

    private GameObject _particle_trail = null;

    bool _hit_surface;
    
    /*****************************/
    void Start()
    {
        _player = FindObjectOfType<Player>();
        mAudioSource = GetComponent<AudioSource>();
        gameObject.SetActive(false);
        _is_ready = false;
    }       
        
    /*****************************/
    void Update()
    {
        if (!_is_ready) {
            Debug.Log("Was not ready");
            return;
        }

        // Did we go off screen? Sweep it under the rug.
        if (Mathf.Abs(transform.position.x) > GameConstants.SCREEN_X_BOUNDS ) {
            Done();
            return;
        }
        
        // Did we hit the ground? Punish player, make noises, explode
        if ( transform.position.y < GameConstants.SCREEN_Y_FLOOR ) {
            //GameController.instance.AdjustScore(-SCORE_PENALTY_BASE);
            _hit_surface = true;
            Done();
            return;
        }

        if (_particle_trail != null)
            _particle_trail.transform.position = this.transform.position;
    }

    /*****************************/
    private void Done()
    {
        rigidbody2D.velocity =  new Vector2(0,0);

        Instantiate( ExplosionPrefab, transform.position, Quaternion.identity );

        if (_particle_trail != null) {
            Destroy(_particle_trail, 1.0f);
        }
        Hibernate();
    }

    /*****************************/
    public void HitByLaser( Laserbeam laser )
    {
        GameController.instance.State.AdjustScore(GameConstants.SCORE_ASTEROID_BYLASER);
        Destroy( laser.gameObject );
        Explode();
    }

    /*****************************/
    public void Explode()
    {
        // There's a chance to split off one or two small rocks if we're large
        if (isLargeRock) {
            GameObject sm_rock;
            
            if (Random.Range(0,100) >= PercentChanceOfLRock) {
                float sm_rock_driftforce_x = Random.Range(MinSplitRockForce, MaxSplitRockForce) * 150.0f;
                float sm_rock_driftforce_y = Random.Range(MinSplitRockForce, MaxSplitRockForce) * 50.0f;
                
                sm_rock = Instantiate( AsteroidSmallPrefab, transform.position, Quaternion.identity ) as GameObject;   
                sm_rock.rigidbody2D.AddForce(new Vector2(sm_rock_driftforce_x, 0.0f));
                sm_rock.rigidbody2D.AddForce(-new Vector2(0.0f, sm_rock_driftforce_y));
            }
            
            if (Random.Range(0,100) >= PercentChanceOfRRock) {
                float sm_rock_driftforce_x = Random.Range(MinSplitRockForce, MaxSplitRockForce) * 150.0f;
                float sm_rock_driftforce_y = Random.Range(MinSplitRockForce, MaxSplitRockForce) * 50.0f;
                
                sm_rock = Instantiate( AsteroidSmallPrefab, transform.position, Quaternion.identity ) as GameObject;   
                sm_rock.rigidbody2D.AddForce(-new Vector2(sm_rock_driftforce_x, 0.0f));
                sm_rock.rigidbody2D.AddForce(new Vector2(0.0f, sm_rock_driftforce_y));
            }
        }
        Done();
    }

    /*****************************/
    public override void Respawn()
    {
        Vector3 start_pos = new Vector3(Random.Range(MIN_X_OFFSET, MAX_X_OFFSET), Y_SPAWN_OFFSET, 0);
        transform.position = start_pos;
        //transform.position.Set(9090,9090,9090);

        _gravity_multiplier = Random.Range(0, 4.0f);
        //rigidbody2D.gravityScale = rigidbody2D.gravityScale * _gravity_multiplier;

        _particle_trail = Instantiate(ParticleEmitterPrefab, transform.position, Quaternion.identity) as GameObject;
        
        if (!isLargeRock) {
            ParticleEmitter p = _particle_trail.GetComponentInChildren<ParticleEmitter>();
            p.minSize = 0.5f;
            p.maxSize = 1.0f;
        } else {
            int dir = Random.Range(0,3);
            switch(dir) {
                case 0: rigidbody2D.AddForce(new Vector2(Random.Range(MinSplitRockForce, MaxSplitRockForce) * 250.0f, 0.0f)); break;
                case 1: break;
                case 2: rigidbody2D.AddForce(-new Vector2(Random.Range(MinSplitRockForce, MaxSplitRockForce) * 250.0f, 0.0f)); break;
            }
        }
        
        _hit_surface = false;
        _is_ready = true;
    }
}
