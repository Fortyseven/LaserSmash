/************************************************************************
** GameControllerState_RUNNING.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using System;
using UnityEngine;

namespace Game
{
    public class GameControllerState_RUNNING : StateMachineMB.State
    {
        public override Enum Name { get { return GameController.GameState.RUNNING; } }

        private DebounceButton _pause_debounced = new DebounceButton("Pause");

        public override void OnStateEnter( StateMachineMB.State from_state )
        {
            base.OnStateEnter( from_state );
            _pause_debounced.reset();
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

            if ( _pause_debounced.isPressed() ) {
                Owner.ChangeState( GameController.GameState.PAUSED );
            }
        }
    }
}