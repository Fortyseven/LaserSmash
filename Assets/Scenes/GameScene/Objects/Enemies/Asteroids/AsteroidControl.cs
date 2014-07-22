using UnityEngine;
using System.Collections;
using Game;

public class AsteroidControl : MonoBehaviour
{   
    public bool isLargeRock = false;
    
//    [Range(0.05f, 1.0f)]
//    public float DropSpeedMax = 0.5f;
//    [Range(0.05f, 1.0f)]
//    public float DropSpeedMin = 0.05f;

    [Range(0f,100f)]
    public float PercentChanceOfLRock = 50.0f;
    [Range(0f,100f)]
    public float PercentChanceOfRRock = 50.0f;
    
    private float MinSplitRockForce = 0.1f;    
    private float MaxSplitRockForce = 0.5f;

    public int SCORE_PENALTY_BASE = 200;
    public int SCORE_AWARD_BASE = 100;
    
    public AudioClip SoundHitSurface = null;
    private AudioSource mAudioSource = null;
    
    public GameObject ExplosionPrefab = null;
    public GameObject HitSurfacePrefab = null;
    public GameObject PlayerObject = null;
    public GameObject AsteroidSmallPrefab = null;

    //public GameObject ParticleTrail = null;

    private Player mPlayer = null;
    //private float mDropspeed = 0.5f;

    private float mGravityMultiplier = 0.0f;

    private GameControl mGameControl = null;

    //private GameObject mParticleTrail = null;
    
    void Start()
    {
        mGameControl = FindObjectOfType<GameControl>();
        mPlayer = FindObjectOfType<Player>();
        mAudioSource = GetComponent<AudioSource>();

        mGravityMultiplier = Random.Range(0, 4.0f);
        rigidbody2D.gravityScale = rigidbody2D.gravityScale * mGravityMultiplier;

        //Debug.Log("Gravity = " + rigidbody2D.gravityScale);

//        mParticleTrail = Instantiate(ParticleTrail, transform.position, Quaternion.identity) as GameObject;

        if (!isLargeRock) {
            ParticleEmitter p = GetComponentInChildren<ParticleEmitter>();
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
    }       
        
    // Update is called once per frame
    void FixedUpdate()
    {
//        mParticleTrail.transform.position = transform.position;

        // Did we go off screen? Sweep it under the rug.
        if (Mathf.Abs(transform.position.x) > GameConstants.SCREEN_X_BOUNDS ) {
            Explode();
            return;
        }
        
        // Did we hit the ground? Punish player, make noises, explode
        if ( transform.position.y < GameConstants.SCREEN_Y_FLOOR ) {
            mGameControl.getGameState().Score -= SCORE_PENALTY_BASE;
            mPlayer.Hurt();
            mAudioSource.PlayOneShot(SoundHitSurface);
            Explode();
            return;
        }
    }

    private void Done()
    {
        Instantiate( ExplosionPrefab, transform.position, Quaternion.identity );
//        Destroy( mParticleTrail.gameObject );
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

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

    public void HitByLaser( Laserbeam laser )
    {
        mGameControl.getGameState().Score += GameConstants.SCORE_ASTEROID_BYLASER;
        Destroy( laser.gameObject );
        Explode();
    }
}
