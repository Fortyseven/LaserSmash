//#define TESTMODE

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Game;

public class Player : MonoBehaviour
{
    private const float SHIP_SPEED = 13.0f;
    private const float SHIP_X_BOUNDS = 13.0f;
    private const float TOUCH_MOVE_SPEED = 0.05f;    
    private const float LASER_Y_OFFSET_FROM_SHIP = 2.0f;
    private const float FIRE_DELAY = 0.5f;
    
    public GameObject LaserbeamPrefab = null;
    public GameObject DeathExplosionPrefab = null;

    public Image DeathPanel;

    GameObject mLastFireGO = null;

    Vector3 _starting_position;

    public GameObject My_Mesh;
    
//    public dfPanel panelLeft = null;
//    public dfPanel panelRight = null;
    float _touch_axis_x = 0.0f;   
//    bool _gui_key_right = false;
//    bool _gui_key_left = false;
    bool _autofire_enabled = false;

    GameObject mSceneSurface = null;
    Vector3 mSceneSurfacePosition;

    bool _is_alive = false;

    void Awake()
    {
        _starting_position = transform.position;

    }

    /**************************************/
    void Start()
    {
#if UNITY_ANDROID
            _autofire_enabled = true;
#endif

        mSceneSurface = GameObject.Find("Surface");
        if (mSceneSurface == null) {
            throw new UnityException("Could not find stage surface");
        }

        mSceneSurfacePosition = mSceneSurface.transform.position;

        if(_autofire_enabled) {
            StartCoroutine("AutoFireCoroutine");
        }


        Reset();
        enabled = true;
    }

//    public void OnDirectionalInputClicked(int dir)
//    {
//        if (dir == -1) {
//            _gui_key_right = true;
//        }
//    }

//    IEnumerator AutoFireCoroutine()
//    {
//        float t = Time.time;
//        while(true) {
//            if (GameController.instance.State.IsRunning) {
//                Fire();
//            } else {
//                break;
//            }
//            yield return new WaitForSeconds(FIRE_DELAY);
//        }
//
//    }


#region Input
//    public void OnMouseUp( dfControl control, dfMouseEventArgs mouseEvent )
//    {
//        if ( control.name == "moveLeft" ) {
//            mGuiKeyLeft = false;
//            if ( mGuiKeyRight )
//                mTouchXAxis -= mTouchXAxis;
//            else
//                mTouchXAxis = 0.0f;
//            //mTouchXAxis += TOUCH_MOVE_SPEED;
//                        
//        }
//        if ( control.name == "moveRight" ) {
//            mGuiKeyRight = false;
//            if ( mGuiKeyLeft )
//                mTouchXAxis -= mTouchXAxis;
//            else
//                mTouchXAxis = 0.0f;
//            //mTouchXAxis -= TOUCH_MOVE_SPEED;
//        }
//    }

//    public void OnMouseDown( dfControl control, dfMouseEventArgs mouseEvent )
//    {
//        if ( control.name == "moveLeft" ) {
//            mGuiKeyLeft = true;
//            if ( mGuiKeyRight ) {
//                mTouchXAxis = Mathf.Abs( mTouchXAxis );
//                mGuiKeyRight = false;
//            }
//            
//        }
//        if ( control.name == "moveRight" ) {
//            mGuiKeyRight = true;
//            if ( mGuiKeyLeft ) {
//                mTouchXAxis = -Mathf.Abs( mTouchXAxis );
//                mGuiKeyLeft = false;
//            }
//        }
//        
//    }

//    private void updateGUIKeys()
//    {
//        if ( _gui_key_right )
//            _touch_axis_x += TOUCH_MOVE_SPEED;
//        if ( _gui_key_left )
//            _touch_axis_x -= TOUCH_MOVE_SPEED;
//        
//        _touch_axis_x = Mathf.Clamp( _touch_axis_x, -1.0f, 1.0f );
//    }
#endregion

    /**************************************/
    void Update()
    {
//        updateGUIKeys();
        if (!enabled) {
//            Debug.Log("not enabled");
            return;
        }
#if !TESTMODE
        if (GameController.instance.State.Mode == GameState.GameMode.PAUSED) {
            return;
        }
#endif
        Vector3 pos = transform.position;

        pos.x += Input.GetAxis( "Horizontal" ) * SHIP_SPEED * Time.deltaTime;
        pos.x += _touch_axis_x * SHIP_SPEED * Time.deltaTime;
        pos.x = Mathf.Clamp( pos.x, -SHIP_X_BOUNDS, SHIP_X_BOUNDS );

        if (!_autofire_enabled && Input.GetButton("Fire1")) {
            Fire();
        }

        mSceneSurfacePosition.x = transform.position.x * 0.02f;
        mSceneSurface.transform.position = mSceneSurfacePosition;

        transform.position = pos;
    }

    /**************************************/
    void Fire()
    {               
        if ( mLastFireGO == null )              
            mLastFireGO = SpawnLaserbeam();             
    }

    /**************************************/
//    public void Hurt()
//    {
//        //Instantiate( PainPrefab, transform.position, Quaternion.identity );
//    }

    /**************************************/
    void OnTriggerEnter2D(Collider2D col)
    {
        if (_is_alive) PlayerKilled();
    }

    /**************************************/
    GameObject SpawnLaserbeam()
    {
        Vector3 newpos = transform.position;
        newpos.y += LASER_Y_OFFSET_FROM_SHIP; 
        return Instantiate( LaserbeamPrefab, newpos, Quaternion.identity ) as GameObject;
    }

    /**************************************/
    public void PlayerKilled()
    {
#if TESTMODE
        return;
#endif
        _is_alive = false;
        My_Mesh.SetActive(false);
        enabled = false;

        Destroy(Instantiate( DeathExplosionPrefab, transform.position, Quaternion.identity ), 3.0f);

        GameController.instance.State.Lives--;
        GameController.instance.State.AdjustScore(GameConstants.SCORE_PLAYERDEATH);

        GameController.instance.State.Mode = GameState.GameMode.POSTDEATH;

        if (GameController.instance.State.Lives <= 0) {
            gameObject.SetActive(false);
            GameController.instance.OnGameOver();
        } else {
            DeathPanel.gameObject.SetActive(true);
            GameController.instance.WaveCon.Paused = true;
            GameController.instance.WaveCon.Reset();
            StartCoroutine("PlayerRespawnTimeout");
        }
    }

    /**************************************/
    public IEnumerator PlayerRespawnTimeout()
    {
        yield return new WaitForSeconds(3.0f);
        Reset();
    }

    /**************************************/
    public void Reset()
    {
#if !TESTMODE
        DeathPanel.gameObject.SetActive(false);
        transform.position = _starting_position;
#endif
        this.gameObject.SetActive(true);
        My_Mesh.SetActive(true);
        enabled = true;
        _is_alive = true;
        GameController.instance.State.Mode = GameState.GameMode.RUNNING;
#if !TESTMODE
        GameController.instance.WaveCon.Paused = false;
#endif
    }
}

