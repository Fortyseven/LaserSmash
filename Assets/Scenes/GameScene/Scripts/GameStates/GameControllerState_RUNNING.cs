using System;
using Game;
using UnityEngine;


public class GameControllerState_RUNNING : StateMachineMB.State
{
    /***********************************************************/
    private void NewGame()
    {

        ( (GameController)Owner ).ResetGameEnvironment();


        ( (GameController)Owner ).SceneReady = false;
    }

    /***********************************************************/
    public override Enum Name
    { get { return GameController.GameState.RUNNING; } }

    /***********************************************************/
    public override void Start()
    {


        //if ( GameController.DebugMode ) {
        //    if ( gameObject.GetComponent<GameTestControls>() == null ) {
        //        gameObject.AddComponent<GameTestControls>();
        //    }
        //}
    }

    public override void OnStateEnter( StateMachineMB.State from_state )
    {
        if ( from_state == null )
            return;

        NewGame();
    }

    public override void OnStateExit( StateMachineMB.State to_state )
    {
        //throw new NotImplementedException();
    }

    public override void OnUpdate()
    {
        // Cheeky FPS easter egg
        //if ( Input.GetKeyDown( KeyCode.F5 ) ) {
        //    Egg_CockpitCamera.GetComponent<Camera>().enabled = !Egg_CockpitCamera.GetComponent<Camera>().enabled;
        //}

        // DEBUG: Artificially increase score
        if ( Input.GetKeyDown( KeyCode.Home ) ) {
            GameController.instance.GameEnv.AdjustScore( 250 );
        }

        // DEBUG: Kill player
        if ( Input.GetKeyDown( KeyCode.End ) ) {
            //GameController.instance.GameEnv.PlayerShip.GetComponent<Player>().ChangeState(Player.PlayerState_KILLED);
            GameController.instance.KillPlayer();
        }
    }

}