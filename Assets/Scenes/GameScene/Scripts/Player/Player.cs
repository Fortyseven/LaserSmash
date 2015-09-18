//#define TESTMODE

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Game;

public class Player : MonoBehaviour
{
    private const float SHIP_SPEED = 13.0f;
    //private const float SHIP_X_BOUNDS = 13.0f;
    private const float LASER_Y_OFFSET_FROM_SHIP = 2.0f;
    private const float FIRE_DELAY = 0.33f;

    public GameObject LaserbeamPrefab = null;
    public GameObject DeathExplosionPrefab = null;

    public Image DeathPanel;
    public GameObject My_Mesh;

    private float _next_fire_time;
    private Vector3 _starting_position;
    private GameObject _scene_surface;
    private Vector3 _scene_surface_position;

    private bool _ready = false;
    private IGameEnvironment _game_environment = null;

    /**************************************/
    public void Awake()
    {
        _ready = false;
        _starting_position = transform.position;
        _next_fire_time = Time.time + FIRE_DELAY;
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
        enabled = false;
    }

    /**************************************/
    public void Init( IGameEnvironment environment )
    {
        _game_environment = environment;
        _ready = true;
    }

    /**************************************/
    public void Done()
    {
        _ready = false;
        enabled = false;
    }

    /**************************************/
    public void Update()
    {
        UpdateBackgroundSurface();

        if ( _ready || !enabled )
            return;

#if !TESTMODE
        //if ( GameController.instance.Status.Mode == GameStatus.GameMode.PAUSED ) {
        //    return;
        //}
#endif
        Vector3 pos = transform.position;

        pos.x += Input.GetAxis( "Horizontal" ) * SHIP_SPEED * Time.deltaTime;
        //pos.x += _touch_axis_x * SHIP_SPEED * Time.deltaTime;
        pos.x = Mathf.Clamp( pos.x, -GameConstants.SCREEN_X_BOUNDS, GameConstants.SCREEN_X_BOUNDS );

        if ( Input.GetButton( "Fire" ) ) {
            Fire();
        }

        transform.position = pos;

        if ( Input.GetButtonDown( "Hyperspace" ) ) {
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
        if ( Time.time >= _next_fire_time ) {
            SpawnLaserbeam();
            _next_fire_time = Time.time + FIRE_DELAY;
        }
    }

    /**************************************/
    void Hyperspace()
    {
        // TODO: beaming animation, from and to, sfx
        Vector3 pos = transform.position;
        pos.x = Random.Range( -GameConstants.SCREEN_X_BOUNDS, GameConstants.SCREEN_X_BOUNDS );
        transform.position = pos;
    }

    /**************************************/
    private void SpawnLaserbeam()
    {
        Vector3 newpos = transform.position;
        newpos.y += LASER_Y_OFFSET_FROM_SHIP;
        Instantiate( LaserbeamPrefab, newpos, Quaternion.identity );
    }

    /**************************************/
    public void Kill()
    {
#if TESTMODE
        return;
#endif
        My_Mesh.SetActive( false );
        enabled = false;

        Destroy( Instantiate( DeathExplosionPrefab, transform.position, Quaternion.identity ), 3.0f );

        _game_environment.Lives--;
        _game_environment.AdjustScore( GameConstants.SCORE_PLAYERDEATH );

        //GameController.instance.Status.Mode = GameStatus.GameMode.POSTDEATH;

        if ( _game_environment.Lives <= 0 ) {
            gameObject.SetActive( false );
            GameController.instance.ChangeState( GameController.NewGameState.GAMEOVER );
        }
        else {
            DeathPanel.gameObject.SetActive( true );
            _game_environment.WaveCon.Paused = true;
            _game_environment.WaveCon.Reset();
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
        //GameController.instance.Status.Mode = GameStatus.GameMode.RUNNING;
    }
}

