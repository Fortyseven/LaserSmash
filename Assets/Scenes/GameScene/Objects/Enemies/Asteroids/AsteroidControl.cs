using UnityEngine;
using System.Collections;

public class AsteroidControl : MonoBehaviour
{   
    public bool isLargeRock = false;
    
    [Range(0.05f, 1.0f)]
    public float DropSpeedMax = 0.5f;
    [Range(0.05f, 1.0f)]
    public float DropSpeedMin = 0.05f;

    [Range(0f,100f)]
    public float PercentChanceOfLRock = 50.0f;
    [Range(0f,100f)]
    public float PercentChanceOfRRock = 50.0f;
    
    public float MinSplitRockForce = 0.001f;    
    public float MaxSplitRockForce = 0.005f;

    public int SCORE_PENALTY_BASE = 200;
    public int SCORE_AWARD_BASE = 100;
    
    public AudioClip SoundHitSurface = null;
    private AudioSource mAudioSource = null;
    
    public GameObject ExplosionPrefab = null;
    public GameObject HitSurfacePrefab = null;
    public GameObject PlayerObject = null;
    public GameObject AsteroidSmallPrefab = null;
    
    private Player mPlayer = null;
    private float mDropspeed = 0.5f;
    
    private GameControl mGameControl = null;

    
    void Start()
    {
        mDropspeed = Random.Range( 0.05f, 0.5f );
        mGameControl = FindObjectOfType<GameControl>();
        mPlayer = FindObjectOfType<Player>();
        mAudioSource = GetComponent<AudioSource>();
                
        //Debug.Log (mGameControl);
    }       
        
    // Update is called once per frame
    void Update()
    {
        transform.Translate( 0, -mDropspeed * Time.deltaTime, 0 );
        
        // Did we go off screen? Sweep it under the rug.
        if (Mathf.Abs(transform.position.x) > 2.0f ) {
            Destroy( this.gameObject );
            return;
        }
        
        // Did we hit the ground? Punish player, make noises, explode
        if ( transform.position.y < -1.0f ) {
            mGameControl.getGameState().Score -= SCORE_PENALTY_BASE;
            mPlayer.Hurt();
            
            mAudioSource.PlayOneShot(SoundHitSurface);
            Instantiate( HitSurfacePrefab, this.gameObject.transform.position, Quaternion.identity );
            
            Destroy( this.gameObject );
        }
    }
        
    public void HitByLaser( Laserbeam laser )
    {
        mGameControl.getGameState().Score += 100;
        Instantiate( ExplosionPrefab, laser.gameObject.transform.position, Quaternion.identity );
        Destroy( this.gameObject, 0.05f );
        Destroy( laser.gameObject );
        
        // There's a chance to split off one or two small rocks if we're large
        if (isLargeRock) {
            GameObject sm_rock;
            float sm_rock_driftforce = Random.Range(MinSplitRockForce, MaxSplitRockForce);
            
            if (Random.Range(0,100) >= PercentChanceOfLRock) {
                sm_rock_driftforce = Random.Range(MinSplitRockForce, MaxSplitRockForce);
                sm_rock = Instantiate( AsteroidSmallPrefab, laser.gameObject.transform.position, Quaternion.identity ) as GameObject;   
                sm_rock.rigidbody2D.AddForce(new Vector2(sm_rock_driftforce,0.0f));
            }
            
            if (Random.Range(0,100) >= PercentChanceOfRRock) {
                sm_rock_driftforce = Random.Range(MinSplitRockForce, MaxSplitRockForce);
                sm_rock = Instantiate( AsteroidSmallPrefab, laser.gameObject.transform.position, Quaternion.identity ) as GameObject;   
                sm_rock.rigidbody2D.AddForce(new Vector2(-sm_rock_driftforce,0.0f));
            }
        }       
    }
}
