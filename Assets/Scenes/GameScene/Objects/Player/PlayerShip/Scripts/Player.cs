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
    
    float _touch_axis_x = 0.0f;   

    GameObject mSceneSurface = null;
    Vector3 mSceneSurfacePosition;

    bool _is_alive = false;

    /**************************************/
    void Awake()
    {
        _starting_position = transform.position;
    }

    /**************************************/
    void Start()
    {
        mSceneSurface = GameObject.Find("Surface");
        if (mSceneSurface == null) {
            throw new UnityException("Could not find stage surface");
        }

        mSceneSurfacePosition = mSceneSurface.transform.position;

        Reset();
        enabled = true;
    }

    /**************************************/
    void Update()
    {
        if (!enabled) {
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

        if (Input.GetButton("Fire1")) {
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

