/************************************************************************
** Flash.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/


using UnityEngine;
using UnityEngine.UI;

namespace Game
{

    public class Flash : MonoBehaviour
    {
        public float FlashDelayInSeconds = 1.0f;
        public bool StartEnabled = true;

        private Text _renderer;
        private float _timeout;
        //    float _initial_alpha;
        private bool _is_on;
        private bool _paused;

        private void Start()
        {
            _renderer = GetComponent<Text>();
            _timeout = Time.time + FlashDelayInSeconds;
            //        _initial_alpha = _renderer.GetAlpha();

            ResetFlashers();
        }

        private void Update()
        {
            if ( _paused )
                return;

            if ( Time.time >= _timeout ) {
                _is_on = !_is_on;
                _renderer.enabled = _is_on;
                _timeout = Time.time + FlashDelayInSeconds;
            }
        }

        public void Go()
        {
            _paused = false;
            _is_on = true;
            _renderer.enabled = true;
            _timeout = Time.time + FlashDelayInSeconds;
        }

        public void ResetFlashers()
        {
            _is_on = StartEnabled;
            _paused = false;

            if ( !_is_on ) {
                _renderer.enabled = false;
                _paused = true;
            }
        }
    }
}
