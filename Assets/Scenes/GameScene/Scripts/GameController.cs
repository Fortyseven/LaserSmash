//#define TESTMODE

using UnityEngine;

public class GameController : StateMachineMB
{
    public static GameController instance = null;
    public static bool DebugMode { get; set; }

    //public Camera Egg_CockpitCamera;
    //private GameState _game_state;

    public bool SceneReady { get; set; }

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
        base.Start();

        AddState( new GameControllerState_RUNNING() );
        AddState( new GameControllerState_GAMEOVER() );
        // State change kicks off in Update
    }

    /***************************************************************************/
    private bool _init_kickstart = false;

    public void Update()
    {
        if ( !SceneReady )
            return;

        if ( !_init_kickstart ) {
            _init_kickstart = true;
            ChangeState( NewGameState.RUNNING );
        }
    }

    /***************************************************************************/
    public void OnSceneIntroAnimationComplete()
    {
        SceneReady = true;
    }
}
