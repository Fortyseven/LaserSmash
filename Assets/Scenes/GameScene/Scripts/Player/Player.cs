//#define TESTMODE

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class Player : StateMachineMB
    {
        private const float SHIP_SPEED = 13.0f;
        //private const float SHIP_X_BOUNDS = 13.0f;
        private const float LASER_Y_OFFSET_FROM_SHIP = 2.0f;
        private const float FIRE_DELAY = 0.33f;

        public GameObject LaserbeamPrefab = null;
        public GameObject DeathExplosionPrefab = null;

        private GameObject _ship_mesh;

        public enum PlayerState { NORMAL, RESET, KILLED, DISABLED };

        /***********************************************************************************/
        private class PlayerState_NORMAL : State
        {
            public override Enum Name { get { return PlayerState.NORMAL; } }

            private float _next_fire_time;

            public override void OnStateEnter( State from_state )
            {
                _next_fire_time = Time.time + FIRE_DELAY;
            }

            public override void OnUpdate()
            {
                Vector3 pos = OwnerMB.transform.position;

                pos.x += Input.GetAxis( "Horizontal" ) * SHIP_SPEED * Time.deltaTime;
                pos.x = Mathf.Clamp( pos.x, -GameConstants.SCREEN_X_BOUNDS, GameConstants.SCREEN_X_BOUNDS );

                if ( Input.GetButton( "Fire" ) ) {
                    Fire();
                }

                OwnerMB.transform.position = pos;

                if ( Input.GetButtonDown( "Hyperspace" ) ) {
                    Hyperspace();
                }
            }

            /**************************************/
            private void Fire()
            {
                if ( Time.time >= _next_fire_time ) {
                    SpawnLaserbeam();
                    _next_fire_time = Time.time + FIRE_DELAY;
                }
            }

            /**************************************/
            private void Hyperspace()
            {
                // TODO: beaming animation, from and to, sfx
                Vector3 pos = OwnerMB.transform.position;
                pos.x = Random.Range( -GameConstants.SCREEN_X_BOUNDS, GameConstants.SCREEN_X_BOUNDS );
                OwnerMB.transform.position = pos;
            }

            /**************************************/
            private void SpawnLaserbeam()
            {
                Vector3 newpos = Owner.transform.position;
                newpos.y += LASER_Y_OFFSET_FROM_SHIP;
                Instantiate( ( (Player)Owner ).LaserbeamPrefab, newpos, Quaternion.identity );
            }
        }

        /***********************************************************************************/
        private class PlayerState_RESET : State
        {
            private Vector3 _starting_position;

            public override Enum Name { get { return PlayerState.RESET; } }

            public override void Start()
            {
                _starting_position = OwnerMB.transform.position;
            }

            public override void OnStateEnter( State from_state )
            {
#if !TESTMODE
                OwnerMB.transform.position = _starting_position;
#endif
                ( (Player)Owner )._ship_mesh.SetActive( true );

                Owner.ChangeState( PlayerState.NORMAL );
            }

            public override void OnUpdate()
            {
                ;
            }
        }

        /***********************************************************************************/
        private class PlayerState_DISABLED : State
        {
            public override Enum Name { get { return PlayerState.DISABLED; } }

            public override void OnStateEnter( State from_state )
            {
                ( (Player)Owner )._ship_mesh.SetActive( false );
            }

            public override void OnStateExit( State to_state )
            {
                ( (Player)Owner )._ship_mesh.SetActive( true );

            }

            public override void OnUpdate()
            {
                ;
            }
        }

        /***********************************************************************************/
        private class PlayerState_KILLED : State
        {
            public override Enum Name
            {
                get { return PlayerState.KILLED; }
            }

            private float _timeout_timer;

            public override void OnStateEnter( State from_state )
            {
#if TESTMODE
        return;
#endif
                ( (Player)Owner )._ship_mesh.SetActive( false );

                Destroy(
                    Instantiate( ( (Player)Owner ).DeathExplosionPrefab, OwnerMB.transform.position, Quaternion.identity ),
                    3.0f );

                GameController.instance.ChangeState( GameController.GameState.PLAYER_DYING );

                _timeout_timer = 3.0f;
            }

            public override void OnUpdate()
            {
                _timeout_timer -= Time.deltaTime;

                if ( _timeout_timer <= 0 ) {
                    // Will kick off resumption of game or GameOver, and possible reset us
                    GameController.instance.CurrentState.SendMessage();
                }
            }
        }

        /**************************************/
        public void Start()
        {
            DebugMode = true;

            _ship_mesh = transform.Find( "Mesh" ).gameObject;

            AddState( new PlayerState_NORMAL() );
            AddState( new PlayerState_RESET() );
            AddState( new PlayerState_DISABLED() );
            AddState( new PlayerState_KILLED() );

            // Just sit pretty until GameController changes us to Reset
        }
    }
}