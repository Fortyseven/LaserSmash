using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private const float SHIP_SPEED = 40.0f;
    private const float SHIP_X_BOUNDS = 12.0f;
    private const float TOUCH_MOVE_SPEED = 0.05f;    
    private const float LASER_Y_OFFSET_FROM_SHIP = 2.0f;
    
    public GameObject LaserbeamPrefab = null;
    //public GameObject PainPrefab = null;
    public GameObject deathExplosionPrefab = null;
    
    
    float mLastFire = -1.0f;

    GameObject mLastFireGO = null;
    
    public dfPanel panelLeft = null;
    public dfPanel panelRight = null;
    private float mTouchXAxis = 0.0f;   
    private bool mGuiKeyRight = false;
    private bool mGuiKeyLeft = false;
    private bool mAutoFireEnabled = false;

    private GameObject mSceneStars = null;
    private GameObject mSceneSurface = null;
    private Vector3 mSceneStarsPosition;
    private Vector3 mSceneSurfacePosition;
    
    void Start()
    {
        if (GameObject.Find("Game").GetComponent<GameControl>().isMobileMode) {
            mAutoFireEnabled = true;
        }
        
        mSceneStars = GameObject.Find("BackgroundStars");
        mSceneStarsPosition = mSceneStars.transform.position;
        mSceneSurface = GameObject.Find("Surface");
        mSceneSurfacePosition = mSceneSurface.transform.position;
    }

    public void OnMouseUp( dfControl control, dfMouseEventArgs mouseEvent )
    {
        if ( control.name == "moveLeft" ) {
            mGuiKeyLeft = false;
            if ( mGuiKeyRight )
                mTouchXAxis -= mTouchXAxis;
            else
                mTouchXAxis = 0.0f;
            //mTouchXAxis += TOUCH_MOVE_SPEED;
                        
        }
        if ( control.name == "moveRight" ) {
            mGuiKeyRight = false;
            if ( mGuiKeyLeft )
                mTouchXAxis -= mTouchXAxis;
            else
                mTouchXAxis = 0.0f;
            //mTouchXAxis -= TOUCH_MOVE_SPEED;
        }
    }

    public void OnMouseDown( dfControl control, dfMouseEventArgs mouseEvent )
    {
        if ( control.name == "moveLeft" ) {
            mGuiKeyLeft = true;
            if ( mGuiKeyRight ) {
                mTouchXAxis = Mathf.Abs( mTouchXAxis );
                mGuiKeyRight = false;
            }
            
        }
        if ( control.name == "moveRight" ) {
            mGuiKeyRight = true;
            if ( mGuiKeyLeft ) {
                mTouchXAxis = -Mathf.Abs( mTouchXAxis );
                mGuiKeyLeft = false;
            }
        }
        
    }

    private void updateGUIKeys()
    {
        if ( mGuiKeyRight )
            mTouchXAxis += TOUCH_MOVE_SPEED;
        if ( mGuiKeyLeft )
            mTouchXAxis -= TOUCH_MOVE_SPEED;
        
        mTouchXAxis = Mathf.Clamp( mTouchXAxis, -1.0f, 1.0f );
    }
        
    void Update()
    {
        updateGUIKeys();
                
        Vector3 pos = transform.position;
                                
        pos.x += Input.GetAxis( "Horizontal" ) * SHIP_SPEED * Time.deltaTime;
        pos.x += mTouchXAxis * SHIP_SPEED * Time.deltaTime;
        pos.x = Mathf.Clamp( pos.x, -SHIP_X_BOUNDS, SHIP_X_BOUNDS );
            
        if (mAutoFireEnabled) Fire();
        else if(Input.GetButton("Fire1")) Fire();

        mSceneStarsPosition.x = transform.position.x * 0.03f;
        mSceneStars.transform.position = mSceneStarsPosition;
        
        mSceneSurfacePosition.x = transform.position.x * 0.02f;
        mSceneSurface.transform.position = mSceneSurfacePosition;
        
        transform.position = pos;
    }
        
    void Fire()
    {               
        if ( mLastFireGO == null )              
            mLastFireGO = SpawnLaserbeam();             
    }
    
    void Kill()
    {
        Instantiate( deathExplosionPrefab, transform.position, Quaternion.identity );   
        enabled = false;
    }
        
    public void Hurt()
    {
        //Instantiate( PainPrefab, transform.position, Quaternion.identity );
    }
        
    GameObject SpawnLaserbeam()
    {
        Vector3 newpos = transform.position;
        newpos.y += LASER_Y_OFFSET_FROM_SHIP; 
        return Instantiate( LaserbeamPrefab, newpos, Quaternion.identity ) as GameObject;
    }

}

