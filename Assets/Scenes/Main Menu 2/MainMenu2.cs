using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    class MainMenu2 : StateMachineMB
    {
        public Canvas[] _canvases;
        public Canvas _canvas_loading;

        public Text LabelVersion;

        private NavPoints _nav_points;

        public enum PageStates { PAGE_MAIN, PAGE_CONFIG, PAGE_ABOUT, PAGE_CONTROLS };

        /*************************************************************************/
        class Menu_MainMenu : MainMenuPageState
        {
            public override Enum Name { get { return PageStates.PAGE_MAIN; } }

            public Menu_MainMenu( Canvas canvas ) : base( canvas ) {; }

            public override void OnUpdate() {; }

            public override void OnStateEnter( State from )
            {
                base.OnStateEnter( from );
            }

            public override void OnStateExit( State from )
            {
                base.OnStateExit( from );
            }
        }

        /*************************************************************************/
        class Menu_About : MainMenuPageState
        {
            public override Enum Name { get { return PageStates.PAGE_ABOUT; } }

            public Menu_About( Canvas canvas ) : base( canvas ) {; }

            public override void OnUpdate() {; }

            public override void OnStateEnter( State from )
            {
                base.OnStateEnter( from );
            }
            public override void OnStateExit( State from )
            {
                base.OnStateExit( from );
            }

        }

        /*************************************************************************/
        /*************************************************************************/
        void Start()
        {
            LabelVersion.text = Game.GameConstants.VERSION_STRING;
            _canvas_loading.gameObject.SetActive( false );
            _nav_points = GetComponent<NavPoints>();

            for ( int i = 0; i < _canvases.Length; i++ ) {
                _canvases[ i ].gameObject.SetActive( false );
            }

            AddState( new Menu_MainMenu( _canvases[ 0 ] ) );
            AddState( new Menu_Controls( _canvases[ 1 ] ) );
            AddState( new Menu_About( _canvases[ 2 ] ) );

            ChangeState( PageStates.PAGE_MAIN );
        }

        /*********************************************/
        protected new void Update()
        {
            base.Update();

            if ( CurrentState == null || ( (MainMenuPageState)CurrentState ).InTransit ) {
                return;
            }

            //DEBUG
            //if ( Input.GetKeyDown( KeyCode.Alpha1 ) ) {
            //    ChangePage( PageStates.PAGE_MAIN );
            //}
            //else if ( Input.GetKeyDown( KeyCode.Alpha2 ) ) {
            //    ChangePage( PageStates.PAGE_CONTROLS );
            //}
            //else if ( Input.GetKeyDown( KeyCode.Alpha3 ) ) {
            //    ChangePage( PageStates.PAGE_ABOUT );
            //}
        }

        /*********************************************/
        private void ChangePage( PageStates page )
        {
            if ( CurrentState == page || ( (MainMenuPageState)CurrentState ).InTransit ) {
                return;
            }

            switch ( page ) {
                case PageStates.PAGE_MAIN:
                    _nav_points.MoveToNavPoint( 0 );
                    break;
                case PageStates.PAGE_ABOUT:
                    _nav_points.MoveToNavPoint( 2 );
                    break;
                case PageStates.PAGE_CONFIG:
                case PageStates.PAGE_CONTROLS:
                    _nav_points.MoveToNavPoint( 3 );
                    break;
                default:
                    Debug.LogError( "Invalid page state requested" );
                    return;
            }

            ChangeState( page );
        }

        /*********************************************/
        //private IEnumerator CO_StartGame()
        //{
        //    _canvas_loading.gameObject.SetActive( true );
        //    AsyncOperation async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        //    AsyncOperation async_bg = SceneManager.LoadSceneAsync( "BG1", LoadSceneMode.Additive ); // TODO: For future expansion

        //    while ( async.progress < 1.0f && async_bg.progress < 1.0f ) {
        //        Debug.Log( async.progress + " - " + async_bg.progress );
        //        yield return null;
        //    }
        //    _canvas_loading.gameObject.SetActive( false );
        //}
        /*********************************************/
        public void OnClick_Start()
        {
            // StartCoroutine( CO_StartGame() );
            SceneManager.LoadScene( 1, LoadSceneMode.Single );
        }

        public void OnClick_Keys()
        {
            ChangePage( PageStates.PAGE_CONTROLS );
        }

        public void OnClick_Help()
        {
            ChangePage( PageStates.PAGE_ABOUT );
        }

        public void OnClick_Return()
        {
            ChangePage( PageStates.PAGE_MAIN );
        }

        public void OnClick_Quit()
        {
            Application.Quit();
        }

        public void OnClick_Config( int input_id )
        {
            if (CurrentState == PageStates.PAGE_CONTROLS) {
                CurrentState.SendStateMessage( input_id );
            }
        }
        /*********************************************/
    }
}