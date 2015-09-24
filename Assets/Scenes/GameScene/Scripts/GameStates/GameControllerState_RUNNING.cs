using System;
using UnityEngine;

public class GameControllerState_RUNNING : StateMachineMB.State
{
    public override Enum Name { get { return GameController.GameState.RUNNING; } }

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