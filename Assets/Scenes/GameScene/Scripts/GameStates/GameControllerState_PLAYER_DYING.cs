/************************************************************************
** GameControllerState_PLAYER_DYING.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameControllerState_PLAYER_DYING : StateMachineMB.State
    {

        // Use this for initialization
        public override Enum Name { get { return GameController.GameState.PLAYER_DYING; } }
        private Image _death_panel_image; // The red death filter while exploding

        public override void Start()
        {
            _death_panel_image = GameObject.Find( "DeathPanel" ).GetComponent<Image>();
            _death_panel_image.gameObject.SetActive( false );
        }

        public override void OnStateEnter( StateMachineMB.State from_state )
        {
            _death_panel_image.gameObject.SetActive( true );

            GameController.instance.GameEnv.Lives--;
            GameController.instance.GameEnv.AdjustScore( GameConstants.SCORE_PLAYERDEATH );

            GameController.instance.GameEnv.WaveCon.Paused = true;
            GameController.instance.GameEnv.WaveCon.Reset();
        }

        public override void OnStateExit( StateMachineMB.State to_state )
        {
            GameController.instance.GameEnv.WaveCon.Paused = false;
        }

        // Triggered at the end of the death animation
        public override void OnStateMessageReceived( object o )
        {
            if ( GameController.instance.GameEnv.Lives <= 0 ) {
                Owner.ChangeState( GameController.GameState.GAMEOVER );
            }
            else {
                _death_panel_image.gameObject.SetActive( false );
                GameController.instance.GameEnv.PlayerComponent.ChangeState( Player.PlayerState.RESET );
                Owner.ChangeState( GameController.GameState.RUNNING );
            }
        }

        public override void OnUpdate()
        {
            ;
        }
    }
}