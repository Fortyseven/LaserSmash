using System;
using UnityEngine;

// ReSharper disable once ClassNeverInstantiated.Global
public class GameControllerState_RUNNING : StateBehavior
{
    public override void Init()
    {
        ( (GameController)Parent ).PlayerShip = GameObject.Find( "PlayerBase" );
        ( (GameController)Parent ).PlayerComponent = ( (GameController)Parent ).PlayerShip.GetComponent<Player>();
        ( (GameController)Parent ).WaveCon = GetComponentInChildren<WaveController>();
        ( (GameController)Parent ).State = new GameState();

        NewGame();

        if ( GameController.DebugMode ) {
            if ( gameObject.GetComponent<GameTestControls>() == null ) {
                gameObject.AddComponent<GameTestControls>();
            }
        }
    }

    public override void OnEnter( Enum changing_from )
    {
        if ( changing_from == null )
            return;

        NewGame();
    }

    private void NewGame()
    {
#if !TESTMODE
        ( (GameController)Parent ).GameOverCanvas.gameObject.SetActive( false );
#endif
        Flash f = ( (GameController)Parent ).GameOverCanvas.GetComponentInChildren<Flash>();
        if ( f )
            f.ResetFlashers();

        //Egg_CockpitCamera.enabled = false;
        ( (GameController)Parent ).State.Reset();
        ( (GameController)Parent ).WaveCon.Reset();
        ( (GameController)Parent ).PlayerComponent.Reset();
        ( (GameController)Parent ).SceneReady = false;
    }

    public override void OnUpdate()
    {
        // Cheeky FPS easter egg
        //if ( Input.GetKeyDown( KeyCode.F5 ) ) {
        //    Egg_CockpitCamera.GetComponent<Camera>().enabled = !Egg_CockpitCamera.GetComponent<Camera>().enabled;
        //}

        // DEBUG: Artificially increase score
        if ( Input.GetKeyDown( KeyCode.Home ) ) {
            ( (GameController)Parent ).State.AdjustScore( 250 );
        }

        // DEBUG: Kill player
        if ( Input.GetKeyDown( KeyCode.End ) ) {
            ( (GameController)Parent ).PlayerShip.GetComponent<Player>().Kill();
        }
    }
}