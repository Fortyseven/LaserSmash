using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /*************************************************************************/
    class Menu_Controls : MainMenuPageState
    {
        public override Enum Name { get { return MainMenu2.PageStates.PAGE_CONTROLS; } }

        private Text mKeyValueLeft = null;
        private Text mKeyValueRight = null;
        private Text mKeyValueFire = null;
        private Text mKeyValueHyper = null;
        private Text mKeyValuePause = null;

        private Text mJoyValueLeft = null;
        private Text mJoyValueRight = null;
        private Text mJoyValueFire = null;
        private Text mJoyValueHyper = null;
        private Text mJoyValuePause = null;

        private bool mIsInitialized = false;

        public enum InputIDType { LEFT = 0, RIGHT = 1, FIRE = 2, HYPER = 3, PAUSE = 4 };

        public bool mEditMode = false;
        public Text mEditKeyString = null;
        public Text mEditJoyString = null;

        public Menu_Controls( Canvas canvas ) : base( canvas ) {; }

        public override void OnStateEnter( StateMachineMB.State from )
        {
            base.OnStateEnter( from );
            if( !mIsInitialized ) {

                mKeyValueLeft = GameObject.Find( "keyValueLeft" ).GetComponent<Text>();
                mKeyValueRight = GameObject.Find( "keyValueRight" ).GetComponent<Text>();
                mKeyValueFire = GameObject.Find( "keyValueFire" ).GetComponent<Text>();
                mKeyValueHyper = GameObject.Find( "keyValueHyper" ).GetComponent<Text>();
                mKeyValuePause = GameObject.Find( "keyValuePause" ).GetComponent<Text>();

                mJoyValueLeft = GameObject.Find( "joyValueLeft" ).GetComponent<Text>();
                mJoyValueRight = GameObject.Find( "joyValueRight" ).GetComponent<Text>();
                mJoyValueFire = GameObject.Find( "joyValueFire" ).GetComponent<Text>();
                mJoyValueHyper = GameObject.Find( "joyValueHyper" ).GetComponent<Text>();
                mJoyValuePause = GameObject.Find( "joyValuePause" ).GetComponent<Text>();
                mIsInitialized = true;
            }

            mKeyValueLeft.text = "--";
            mKeyValueRight.text = "--";
            mKeyValueFire.text = "--";
            mKeyValueHyper.text = "--";
            mKeyValuePause.text = "--";

            mJoyValueLeft.text = "--";
            mJoyValueRight.text = "--";
            mJoyValueFire.text = "--";
            mJoyValueHyper.text = "--";
            mJoyValuePause.text = "--";
        }

        public override void OnStateExit( StateMachineMB.State from )
        {
            base.OnStateExit( from );
        }

        public override void OnStateMessageReceived( object id )
        {
            startEditMode( (InputIDType)id );
        }

        private void startEditMode( InputIDType id )
        {
            Debug.Log( "Starting edit mode: " + id );
            mEditMode = true;
            switch( id ) {
                case InputIDType.LEFT:
                    mEditKeyString = mKeyValueLeft;
                    mEditJoyString = mJoyValueLeft;
                    break;
                case InputIDType.RIGHT:
                    mEditKeyString = mKeyValueRight;
                    mEditJoyString = mJoyValueRight;
                    break;
                case InputIDType.FIRE:
                    mEditKeyString = mKeyValueFire;
                    mEditJoyString = mJoyValueFire;
                    break;
                case InputIDType.HYPER:
                    mEditKeyString = mKeyValueHyper;
                    mEditJoyString = mJoyValueHyper;
                    break;
                case InputIDType.PAUSE:
                    mEditKeyString = mKeyValuePause;
                    mEditJoyString = mJoyValuePause;
                    break;

            }
        }

        private void endEditMode()
        {
            Debug.Log( "Ending edit mode" );
            mEditMode = false;
        }

        private void PollInputs()
        {
            if( Input.anyKey ) {
                foreach( KeyCode kcode in Enum.GetValues( typeof( KeyCode ) ) ) {
                    if( Input.GetKeyDown( kcode ) ) {
                        //Debug.Log( "Key pressed: " + kcode );
                        mEditKeyString.text = kcode.ToString();
                        endEditMode();
                        return;
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            if( mEditMode ) {
                PollInputs();
            }
        }
    }
}
