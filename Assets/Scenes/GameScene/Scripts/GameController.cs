﻿//#define TESTMODE

using Game;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GameController : StateMachineMB
{
    public static GameController instance = null;
    public static bool DebugMode { get; private set; }

    private GameObject _scene_surface;
    private Vector3 _scene_surface_position;

    public GameEnvironment GameEnv { get; private set; }

    //public Camera Egg_CockpitCamera;

    public bool SceneReady { get; set; }

    public enum GameState
    {
        INTRO_ANIM,
        RUNNING,
        PLAYER_DYING,
        PAUSED,
        GAMEOVER
    }

    /**************************************/
    public void Awake()
    {
        Init.Construct( true );

        Debug.Log( "GameController - Awake" );

        instance = this;

        //DebugMode = Application.loadedLevelName.Equals( "GameTest" );

        Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer( "Enemy" ), LayerMask.NameToLayer( "Enemy" ) );
    }
    /**************************************/

    [UsedImplicitly]
    public void Start()
    {
        AddState( new GameControllerState_INTRO_ANIM() );
        AddState( new GameControllerState_RUNNING() );
        AddState( new GameControllerState_GAMEOVER() );
        AddState( new GameControllerState_PLAYER_DYING() );

        SetupBackgroundSceneControl();


        ChangeState( GameState.INTRO_ANIM );
    }

    /**************************************/
    private void SetupBackgroundSceneControl()
    {
        _scene_surface = GameObject.Find( "Surface" );
        if ( _scene_surface == null ) {
            throw new UnityException( "Could not find stage surface" );
        }

        _scene_surface_position = _scene_surface.transform.position;
    }

    /**************************************/
    public new void Update()
    {
        UpdateBackgroundSurface();
    }

    /**************************************/
    private void UpdateBackgroundSurface()
    {
        _scene_surface_position.x = transform.position.x * 0.02f;
        _scene_surface.transform.position = _scene_surface_position;
    }

    /***************************************************************************/
    public void OnSceneIntroAnimationComplete()
    {
        //SceneReady = true;
        CurrentState.SendMessage();
    }

    public void ResetGameEnvironment()
    {
        GameEnv = new GameEnvironment( this.gameObject );
    }


    public void KillPlayer()
    {
        GameEnv.PlayerComponent.ChangeState( Player.PlayerState.KILLED );
    }

    public class GameEnvironment : IGameEnvironment
    {
        public GameStatus Status { get; private set; }
        public WaveController WaveCon { get; private set; }
        public GameObject PlayerShip { get; private set; }
        public Player PlayerComponent { get; private set; }

        private int _score;
        private int _peak_score;
        private int _last_life_peak_score;
        private int _mult;
        private int _lives;

        private Text _ui_score_value;
        private Text _ui_lives_value;
        private Text _ui_mult_value;

        public GameEnvironment( GameObject owner )
        {
            Debug.LogWarning( "NEW GAME!" );
            //Egg_CockpitCamera.enabled = false;
            //PlayerComponent.Init();

            // Fetch UI objects
            _ui_score_value = GameObject.Find( "UI_ScoreValue" ).GetComponent<Text>();
            _ui_mult_value = GameObject.Find( "UI_MultValue" ).GetComponent<Text>();
            _ui_lives_value = GameObject.Find( "UI_LivesValue" ).GetComponent<Text>();

            PlayerShip = GameObject.Find( "PlayerBase" );

            if ( PlayerShip == null )
                throw new UnityException( "PlayerBase object not found" );

            PlayerComponent = PlayerShip.GetComponent<Player>();

            if ( PlayerComponent == null )
                throw new UnityException( "Player Component not found" );

            WaveCon = owner.GetComponentInChildren<WaveController>();
            WaveCon.Init();
            WaveCon.Paused = false;

            Score = 0;
            Multiplier = GameConstants.INITIAL_MULTIPLIER;
            Lives = GameConstants.INITIAL_LIVES;
            PlayerComponent.ChangeState( Player.PlayerState.RESET );
        }

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
}