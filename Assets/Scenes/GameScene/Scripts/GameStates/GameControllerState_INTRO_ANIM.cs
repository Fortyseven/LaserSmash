using System;
using UnityEngine;

// ReSharper disable once ClassNeverInstantiated.Global
//public class GameControllerState_INTRO_ANIM: StateBehavior
//{
//    public override void Init()
//    {
//        Debug.Log( "GCS_INIT" );
//        ( (GameController)Parent ).PlayerShip = GameObject.Find( "PlayerBase" );
//        ( (GameController)Parent ).PlayerComponent = ( (GameController)Parent ).PlayerShip.GetComponent<Player>();
//        ( (GameController)Parent ).WaveCon = GetComponentInChildren<WaveController>();
//        ( (GameController)Parent ).Status = new GameStatus();

//        NewGame();

//        if ( GameController.DebugMode ) {
//            if ( gameObject.GetComponent<GameTestControls>() == null ) {
//                gameObject.AddComponent<GameTestControls>();
//            }
//        }
//    }

//    public override void OnEnter( Enum changing_from )
//    {
//        Debug.Log( "GCS_ONENTER1" );
//        if ( changing_from == null )
//            return;
//        Debug.Log( "GCS_ONENTER2" );
//        NewGame();
//    }

//    private void NewGame()
//    {
//        Debug.LogWarning("NEW GAME!");
//        //Egg_CockpitCamera.enabled = false;
//        ( (GameController)Parent ).Status.Reset();
//        ( (GameController)Parent ).WaveCon.Reset();
//        ( (GameController)Parent ).PlayerComponent.Reset();
//        ( (GameController)Parent ).SceneReady = false;
//    }

//    public override void OnUpdate()
//    {
//        // Cheeky FPS easter egg
//        //if ( Input.GetKeyDown( KeyCode.F5 ) ) {
//        //    Egg_CockpitCamera.GetComponent<Camera>().enabled = !Egg_CockpitCamera.GetComponent<Camera>().enabled;
//        //}

//        // DEBUG: Artificially increase score
//        if ( Input.GetKeyDown( KeyCode.Home ) ) {
//            ( (GameController)Parent ).Status.AdjustScore( 250 );
//        }

//        // DEBUG: Kill player
//        if ( Input.GetKeyDown( KeyCode.End ) ) {
//            ( (GameController)Parent ).PlayerShip.GetComponent<Player>().Kill();
//        }
//    }
//}