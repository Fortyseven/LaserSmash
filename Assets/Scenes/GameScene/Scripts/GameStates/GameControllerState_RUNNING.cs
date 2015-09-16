using System;
using Game;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once ClassNeverInstantiated.Global
public class GameControllerState_RUNNING : StateBehavior, IGameEnvironment
{
    public GameStatus Status { get { return _status; } }
    public WaveController WaveCon { get { return _wave_controller; } }
    public GameObject PlayerShip { get { return _player_ship; } }
    public Player PlayerComponent { get { return _player_component; } }

    public int Lives
    {
        get { return _lives; }
        set
        {
            // Only adjust lives when the game is running; once it's
            // at zero, only a Reset() will unlock it
            //if ( Mode == GameMode.GAMEOVER )
            //    return;

            _lives = value;
            SetLivesValue( value );
        }
    }

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            SetScoreValue( value );
            ValidateMultplier();
        }
    }

    public int PeakScore
    {
        get { return _peak_score; }
        set { _peak_score = value; }
    }

    public int Multiplier
    {
        get { return _mult; }
        set
        {
            _mult = value;
            SetMultValue( _mult );
        }
    }


    private GameStatus _status;
    private WaveController _wave_controller;
    private GameObject _player_ship;
    private Player _player_component;

    private Text _ui_score_value;
    private Text _ui_lives_value;
    private Text _ui_mult_value;

    private int _score;
    private int _peak_score;
    private int _last_life_peak_score;
    private int _mult;
    private int _lives;


    /***********************************************************/
    /***********************************************************/

    public override void Init()
    {
    }

    private void Setup()
    {
        // Fetch UI objects
        _ui_score_value = GameObject.Find( "UI_ScoreValue" ).GetComponent<Text>();
        _ui_mult_value = GameObject.Find( "UI_MultValue" ).GetComponent<Text>();
        _ui_lives_value = GameObject.Find( "UI_LivesValue" ).GetComponent<Text>();


        _player_ship = GameObject.Find( "PlayerBase" );

        if ( _player_ship == null )
            throw new UnityException( "PlayerBase object not found" );

        _player_component = PlayerShip.GetComponent<Player>();

        if ( _player_component == null )
            throw new UnityException( "Player Component not found" );

        _wave_controller = GetComponentInChildren<WaveController>();
        _status = new GameStatus();

        WaveCon.Init( this );


        if ( GameController.DebugMode ) {
            if ( gameObject.GetComponent<GameTestControls>() == null ) {
                gameObject.AddComponent<GameTestControls>();
            }
        }
    }
    public override void OnEnter( Enum changing_from )
    {
        if ( changing_from == null )
            return;
        Setup();
        NewGame();
    }

    private void NewGame()
    {
        Debug.LogWarning( "NEW GAME!" );
        //Egg_CockpitCamera.enabled = false;
        PlayerComponent.Reset();
        WaveCon.Paused = false;
        Score = 0;
        Multiplier = GameConstants.INITIAL_MULTIPLIER;
        Lives = GameConstants.INITIAL_LIVES;

        ( (GameController)Parent ).SceneReady = false;
    }

    public override void OnUpdate()
    {
        // Cheeky FPS easter egg
        //if ( Input.GetKeyDown( KeyCode.F5 ) ) {
        //    Egg_CockpitCamera.GetComponent<Camera>().enabled = !Egg_CockpitCamera.GetComponent<Camera>().enabled;
        //}

        // DEBUG: Artificially increase score
        if ( Input.GetKeyDown( KeyCode.Home ) ) {
            AdjustScore( 250 );
        }

        // DEBUG: Kill player
        if ( Input.GetKeyDown( KeyCode.End ) ) {
            PlayerShip.GetComponent<Player>().Kill();
        }
    }

    /***************************************************************************/
    public void SetScoreValue( int score )
    {
        _ui_score_value.text = score.ToString();
    }

    /***************************************************************************/
    public void SetMultValue( int mult )
    {
        _ui_mult_value.text = mult.ToString() + "x";
    }

    /***************************************************************************/
    public void SetLivesValue( int lives )
    {
        _ui_lives_value.text = lives.ToString();
    }

    public void AdjustScore( int score_offset )
    {
        Score += score_offset * Multiplier;

        if ( Score > _peak_score ) {
            _peak_score = Score;
            if ( _peak_score - _last_life_peak_score > 1000 ) {
                _last_life_peak_score = _peak_score;
                Lives++;
                //TODO: Add 1UP noise
            }
        }
    }

    /***************************************************************************/
    private void ValidateMultplier()
    {
        int cur_mult = Multiplier;

        if ( Score < GameConstants.SCORE_THRESH_2X ) {
            Multiplier = 1;
        }
        else if ( Score < GameConstants.SCORE_THRESH_3X ) {
            Multiplier = 2;
        }
        else if ( Score < GameConstants.SCORE_THRESH_4X ) {
            Multiplier = 3;
        }
        else if ( Score < GameConstants.SCORE_THRESH_5X ) {
            Multiplier = 4;
        }
        else if ( Score < GameConstants.SCORE_THRESH_6X ) {
            Multiplier = 5;
        }
        else {
            Multiplier = 6;
        }

        if ( cur_mult != Multiplier )
            OnMultChange();
    }

    private void OnMultChange()
    {
        //switch ( Multiplier ) {
        //    case 2: // Blue
        //        GameController.instance.DoLevelTransition( new Color( 0, 0, 1.0f, 1.0f ) );
        //        break;
        //    case 3: // Purple
        //        GameController.instance.DoLevelTransition( new Color( 1.0f, 0, 1.0f, 1.0f ) );
        //        break;
        //    case 4: // Cyan
        //        GameController.instance.DoLevelTransition( new Color( 0, 1.0f, 1.0f, 1.0f ) );
        //        break;
        //    case 5: // Gray
        //        GameController.instance.DoLevelTransition( new Color( 1.0f, 0.75f, 0, 1.0f ) );
        //        break;
        //    default: // "Black"
        //        GameController.instance.DoLevelTransition( new Color( 0.5f, 0.5f, 0.5f, 1.0f ) );
        //        break;
        //}
    }

}