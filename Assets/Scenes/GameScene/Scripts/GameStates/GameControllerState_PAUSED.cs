/************************************************************************
** GameControllerState_PAUSED.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameControllerState_PAUSED : StateMachineMB.State
    {
        public override Enum Name { get { return GameController.GameState.PAUSED; } }

        private StateMachineMB.State _calling_state;
        private GameObject _ui_panel;

        public override void Start()
        {
            SkipUpdateOnZeroTimeScale = false;

            _ui_panel = GameObject.Find( "Pause Canvas" );

            if ( _ui_panel == null ) throw new UnityException( "Could not find 'Pause Canvas' object" );

            _ui_panel.SetActive( false );
        }

        public override void OnStateEnter( StateMachineMB.State from_state )
        {
            _calling_state = from_state;
            Time.timeScale = 0;
            _ui_panel.SetActive( true );
        }

        public override void OnStateExit( StateMachineMB.State to_state )
        {
            Unpause();
        }

        public override void OnUpdate()
        {
            if ( Input.GetKeyDown( KeyCode.Escape ) ) {
                Owner.ChangeState( _calling_state );
            }
        }

        public override void OnStateMessageReceived( object o )
        {
            Unpause();
            // there's really only one place a message will come from
            SceneManager.LoadScene( "MainMenuV2", LoadSceneMode.Single );
        }

        private void Unpause()
        {
            _ui_panel.SetActive( false );
            Time.timeScale = 1;
        }

    }
}