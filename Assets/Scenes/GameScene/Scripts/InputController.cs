using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class InputController
    {
        const string PREFS_KEY_LEFT = "mKeyValueLeft";
        const string PREFS_KEY_RIGHT = "mKeyValueRight";
        const string PREFS_KEY_FIRE = "mKeyValueFire";
        const string PREFS_KEY_HYPER = "mKeyValueHyper";
        const string PREFS_KEY_PAUSE = "mKeyValuePause";

        const string PREFS_JOY_LEFT = "mJoyValueLeft";
        const string PREFS_JOY_RIGHT = "mJoyValueRight";
        const string PREFS_JOY_FIRE = "mJoyValueFire";
        const string PREFS_JOY_HYPER = "mJoyValueHyper";
        const string PREFS_JOY_PAUSE = "mJoyValuePause";

        public delegate void OnInputEvent( TEvent ev );
        public event OnInputEvent mEventSubscribers;

        public struct TEvent
        {
            public EventType event_type;
            public InputType input_type;
            public float axis_value;
        }

        public enum EventType
        {
            EV_UP, EV_DOWN, EV_LEFT, EV_RIGHT, EV_FIRE, EV_HYPER, EV_PAUSE,
            EV_VERT, EV_HORIZ
        };

        public enum InputType { INPUT_KEY, INPUT_JOYBUTTON, INPUT_JOYAXIS };

        private List<InputElement> mInputElements = new List<InputElement>();

        /*********************************************/
        public class InputElement
        {
            public readonly EventType EventType;
            public readonly InputType InputType;
            public readonly object InputValue;

            /* -------------------- */
            public InputElement( string input_id, InputType type, EventType event_type, object default_value )
            {
                InputValue = default_value;

                InputType = type;
                EventType = event_type;

                // Pull the key codes/string from the Player Prefs depending on the type of input

                if( InputType == InputType.INPUT_KEY ) {
                    InputValue = PlayerPrefs.GetInt( input_id, (int)default_value );
                }
                else if( InputType == InputType.INPUT_JOYBUTTON ||
                         InputType == InputType.INPUT_JOYAXIS ) {
                    InputValue = PlayerPrefs.GetString( input_id, (string)default_value );
                }
            }

            /* -------------------- */
            public bool Poll()
            {
                switch( InputType ) {
                    case InputType.INPUT_KEY:
                        return Input.GetKey( (KeyCode)InputValue );
                    case InputType.INPUT_JOYBUTTON:
                        return Input.GetKey( (string)InputValue );
                    case InputType.INPUT_JOYAXIS:
                    default:
                        return false;
                }
            }
        }

        /*********************************************/
        public void Subscribe( OnInputEvent func )
        {
            mEventSubscribers += func;
        }

        /*********************************************/
        public void Unsubscribe( OnInputEvent func )
        {
            mEventSubscribers -= func;
        }

        /*********************************************/
        public void Poll()
        {
            if( mEventSubscribers == null ) return;

            if( Input.anyKey ) {
                foreach( var input in mInputElements ) {
                    if( input.Poll() ) {
                        mEventSubscribers( new TEvent {
                            input_type = input.InputType,
                            event_type = input.EventType
                        } );
                    }
                }
            }
        }

        /*********************************************/
        public void Init()
        {
            mInputElements.Add( new InputElement( PREFS_KEY_LEFT, InputType.INPUT_KEY, EventType.EV_LEFT, KeyCode.LeftArrow ) );
            mInputElements.Add( new InputElement( PREFS_KEY_RIGHT, InputType.INPUT_KEY, EventType.EV_RIGHT, KeyCode.RightArrow ) );
            mInputElements.Add( new InputElement( PREFS_KEY_FIRE, InputType.INPUT_KEY, EventType.EV_FIRE, KeyCode.LeftControl ) );
            mInputElements.Add( new InputElement( PREFS_KEY_HYPER, InputType.INPUT_KEY, EventType.EV_HYPER, KeyCode.UpArrow ) );
            mInputElements.Add( new InputElement( PREFS_KEY_PAUSE, InputType.INPUT_KEY, EventType.EV_PAUSE, KeyCode.Escape ) );

            mInputElements.Add( new InputElement( PREFS_JOY_LEFT, InputType.INPUT_JOYBUTTON, EventType.EV_LEFT, null ) );
            mInputElements.Add( new InputElement( PREFS_JOY_RIGHT, InputType.INPUT_JOYBUTTON, EventType.EV_RIGHT, null ) );
            mInputElements.Add( new InputElement( PREFS_JOY_FIRE, InputType.INPUT_JOYBUTTON, EventType.EV_FIRE, "joystick 1 button 1" ) );
            mInputElements.Add( new InputElement( PREFS_JOY_HYPER, InputType.INPUT_JOYBUTTON, EventType.EV_HYPER, "joystick 1 button 2" ) );
            mInputElements.Add( new InputElement( PREFS_JOY_PAUSE, InputType.INPUT_JOYBUTTON, EventType.EV_PAUSE, "joystick 1 button 9" ) );
        }
    }
}
