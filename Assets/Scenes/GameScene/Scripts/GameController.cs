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
        public Canvas LoadingCanvas;

        [HideInInspector]
        public CameraShaker Shaker;

        public bool SceneReady { get; set; }

        public enum GameState
        {
            INTRO_ANIM,
            RUNNING,
            PLAYER_DYING,
            PAUSED,
            GAMEOVER
        }

        private AsyncOperation _bg_load_async;
        private bool _bg_loading;

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
            LoadingCanvas.gameObject.SetActive( true );
            _bg_loading = true;
            _bg_load_async = SceneManager.LoadSceneAsync( "BG1", LoadSceneMode.Additive );

            Shaker = gameObject.AddComponent<CameraShaker>();

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
        }

        /**************************************/
        public new void Update()
        {
            if ( _bg_loading ) {
                if ( _bg_load_async.progress >= 1.0f ) {
                    _bg_loading = false;
                    LoadingCanvas.gameObject.SetActive( false );
                }
            }

            base.Update();

            UpdateCameraPan();
        }

        /**************************************/
        private void UpdateCameraPan()
        {
            _initial_camera_position.x = GameEnv.PlayerShip.transform.position.x * CAMERA_PAN_FACTOR;
            Camera.main.transform.localPosition = _initial_camera_position + Shaker.GetOffset();
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
                GameController.instance.Shaker.SHAKE( 2.5f, 1.0f );
                GameEnv.PlayerComponent.ChangeState( Player.PlayerState.KILLED );
            }
        }
    }
}