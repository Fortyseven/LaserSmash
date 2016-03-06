using UnityEngine;
using System.Collections;
using System;

namespace Game
{
    class MainMenu2 : StateMachineMB
    {
        public Canvas[] _canvases;

        private NavPoints _nav_points;

        public enum PageStates
        {
            PAGE_MAIN, PAGE_CONFIG, PAGE_ABOUT
        };

        /*************************************************************************/
        class Menu_MainMenu : MainMenuPageState
        {
            public override Enum Name { get { return PageStates.PAGE_MAIN; } }

            public Menu_MainMenu( Canvas canvas ) : base( canvas )
            {
                ;
            }

            public override void OnUpdate()
            {

            }

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
        class Menu_Config : MainMenuPageState
        {
            public override Enum Name { get { return PageStates.PAGE_CONFIG; } }

            public Menu_Config( Canvas canvas ) : base( canvas )
            {
                ;
            }

            public override void OnUpdate()
            {

            }

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

            public Menu_About( Canvas canvas ) : base( canvas )
            {
                ;
            }

            public override void OnUpdate()
            {

            }

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
            _nav_points = GetComponent<NavPoints>();

            for ( int i = 0; i < _canvases.Length; i++ ) {
                _canvases[ i ].gameObject.SetActive( false );
            }

            AddState( new Menu_MainMenu( _canvases[ 0 ] ) );
            AddState( new Menu_Config( _canvases[ 1 ] ) );
            AddState( new Menu_About( _canvases[ 2 ] ) );

            ChangeState( PageStates.PAGE_MAIN );
        }

        /*************************************/
        protected new void Update()
        {
            base.Update();

            if ( CurrentState == null || ( (MainMenuPageState)CurrentState ).InTransit ) return;

            if ( Input.GetKeyDown( KeyCode.Alpha1 ) ) {
                ChangePage( PageStates.PAGE_MAIN );
            }
            else if ( Input.GetKeyDown( KeyCode.Alpha2 ) ) {
                ChangePage( PageStates.PAGE_CONFIG );
            }
            else if ( Input.GetKeyDown( KeyCode.Alpha3 ) ) {
                ChangePage( PageStates.PAGE_ABOUT );
            }
        }

        /*************************************/
        private void ChangePage( Enum page )
        {
            if ( CurrentState == page ) return;

            if ( page.Equals( PageStates.PAGE_MAIN ) ) {
                _nav_points.MoveToNavPoint( 0 );
            }
            if ( page.Equals( PageStates.PAGE_CONFIG ) ) {
                _nav_points.MoveToNavPoint( 1 );
            }
            if ( page.Equals( PageStates.PAGE_ABOUT ) ) {
                _nav_points.MoveToNavPoint( 2 );
            }

            ChangeState( page );
        }
    }
}