//#define TESTMODE

using JetBrains.Annotations;
using UnityEngine;

public class GameController : StateMachineMB
{
    public static GameController instance = null;
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
        base.Update();
        UpdateBackgroundSurface();
    }

    /**************************************/
    private void UpdateBackgroundSurface()
    {
        _scene_surface_position.x = transform.position.x * 0.02f;
        _scene_surface.transform.position = _scene_surface_position;
    }

    /**************************************/
    public void OnSceneIntroAnimationComplete()
    {
        //SceneReady = true;
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