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
            public Text textValue;
            public KeyCode value;
        }

        private TextObjectPair mKeyLeft;
        private TextObjectPair mKeyRight;
        private TextObjectPair mKeyFire;
        private TextObjectPair mKeyHyper;
        private TextObjectPair mKeyPause;

        private TextObjectPair mJoyLeft;
        private TextObjectPair mJoyRight;
        private TextObjectPair mJoyFire;
        private TextObjectPair mJoyHyper;
        private TextObjectPair mJoyPause;

        private bool mIsInitialized = false;

        public enum InputIDType { LEFT = 0, RIGHT = 1, FIRE = 2, HYPER = 3, PAUSE = 4 };

        public bool mEditMode = false;
        public TextObjectPair mEditInput = null;

        public Menu_Controls( Canvas canvas ) : base( canvas ) {; }

        public override void OnStateEnter( StateMachineMB.State from )
        {
            base.OnStateEnter( from );

            if( !mIsInitialized ) {
                mKeyLeft = new TextObjectPair {
                    textValue = GameObject.Find( "keyValueLeft" ).GetComponent<Text>(),
                    value = KeyCode.LeftArrow
                };
                mKeyRight = new TextObjectPair {
                    textValue = GameObject.Find( "keyValueRight" ).GetComponent<Text>(),
                    value = KeyCode.RightArrow
                };
                mKeyFire = new TextObjectPair {
                    textValue = GameObject.Find( "keyValueFire" ).GetComponent<Text>(),
                    value = KeyCode.Space
                };
                mKeyHyper = new TextObjectPair {
                    textValue = GameObject.Find( "keyValueHyper" ).GetComponent<Text>(),
                    value = KeyCode.UpArrow
                };
                mKeyPause = new TextObjectPair {
                    textValue = GameObject.Find( "keyValuePause" ).GetComponent<Text>(),
                    value = KeyCode.P
                };

                mJoyLeft = new TextObjectPair {
                    textValue = GameObject.Find( "joyValueLeft" ).GetComponent<Text>()
                    //value = KeyCode.LeftArrow
                };
                mJoyRight = new TextObjectPair {
                    textValue = GameObject.Find( "joyValueRight" ).GetComponent<Text>()
                    //value = KeyCode.LeftArrow
                };
                mJoyFire = new TextObjectPair {
                    textValue = GameObject.Find( "joyValueFire" ).GetComponent<Text>()
                    //value = KeyCode.LeftArrow
                };
                mJoyHyper = new TextObjectPair {
                    textValue = GameObject.Find( "joyValueHyper" ).GetComponent<Text>()
                    //value = KeyCode.LeftArrow
                };
                mJoyPause = new TextObjectPair {
                    textValue = GameObject.Find( "joyValuePause" ).GetComponent<Text>()
                    //value = KeyCode.LeftArrow
                };

                mIsInitialized = true;
            }

            loadSavedInputs();
            populateInputLabels();
        }

        private void populateInputLabels()
        {
            mKeyLeft.textValue.text = ( (KeyCode)mKeyLeft.value ).ToString();
            mKeyRight.textValue.text = ( (KeyCode)mKeyRight.value ).ToString();
            mKeyFire.textValue.text = ( (KeyCode)mKeyFire.value ).ToString();
            mKeyHyper.textValue.text = ( (KeyCode)mKeyHyper.value ).ToString();
            mKeyPause.textValue.text = ( (KeyCode)mKeyPause.value ).ToString();
        }

        private void loadSavedInputs()
        {
            mKeyLeft.value = (KeyCode)PlayerPrefs.GetInt( "mKeyValueLeft", (int)mKeyLeft.value );
            mKeyRight.value = (KeyCode)PlayerPrefs.GetInt( "mKeyValueRight", (int)mKeyRight.value );
            mKeyFire.value = (KeyCode)PlayerPrefs.GetInt( "mKeyValueFire", (int)mKeyFire.value );
            mKeyHyper.value = (KeyCode)PlayerPrefs.GetInt( "mKeyValueHyper", (int)mKeyHyper.value );
            mKeyPause.value = (KeyCode)PlayerPrefs.GetInt( "mKeyValuePause", (int)mKeyPause.value );
        }

        private void saveInputs()
        {
            PlayerPrefs.SetInt( "mKeyValueLeft", (int)mKeyLeft.value );
            PlayerPrefs.SetInt( "mKeyValueRight", (int)mKeyRight.value );
            PlayerPrefs.SetInt( "mKeyValueFire", (int)mKeyFire.value );
            PlayerPrefs.SetInt( "mKeyValueHyper", (int)mKeyHyper.value );
            PlayerPrefs.SetInt( "mKeyValuePause", (int)mKeyPause.value );
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
                if ((String)id == "apply") {
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
                    mEditInput = mKeyLeft;
                    break;
                case InputIDType.RIGHT:
                    mEditInput = mKeyRight;
                    break;
                case InputIDType.FIRE:
                    mEditInput = mKeyFire;
                    break;
                case InputIDType.HYPER:
                    mEditInput = mKeyHyper;
                    break;
                case InputIDType.PAUSE:
                    mEditInput = mKeyPause;
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
                foreach( KeyCode kcode in Enum.GetValues( typeof( KeyCode ) ) ) {
                    if( Input.GetKeyDown( kcode ) ) {
                        //Debug.Log( "Key pressed: " + kcode );
                        mEditInput.textValue.text = kcode.ToString();
                        mEditInput.value = kcode;
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
