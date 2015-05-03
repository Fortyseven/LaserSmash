//#define TESTMODE

using UnityEngine;

using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const float GAMEOVER_TIMEOUT = 4.0f;

    public static GameController instance = null;

    /*---------------------------------------------------------*/

    public Text _UIScoreValue;
    public Text _UILivesValue;
    public Text _UIMultValue;

    public  Canvas GameOverCanvas;

    //public Camera Egg_CockpitCamera;

    private GameState _game_state;
    private GameObject _player_ship;
    private WaveController _wave_controller;
    private Player _player_component;

    private float _game_over_timeout;
    private bool _game_over_message_enabled;

    #region properties
    public WaveController WaveCon
    {
        get { return _wave_controller; }
    }
    public GameObject PlayerShip
    {
        get { return _player_ship; }
    }
    public Player PlayerComponent
    {
        get { return _player_component; }
    }
    public GameState State
    {
        get { return _game_state; }
    }

    public bool SceneReady { get; private set; }
    public bool DebugMode { get; private set; }

    #endregion


    /***************************************************************************/
    public void Awake()
    {
        instance = this;
        //_prevcolor = TopLight.color;
        //        Application.targetFrameRate = 30;
    }

    /***************************************************************************/
    public void Start()
    {
        _player_ship = GameObject.Find( "PlayerBase" );
        _player_component = _player_ship.GetComponent<Player>();

        Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer( "Enemy" ), LayerMask.NameToLayer( "Enemy" ) );
        ConfigureGame();

        _wave_controller = GetComponentInChildren<WaveController>();
        _game_state = new GameState();
        NewGame();
        if ( Application.loadedLevelName.Equals( "GameTest" ) ) {
            DebugMode = true;
            if ( gameObject.GetComponent<GameTestControls>() == null ) {
                gameObject.AddComponent<GameTestControls>();
            }
        }
    }

    /***************************************************************************/
    public void Update()
    {
        if ( !SceneReady )
            return;

        if ( DebugMode ) {
            //DebugModeUpdate();
            return;
        }

        switch ( State.Mode ) {
            case GameState.GameMode.GAMEOVER:
                UpdateGameOver();
                break;

            case GameState.GameMode.RUNNING:
                UpdateRunning();
                break;
        }
    }

    /***************************************************************************/
    private void UpdateRunning()
    {
        // Cheeky FPS easter egg
        //if ( Input.GetKeyDown( KeyCode.F5 ) ) {
        //    Egg_CockpitCamera.GetComponent<Camera>().enabled = !Egg_CockpitCamera.GetComponent<Camera>().enabled;
        //}

        // DEBUG: Artificially increase score
        if ( Input.GetKeyDown( KeyCode.Home ) ) {
            State.AdjustScore( 250 );
        }

        // DEBUG: Kill player
        if ( Input.GetKeyDown( KeyCode.End ) ) {
            _player_ship.GetComponent<Player>().Kill();
        }
    }
    /***************************************************************************/
    private void UpdateGameOver()
    {
        if ( Time.time >= _game_over_timeout && !_game_over_message_enabled ) {
            GameOverCanvas.GetComponentInChildren<Flash>().Go();
            _game_over_message_enabled = true;
        }
        if ( Input.anyKeyDown && ( Time.time >= _game_over_timeout ) ) {
            NewGame();
        }
    }

    /***************************************************************************/
    public void SetScoreValue( int score )
    {
        _UIScoreValue.text = score.ToString();
    }

    /***************************************************************************/
    public void SetMultValue( int mult )
    {
        _UIMultValue.text = mult.ToString() + "x";
    }

    /***************************************************************************/
    public void SetLivesValue( int lives )
    {
        _UILivesValue.text = lives.ToString();
    }

    /***************************************************************************/
    private void ConfigureGame()
    {
        //        DifficultyController.Load();

#if !UNITY_ANDROID
        //        DisableOnScreenControls();
#endif
    }

#if !UNITY_ANDROID
    //    /***************************************************************************/
    //    private void DisableOnScreenControls()
    //    {
    //        GameObject pan = GameObject.Find( "Input_Left" ) as GameObject;
    //        pan.SetActive(false);
    //        pan = GameObject.Find( "Input_Right" ) as GameObject;
    //        pan.SetActive(false);
    //    }
#endif

    /***************************************************************************/
    public void OnGameOver()
    {
        instance.State.Mode = GameState.GameMode.GAMEOVER;
        GameOverCanvas.gameObject.SetActive( true );
        Text peak_score_value = GameObject.Find( "PeakScoreValue" ).GetComponent<Text>();
        peak_score_value.text = State.PeakScore.ToString();
        _game_over_timeout = Time.time + GAMEOVER_TIMEOUT;
        _game_over_message_enabled = false;
    }

    public void NewGame()
    {
#if !TESTMODE
        GameOverCanvas.gameObject.SetActive( false );
#endif
        Flash f = GameOverCanvas.GetComponentInChildren<Flash>();
        if ( f )
            f.ResetFlashers();

        //Egg_CockpitCamera.enabled = false;
        State.Reset();
        _wave_controller.Reset();
        PlayerComponent.Reset();
        SceneReady = false;
    }

    public void OnSceneReady()
    {
        SceneReady = true;
    }
}
