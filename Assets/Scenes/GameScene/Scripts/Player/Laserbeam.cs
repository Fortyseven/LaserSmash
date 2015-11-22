using System;
using UnityEngine;

namespace Game
{
    public class Laserbeam : MonoBehaviour
    {
        const float SPEED = 45.0f;

        public delegate void OnDoneHandlerType();

        private OnDoneHandlerType _on_done_callback = null;
        private bool _firing = false;

        public void OnTriggerEnter( Collider other )
        {
            GenericEnemy ge = other.gameObject.GetComponent<GenericEnemy>();
            if ( ge != null ) {
                ge.HitByLaser( this );
                HideBeam();
            }
        }

        public void Init( OnDoneHandlerType handler )
        {
            _on_done_callback = handler;
            HideBeam();
        }

        public void Fire( Vector3 newpos )
        {
            transform.position = newpos;
            ShowBeam();
        }
        private void ShowBeam()
        {
            _firing = true;
            gameObject.SetActive( true );
        }

        private void HideBeam()
        {
            _firing = false;
            _on_done_callback();
            gameObject.SetActive( false );
        }

        public void Update()
        {
            if ( !_firing ) return;

            transform.Translate( new Vector3( 0.0f, SPEED * Time.deltaTime, 0.0f ), transform );

            if ( transform.position.y > GameConstants.SCREEN_Y_GEN_OFFSET ) {
                HideBeam();
            }

            //Destroy( gameObject );
        }
    }
}