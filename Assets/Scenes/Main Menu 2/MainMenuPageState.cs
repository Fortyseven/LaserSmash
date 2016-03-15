using UnityEngine;
using System.Collections;
using System;

namespace Game
{
    public abstract class MainMenuPageState : StateMachineMB.State
    {
        public bool InTransit { get; private set; }

        private Canvas _canvas;

        private const float FADE_SPEED = 2f;

        private IEnumerator _fade_in_thread, _fade_out_thread;

        /*************************************/
        public MainMenuPageState( Canvas canvas ) : base()
        {
            _canvas = canvas;
        }

        /*************************************/
        public override void OnStateEnter( StateMachineMB.State from )
        {
            if ( InTransit ) {
                OwnerMB.StopCoroutine( _fade_out_thread );
                OwnerMB.StopCoroutine( _fade_in_thread );
                InTransit = false;
            }
            _fade_in_thread = FadeIn_Coroutine();
            OwnerMB.StartCoroutine( _fade_in_thread );
        }

        /*************************************/
        public override void OnStateExit( StateMachineMB.State to )
        {
            _fade_out_thread = FadeOut_Coroutine();
            OwnerMB.StartCoroutine( _fade_out_thread );
        }

        /*************************************/
        public IEnumerator FadeIn_Coroutine()
        {
            InTransit = true;

            _canvas.gameObject.SetActive( true );

            CanvasRenderer[] _rends = _canvas.GetComponentsInChildren<CanvasRenderer>();

            float a = 0;

            while ( a < 1.0f ) {
                a += Time.deltaTime * FADE_SPEED;
                for ( int k = 0; k < _rends.Length; k++ ) {
                    _rends[ k ].SetAlpha( a );
                }
                yield return null;
            }

            // Ensure they're fully visible 
            for ( int k = 0; k < _rends.Length; k++ ) {
                _rends[ k ].SetAlpha( 1.0f );
                yield return null;
            }

            InTransit = false;
        }

        /*************************************/
        public IEnumerator FadeOut_Coroutine()
        {
            CanvasRenderer[] _rends = _canvas.GetComponentsInChildren<CanvasRenderer>();

            InTransit = true;

            float a = 1.0f;
            while ( a > 0.0f ) {
                a -= Time.deltaTime * FADE_SPEED;
                for ( int k = 0; k < _rends.Length; k++ ) {
                    _rends[ k ].SetAlpha( a );
                }
                yield return null;
            }

            // Ensure they're fully invisible 
            for ( int k = 0; k < _rends.Length; k++ ) {
                _rends[ k ].SetAlpha( 0.0f );
                yield return null;
            }

            _canvas.gameObject.SetActive( false );
            InTransit = false;
        }
    }
}