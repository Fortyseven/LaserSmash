using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class InputController
    {

        public enum EventType
        {
            EV_LEFT, EV_RIGHT, EV_FIRE, EV_HYPER, EV_PAUSE
        };

        public class InputPair
        {
            public KeyCode key_value;
            public string joy_value;
        }

        InputPair mInputLeft = new InputPair();
        InputPair mInputRight = new InputPair();
        InputPair mInputFire = new InputPair();
        InputPair mInputHyper = new InputPair();
        InputPair mInputPause = new InputPair();

        public delegate void OnInputEvent( EventType ev );
        public event OnInputEvent mEventSubscribers;

        public void Subscribe( OnInputEvent func )
        {
            mEventSubscribers += func;
        }

        public void Unsubscribe( OnInputEvent func )
        {
            mEventSubscribers -= func;
        }

        public void Poll()
        {
            if( mEventSubscribers == null ) return;

            if( Input.anyKey ) {
                if( Input.GetKeyDown( mInputLeft.key_value ) ||
                    Input.GetKeyDown( mInputLeft.joy_value ) ) {
                    mEventSubscribers( EventType.EV_LEFT );
                }
                if( Input.GetKeyDown( mInputRight.key_value ) ||
                    Input.GetKeyDown( mInputRight.joy_value ) ) {
                    mEventSubscribers( EventType.EV_RIGHT );
                }
                if( Input.GetKeyDown( mInputFire.key_value ) ||
                    Input.GetKeyDown( mInputFire.joy_value ) ) {
                    mEventSubscribers( EventType.EV_FIRE );
                }
                if( Input.GetKeyDown( mInputHyper.key_value ) ||
                    Input.GetKeyDown( mInputHyper.joy_value ) ) {
                    mEventSubscribers( EventType.EV_HYPER );
                }
                if( Input.GetKeyDown( mInputPause.key_value ) ||
                    Input.GetKeyDown( mInputPause.joy_value ) ) {
                    mEventSubscribers( EventType.EV_PAUSE );
                }

            }
        }

        public void Init()
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
    }
}
