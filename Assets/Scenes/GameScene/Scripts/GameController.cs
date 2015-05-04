//#define TESTMODE

using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public static bool DebugMode { get; set; }

    public Text _UIScoreValue;
    public Text _UILivesValue;
    public Text _UIMultValue;

    public  Canvas GameOverCanvas;

    //public Camera Egg_CockpitCamera;

    //private GameState _game_state;

    public WaveController WaveCon { get; set; }
    public GameObject PlayerShip { get; set; }
    public Player PlayerComponent { get; set; }
    public GameState State { get; set; }

    public bool SceneReady { get; set; }

    public StateMachine Machine { get; private set; }

    public enum NewGameState
    {
        RUNNING,
        PAUSED,
        GAMEOVER
    }
    
    /***************************************************************************/
    public void Awake()
    {
        instance = this;
    }

    /***************************************************************************/
    public void Start()
    {
        DebugMode = Application.loadedLevelName.Equals( "GameTest" );

        Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer( "Enemy" ), LayerMask.NameToLayer( "Enemy" ) );

        Machine = gameObject.AddComponent<StateMachine>();
        Machine.Init( this );
        Machine.AddState<GameControllerState_RUNNING>( NewGameState.RUNNING );
        Machine.AddState<GameControllerState_GAMEOVER>( NewGameState.GAMEOVER );
    }

    /***************************************************************************/
    private bool _init_kickstart = false;
    public void Update()
    {
        if ( !SceneReady )
            return;

        if ( !_init_kickstart ) {
            _init_kickstart = true;
            Machine.SwitchStateTo( NewGameState.RUNNING );
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
    public void OnSceneIntroAnimationComplete()
    {
        SceneReady = true;
    }
}
