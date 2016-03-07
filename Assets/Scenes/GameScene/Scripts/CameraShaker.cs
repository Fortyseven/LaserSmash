using UnityEngine;
using System.Collections;
using System;

namespace Game
{
    public class CameraShaker : MonoBehaviour
    {
        private const float MAGNITUDE = 1.0f;

        private Vector3 _offs;
        private float _shake_timer;
        private float _shake_duration;
        private float _shake_magnitude;

        void Start()
        {
            _offs = Vector3.zero;
            _shake_timer = 0.0f;
        }

        void Update()
        {
            _shake_timer -= Time.deltaTime;

            if ( _shake_timer <= 0 ) {
                _offs = Vector3.zero;
                return;
            }

            _offs.x = Mathf.Sin( _shake_timer * 150 ) * _shake_magnitude * ( _shake_timer / _shake_duration );
            _offs.y = Mathf.Cos( _shake_timer * 150 ) * _shake_magnitude * ( _shake_timer / _shake_duration );
            _offs.z = Mathf.Cos( _shake_timer * 150 ) * _shake_magnitude * ( _shake_timer / _shake_duration );
        }

        public void SHAKE( float duration, float magnitude = MAGNITUDE )
        {            
            _shake_duration = duration;
            _shake_timer = _shake_duration;
            _shake_magnitude = magnitude;
            //StartCoroutine( doSleep() );
        }

        private IEnumerator doSleep()
        {
            float delay = 0.05f;
            Time.timeScale = 0;

            while ( delay > 0 ) {
                delay -= Time.unscaledDeltaTime;
                yield return null;
            }

            Time.timeScale = 1.0f;
        }

        public Vector3 GetOffset()
        {
            return _offs;
        }

    }
}