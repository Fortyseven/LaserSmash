using System;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using Random = UnityEngine.Random;
using State = Game.StateMachineMB.State;

namespace Game
{
    public class UFO_State_ATTACKING : State
    {
        private const float CHARGING_SPEED = 3.0f;
        private float _time_started_charging;
        private Vector3 _player_target_position;
        private bool _has_fired;

        public override Enum Name { get { return UFO.UFOState.ATTACKING; } }

        public override void OnStateEnter( State from_state )
        {
            ( (UFO)Owner ).Speed = CHARGING_SPEED;

            ( (UFO)Owner )._charging_flare_sprite.enabled = true;
            ( (UFO)Owner )._charging_light.enabled = true;
            ( (UFO)Owner )._audio.pitch = UFO.CHARGING_PITCH;

            _time_started_charging = Time.time;
            _has_fired = false;

#if DEVELOPMENT_BUILD
            if ( CHARGING_TIME < TARGET_LOCK_TIME ) {
                throw new UnityException( "TARGET_LOCK_TIME must be less than CHARGING_TIME" );
            }
#endif
            // Immediately acquire target lock, leading to firing
            OwnerMB.StartCoroutine( AcquireTargetLock() );
        }

        public override void OnStateExit( State to_state )
        {
            OwnerMB.StopCoroutine( AcquireTargetLock() );
            OwnerMB.StopCoroutine( Fire() );
            ( (UFO)Owner ).ShutDownLaser();
            _has_fired = false;
        }

        public override void OnUpdate()
        {
            // Laser SOURCE should always follow ship's slow movement, otherwise beam will freeze in space
            ( (UFO)Owner )._laser.SetPosition( 1, OwnerMB.transform.position );

            if ( _has_fired ) return;

            if ( ( Time.time - _time_started_charging ) > UFO.CHARGING_TIME ) {
                OwnerMB.StartCoroutine( Fire() );
            }
        }

        /// <summary>
        /// Wait for TARGET_LOCK_TIME seconds and then log where the player was; fire at that position.
        /// This is used so that there's a chance for the player to dodge the shot.
        /// </summary>
        [UsedImplicitly]
        public IEnumerator AcquireTargetLock()
        {
            yield return new WaitForSeconds( UFO.TARGET_LOCK_TIME );
            _player_target_position = GameController.instance.GameEnv.PlayerShip.transform.position;
        }

        /// <summary>
        /// I'm-a-firin' muh laz0r.
        /// </summary>
        [UsedImplicitly]
        public IEnumerator Fire()
        {
            _has_fired = true;

            ( (UFO)Owner )._laser.SetPosition( 1, OwnerMB.transform.position );
            ( (UFO)Owner )._laser.SetPosition( 0, _player_target_position );

            ( (UFO)Owner )._laser.gameObject.SetActive( true );


            UnityEngine.Object.Instantiate( ( (UFO)Owner ).ExplosionLaserGroundPrefab, _player_target_position, Quaternion.identity );

            // Check for collision
            var r = new Ray(OwnerMB.transform.position, (_player_target_position - OwnerMB.transform.position) * 2);


            //Debug.DrawRay( OwnerMB.transform.position, ( _player_target_position - OwnerMB.transform.position ) * 2 );

            /* We COULD check if this is the player being hit, but all the enemies are on layer 8, and
               nothing else with a collider exists on any other layer but the player. */


            // UFOs can be flying about during a Game Over state, but we only care about killing
            // the player when we're actually in a game
            if ( GameController.instance.CurrentState == GameController.GameState.RUNNING ) {
                if ( Physics.Raycast( r.origin, r.direction, Mathf.Infinity, 1 << 12 ) ) {
                    // Manually calling this so we don't vanish immediately after killing player
                    // UFOs should be seen leaving the scene, like the badass motherfucker it is.
                    GameController.instance.KillPlayer();
                }
            }

            Debug.Log( "Beginning fade" );

            // Fade out the beam over LASER_FADE_TIME seconds
            Color col = ((UFO) Owner)._laser.material.GetColor("_TintColor");

            float timer = UFO.LASER_FADE_TIME;

            while ( timer > 0 ) {
                timer -= Time.deltaTime;

                //alpha -= ( 1.0f / LASER_FADE_TIME ) * Time.deltaTime;
                col.a = ( ( 1.0f / UFO.LASER_FADE_TIME ) * timer );

                ( (UFO)Owner )._laser.material.SetColor( "_TintColor", col );

                yield return null;
            }

            Debug.Log( "Finished fade, leaving state" );
            Owner.ChangeState( UFO.UFOState.PASSIVE );
        }
    }
}
