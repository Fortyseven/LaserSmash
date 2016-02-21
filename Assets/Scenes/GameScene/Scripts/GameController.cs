/************************************************************************
** GameController.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameController : StateMachineMB
    {
        private const float CAMERA_PAN_FACTOR = 0.10f;

        public static GameController instance = null;
        private Vector3 _initial_camera_position;
        public BarrierGroup Barriers;
        public GameEnvironment GameEnv { get; private set; }
        public AudioClip Snd1UpClip;

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
            AddState( new GameControllerState_PAUSED() );

            SetupBackgroundSceneControl();

            ChangeState( GameState.INTRO_ANIM );
        }

        /**************************************/
        private void SetupBackgroundSceneControl()
        {
            _initial_camera_position = Camera.main.transform.localPosition;
            SceneManager.LoadScene( "BG1", LoadSceneMode.Additive ); // TODO: For future expansion
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
            CurrentState.SendStateMessage();
        }

        /**************************************/
        public void ResetGameEnvironment()
        {
            GameEnv = new GameEnvironment( this.gameObject );
        }

        /**************************************/
        public void KillPlayer()
        {
            if ( GameController.instance.CurrentState == GameController.GameState.RUNNING ) {
                GameEnv.PlayerComponent.ChangeState( Player.PlayerState.KILLED );
            }
        }
    }
}