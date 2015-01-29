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
    public GameObject My_Mesh;

    private GameObject _last_fire_go;
    private Vector3 _starting_position;
    private float _touch_axis_x = 0.0f;
    private GameObject _scene_surface;
    private Vector3 _scene_surface_position;
    //private bool _is_alive = false;
    public bool IsAlive { get; set; }
    /**************************************/
    public void Awake()
    {
        _starting_position = transform.position;
    }

    /**************************************/
    public void Start()
    {
        _scene_surface = GameObject.Find( "Surface" );
        if ( _scene_surface == null ) {
            throw new UnityException( "Could not find stage surface" );
        }

        _scene_surface_position = _scene_surface.transform.position;

        Reset();
        enabled = true;
    }

    /**************************************/
    public void Update()
    {
        UpdateBackgroundSurface();

        if ( !GameController.instance.SceneReady || !enabled )
            return;

#if !TESTMODE
        if ( GameController.instance.State.Mode == GameState.GameMode.PAUSED ) {
            return;
        }
#endif
        Vector3 pos = transform.position;

        pos.x += Input.GetAxis( "Horizontal" ) * SHIP_SPEED * Time.deltaTime;
        //pos.x += _touch_axis_x * SHIP_SPEED * Time.deltaTime;
        pos.x = Mathf.Clamp( pos.x, -SHIP_X_BOUNDS, SHIP_X_BOUNDS );

        if ( Input.GetButton( "Fire1" ) ) {
            Fire();
        }

        transform.position = pos;

        if ( Input.GetKeyDown( KeyCode.W ) ) {
            Hyperspace();
        }
    }

    /**************************************/
    private void UpdateBackgroundSurface()
    {
        _scene_surface_position.x = transform.position.x * 0.02f;
        _scene_surface.transform.position = _scene_surface_position;
    }

    /**************************************/
    void Fire()
    {
        if ( _last_fire_go == null )
            _last_fire_go = SpawnLaserbeam();
    }

    /**************************************/
    void Hyperspace()
    {
        // TODO: beaming animation, from and to, sfx
        Vector3 pos = transform.position;
        pos.x = Random.Range( -SHIP_X_BOUNDS, SHIP_X_BOUNDS );
        transform.position = pos;
    }

    /**************************************/
    private GameObject SpawnLaserbeam()
    {
        Vector3 newpos = transform.position;
        newpos.y += LASER_Y_OFFSET_FROM_SHIP;
        return Instantiate( LaserbeamPrefab, newpos, Quaternion.identity ) as GameObject;
    }

    /**************************************/
    public void Kill()
    {
#if TESTMODE
        return;
#endif
        IsAlive = false;
        My_Mesh.SetActive( false );
        enabled = false;

        Destroy( Instantiate( DeathExplosionPrefab, transform.position, Quaternion.identity ), 3.0f );

        GameController.instance.State.Lives--;
        GameController.instance.State.AdjustScore( GameConstants.SCORE_PLAYERDEATH );

        GameController.instance.State.Mode = GameState.GameMode.POSTDEATH;

        if ( GameController.instance.State.Lives <= 0 ) {
            gameObject.SetActive( false );
            GameController.instance.OnGameOver();
        }
        else {
            DeathPanel.gameObject.SetActive( true );
            GameController.instance.WaveCon.Paused = true;
            GameController.instance.WaveCon.Reset();
            StartCoroutine( "PlayerRespawnTimeout" );
        }
    }

    /**************************************/
    public IEnumerator PlayerRespawnTimeout()
    {
        yield return new WaitForSeconds( 3.0f );
        Reset();
    }

    /**************************************/
    public void Reset()
    {
#if !TESTMODE
        DeathPanel.gameObject.SetActive( false );
        transform.position = _starting_position;
#endif
        gameObject.SetActive( true );
        My_Mesh.SetActive( true );
        enabled = true;
        IsAlive = true;
        GameController.instance.State.Mode = GameState.GameMode.RUNNING;
#if !TESTMODE
        GameController.instance.WaveCon.Paused = false;
#endif
    }
}

