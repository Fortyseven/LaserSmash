//#define TESTMODE

using JetBrains.Annotations;
using UnityEngine;

public class GameController : StateMachineMB
{
    private const float CAMERA_PAN_FACTOR = 0.10f;

    public static GameController instance = null;
    private Vector3 _initial_camera_position;

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
        _initial_camera_position = Camera.main.transform.localPosition;
    }

    /**************************************/
    public new void Update()
    {
        base.Update();
        UpdateCameraPan();
    }

    /**************************************/
    private void UpdateCameraPan()
    {
        _initial_camera_position.x = GameEnv.PlayerShip.transform.position.x * CAMERA_PAN_FACTOR;
        Camera.main.transform.localPosition = _initial_camera_position;
    }

    /**************************************/
    public void OnSceneIntroAnimationComplete()
    {
        CurrentState.SendMessage();
    }

    /**************************************/
    public void ResetGameEnvironment()
    {
        GameEnv = new GameEnvironment( this.gameObject );
    }

    /**************************************/
    public void KillPlayer()
    {
        GameEnv.PlayerComponent.ChangeState( Player.PlayerState.KILLED );
    }
}