//#define TESTMODE

using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public static bool DebugMode { get; set; }

    //public Camera Egg_CockpitCamera;

    //private GameState _game_state;


    public bool SceneReady { get; set; }

    public StateMachine Machine { get; private set; }

    public enum NewGameState
    {
        //        INTRO_ANIM,
        RUNNING,
        PAUSED,
        GAMEOVER
    }

    /***************************************************************************/
    public void Awake()
    {
        Debug.Log( "GameContr - Awake" );
        instance = this;

        DebugMode = Application.loadedLevelName.Equals( "GameTest" );

        Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer( "Enemy" ), LayerMask.NameToLayer( "Enemy" ) );
    }

    /***************************************************************************/
    public void Start()
    {
        Machine = gameObject.AddComponent<StateMachine>();
        Machine.Init( this );
        //      Machine.AddState<GameControllerState_INTRO_ANIM>(NewGameState.INTRO_ANIM);
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
    public void OnSceneIntroAnimationComplete()
    {
        SceneReady = true;
    }
}
