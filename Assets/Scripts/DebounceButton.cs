using UnityEngine;

namespace Game
{
    public class DebounceButton
    {
        private bool _start_state;
        private string _button_name;

        public DebounceButton( string button_name )
        {
            _button_name = button_name;
            this.reset();
        }

        public void reset()
        {
            _start_state = true;
        }

        public bool isPressed()
        {
            var cur_state = Input.GetButton(_button_name);

            if( _start_state && cur_state ) {
                return false;
            }
            _start_state = false;
            return cur_state;
        }
    }
}
