using UnityEngine;
using System.Collections;
using System;

namespace Game
{
    public abstract class MainMenuPageState : StateMachineMB.State
    {
        public bool InTransit { get { return _is_fading; } }

        private Canvas _canvas;

        private bool _is_fading;

        private const float FADE_SPEED = 2f;

        /*************************************/
        public MainMenuPageState( Canvas canvas ) : base()
        {
            Debug.Log( "Assigning canvas " + canvas.name );
            _canvas = canvas;
        }

        /*************************************/
        public override void OnStateEnter( StateMachineMB.State from )
        {
            Debug.Log( "State entered" );
            OwnerMB.StartCoroutine( FadeIn_Coroutine() );
        }

        /*************************************/
        public override void OnStateExit( StateMachineMB.State to )
        {
            Debug.Log( "State exiting" );
            OwnerMB.StartCoroutine( FadeOut_Coroutine() );
        }

        /*************************************/
        public IEnumerator FadeIn_Coroutine()
        {
            _is_fading = true;

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

            _is_fading = false;
        }

        /*************************************/
        public IEnumerator FadeOut_Coroutine()
        {
            CanvasRenderer[] _rends = _canvas.GetComponentsInChildren<CanvasRenderer>();

            _is_fading = true;

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
            _is_fading = false;
        }
    }
}