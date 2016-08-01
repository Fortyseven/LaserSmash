using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class InputController : MonoBehaviour
    {
        private static InputController _instance = null;

        public static bool IsInitialized = false;
        public static bool DebugMode { get; private set; }

        public enum InputEnum { Left, Right, Hyperspace, AutoFire, Fire, Menu };

        public class InputDef
        {
            public string name;
            public InputEnum input_enum;
            public bool is_down = false;
            public List<KeyCode> code;
        }

        public static InputDef Left = new InputDef {
            name = "Left",
            input_enum = InputEnum.Left,
            code = new List<KeyCode>{ KeyCode.A, KeyCode.LeftArrow }
        };
        public static InputDef Right = new InputDef {
            name = "Right",
            input_enum = InputEnum.Right,
            code = new List<KeyCode>{ KeyCode.D, KeyCode.RightArrow }
        };
        public static InputDef Hyperspace = new InputDef {
            name = "Hyperspace",
            input_enum = InputEnum.Hyperspace,
            code = new List<KeyCode>{ KeyCode.W, KeyCode.UpArrow }
        };
        public static InputDef AutoFireToggle = new InputDef {
            name = "Auto-Fire Toggle",
            input_enum = InputEnum.AutoFire,
            code = new List<KeyCode>{ KeyCode.S, KeyCode.DownArrow }
        };
        public static InputDef Fire = new InputDef {
            name = "Fire",
            input_enum = InputEnum.Fire,
            code = new List<KeyCode>{ KeyCode.Space, KeyCode.LeftControl, KeyCode.RightControl }
        };
        public static InputDef Menu = new InputDef {
            name = "Menu",
            input_enum = InputEnum.Menu,
            code = new List<KeyCode>{ KeyCode.Escape }
        };

        public delegate void OnInputCB( InputDef input );
        public static event OnInputCB OnInput;

        private InputDef[] inputs = { Left, Right, Hyperspace, AutoFireToggle, Fire, Menu };

        void Start()
        {
            _instance = this;
            // Restore PlayerPrefs
        }

        public static void Construct( bool debug_mode = false )
        {
            if ( _instance != null ) return;

            DebugMode = debug_mode;
            var go = new GameObject("Init");
            go.AddComponent<InputController>();
        }

        void Update()
        {
            if ( Input.anyKey ) {
                foreach ( var input in inputs ) {
                    bool start_state = input.is_down;
                    input.is_down = false;
                    foreach ( var code in input.code ) {
                        input.is_down = Input.GetKeyDown( code );
                        if ( input.is_down ) break;
                    } // each keycode
                    if ( start_state != input.is_down && input.is_down ) {
                        if ( OnInput != null ) {
                            OnInput( input );
                        }
                    }
                }// each input
            }
        }
    }
}