using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /*************************************************************************/
    class Menu_Controls : MainMenuPageState
    {
        public override Enum Name { get { return MainMenu2.PageStates.PAGE_CONTROLS; } }

        public class TextObjectPair
        {
            public Text textKeyValue;
            public Text textJoyValue;
            public KeyCode key_value;
            public string joy_value;
        }

        private TextObjectPair mInputLeft;
        private TextObjectPair mInputRight;
        private TextObjectPair mInputFire;
        private TextObjectPair mInputHyper;
        private TextObjectPair mInputPause;

        private bool mIsInitialized = false;

        public enum InputIDType { LEFT = 0, RIGHT = 1, FIRE = 2, HYPER = 3, PAUSE = 4 };

        public bool mEditMode = false;
        public TextObjectPair mEditInput = null;

        public Menu_Controls( Canvas canvas ) : base( canvas ) {; }

        public override void OnStateEnter( StateMachineMB.State from )
        {
            base.OnStateEnter( from );

            if( !mIsInitialized ) {
                mInputLeft = new TextObjectPair {
                    textKeyValue = GameObject.Find( "keyValueLeft" ).GetComponent<Text>(),
                    textJoyValue = GameObject.Find( "joyValueLeft" ).GetComponent<Text>(),
                    key_value = KeyCode.LeftArrow,
                    joy_value = ""
                };
                mInputRight = new TextObjectPair {
                    textKeyValue = GameObject.Find( "keyValueRight" ).GetComponent<Text>(),
                    textJoyValue = GameObject.Find( "joyValueRight" ).GetComponent<Text>(),
                    key_value = KeyCode.RightArrow,
                    joy_value = ""
                };
                mInputFire = new TextObjectPair {
                    textKeyValue = GameObject.Find( "keyValueFire" ).GetComponent<Text>(),
                    textJoyValue = GameObject.Find( "joyValueFire" ).GetComponent<Text>(),
                    key_value = KeyCode.Space,
                    joy_value = ""
                };
                mInputHyper = new TextObjectPair {
                    textKeyValue = GameObject.Find( "keyValueHyper" ).GetComponent<Text>(),
                    textJoyValue = GameObject.Find( "joyValueHyper" ).GetComponent<Text>(),
                    key_value = KeyCode.UpArrow,
                    joy_value = ""
                };
                mInputPause = new TextObjectPair {
                    textKeyValue = GameObject.Find( "keyValuePause" ).GetComponent<Text>(),
                    textJoyValue = GameObject.Find( "joyValuePause" ).GetComponent<Text>(),
                    key_value = KeyCode.P,
                    joy_value = ""
                };

                mIsInitialized = true;
            }

            loadSavedInputs();
            populateInputLabels();
        }

        private void populateInputLabels()
        {
            mInputLeft.textKeyValue.text = ( (KeyCode)mInputLeft.key_value ).ToString();
            mInputRight.textKeyValue.text = ( (KeyCode)mInputRight.key_value ).ToString();
            mInputFire.textKeyValue.text = ( (KeyCode)mInputFire.key_value ).ToString();
            mInputHyper.textKeyValue.text = ( (KeyCode)mInputHyper.key_value ).ToString();
            mInputPause.textKeyValue.text = ( (KeyCode)mInputPause.key_value ).ToString();

            mInputLeft.textJoyValue.text = mInputLeft.joy_value.ToString();
            mInputRight.textJoyValue.text = mInputRight.joy_value.ToString();
            mInputFire.textJoyValue.text = mInputFire.joy_value.ToString();
            mInputHyper.textJoyValue.text = mInputHyper.joy_value.ToString();
            mInputPause.textJoyValue.text = mInputPause.joy_value.ToString();
        }

        private void loadSavedInputs()
        {
            mInputLeft.key_value = (KeyCode)PlayerPrefs.GetInt( "mKeyValueLeft", (int)mInputLeft.key_value );
            mInputRight.key_value = (KeyCode)PlayerPrefs.GetInt( "mKeyValueRight", (int)mInputRight.key_value );
            mInputFire.key_value = (KeyCode)PlayerPrefs.GetInt( "mKeyValueFire", (int)mInputFire.key_value );
            mInputHyper.key_value = (KeyCode)PlayerPrefs.GetInt( "mKeyValueHyper", (int)mInputHyper.key_value );
            mInputPause.key_value = (KeyCode)PlayerPrefs.GetInt( "mKeyValuePause", (int)mInputPause.key_value );

            mInputLeft.joy_value = PlayerPrefs.GetString( "mJoyValueLeft", mInputLeft.joy_value );
            mInputRight.joy_value = PlayerPrefs.GetString( "mJoyValueRight", mInputRight.joy_value );
            mInputFire.joy_value = PlayerPrefs.GetString( "mJoyValueFire", mInputFire.joy_value );
            mInputHyper.joy_value = PlayerPrefs.GetString( "mJoyValueHyper", mInputHyper.joy_value );
            mInputPause.joy_value = PlayerPrefs.GetString( "mJoyValuePause", mInputPause.joy_value );
        }

        private void saveInputs()
        {
            PlayerPrefs.SetInt( "mKeyValueLeft", (int)mInputLeft.key_value );
            PlayerPrefs.SetInt( "mKeyValueRight", (int)mInputRight.key_value );
            PlayerPrefs.SetInt( "mKeyValueFire", (int)mInputFire.key_value );
            PlayerPrefs.SetInt( "mKeyValueHyper", (int)mInputHyper.key_value );
            PlayerPrefs.SetInt( "mKeyValuePause", (int)mInputPause.key_value );

            PlayerPrefs.SetString( "mJoyValueLeft", mInputLeft.joy_value );
            PlayerPrefs.SetString( "mJoyValueRight", mInputRight.joy_value );
            PlayerPrefs.SetString( "mJoyValueFire", mInputFire.joy_value );
            PlayerPrefs.SetString( "mJoyValueHyper", mInputHyper.joy_value );
            PlayerPrefs.SetString( "mJoyValuePause", mInputPause.joy_value );

            PlayerPrefs.Save();
            Debug.Log( "Saved." );
        }

        public override void OnStateExit( StateMachineMB.State from )
        {
            base.OnStateExit( from );
        }

        public override void OnStateMessageReceived( object id )
        {
            if( id is String ) {
                if( (String)id == "apply" ) {
                    saveInputs();
                    ( OwnerMB as MainMenu2 ).ChangePage( MainMenu2.PageStates.PAGE_MAIN );
                }
            }
            else {
                startEditMode( (InputIDType)id );
            }
        }

        private void startEditMode( InputIDType id )
        {
            Debug.Log( "Starting edit mode: " + id );
            mEditMode = true;
            switch( id ) {
                case InputIDType.LEFT:
                    mEditInput = mInputLeft;
                    break;
                case InputIDType.RIGHT:
                    mEditInput = mInputRight;
                    break;
                case InputIDType.FIRE:
                    mEditInput = mInputFire;
                    break;
                case InputIDType.HYPER:
                    mEditInput = mInputHyper;
                    break;
                case InputIDType.PAUSE:
                    mEditInput = mInputPause;
                    break;
            }
        }

        private void endEditMode()
        {
            Debug.Log( "Ending edit mode" );
            mEditMode = false;
        }

        private void pollInputs()
        {
            if( Input.anyKey ) {
                if( Input.GetKeyDown( KeyCode.Escape ) ) {
                    endEditMode();
                    return;
                }

                // Poll sticks (seriously)
                for( int stick = 1; stick < 8; stick++ ) {
                    for( int butt = 0; butt < 20; butt++ ) {
                        var joystring = "joystick " + stick + " button " + butt;
                        if( Input.GetKeyDown( joystring ) ) {
                            mEditInput.textJoyValue.text = joystring;
                            mEditInput.joy_value = joystring;
                            endEditMode();
                            return;
                        }
                    }
                }

                foreach( KeyCode kcode in Enum.GetValues( typeof( KeyCode ) ) ) {
                    if( Input.GetKeyDown( kcode ) ) {
                        //Debug.Log( "Key pressed: " + kcode );
                        mEditInput.textKeyValue.text = kcode.ToString();
                        mEditInput.key_value = kcode;
                        endEditMode();
                        return;
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            if( mEditMode ) {
                pollInputs();
            }
        }

        public void OnClickApply()
        {
            saveInputs();
        }
    }
}
