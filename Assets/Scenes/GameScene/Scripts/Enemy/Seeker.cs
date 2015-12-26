using System;
using UnityEngine;

namespace Game
{
    public class Seeker : GenericEnemy
    {
        protected override float SpawnMaxX { get { return 12.0f; } }
        protected override float SpawnMinX { get { return -12.0f; } }

        protected override int BaseScore { get { return GameConstants.SCORE_KILLSAT; } }

        private const int RELENTLESS_CHANCE = 50;
        private const float SURFACE_Y = 0.2f;
        private const float SPEED = 4.0f;

        private bool _killme_flag = false;

        /*******************************************************/

        private enum SeekerState
        {
            TRACKING,
            TRACKING_RELENTLESS,
            LOCKED
        };

        /*******************************************************/
        class SeekerState_TRACKING : State
        {
            public override Enum Name { get { return SeekerState.TRACKING; } }

            public override void OnStateEnter( State from_state )
            {
                OwnerMB.transform.LookAt( Vector3.down );
            }

            public override void OnUpdate()
            {
                Transform player = GameController.instance.GameEnv.PlayerShip.transform;
                Vector3 pos = OwnerMB.transform.position;

                if ( pos.y <= ( SURFACE_Y + 1.5f ) ) {
                    glideForward();
                }
                else {
                    trackPlayer();
                }
            }

            private void glideForward()
            {
                Transform player = GameController.instance.GameEnv.PlayerShip.transform;
                Vector3 pos = OwnerMB.transform.position;

                // just keep going towards the ground, and death
                pos += OwnerMB.transform.forward * Time.deltaTime * SPEED;
                OwnerMB.transform.position = pos;
                if ( pos.y <= SURFACE_Y ) {
                    ( (Seeker)Owner )._killme_flag = true;
                    Instantiate( ( (Seeker)Owner ).ExplosionPrefab, OwnerMB.transform.position, Quaternion.identity );
                }
            }

            protected void trackPlayer()
            {
                Transform player = GameController.instance.GameEnv.PlayerShip.transform;
                Vector3 pos = OwnerMB.transform.position;

                pos = Vector3.MoveTowards( pos, player.position, Time.deltaTime * 0.25f );
                pos += OwnerMB.transform.forward * Time.deltaTime * SPEED;
                OwnerMB.transform.position = pos;

                Vector3 foo = player.position - OwnerMB.transform.position;
                Quaternion bar = Quaternion.LookRotation( foo );
                OwnerMB.transform.rotation = Quaternion.Lerp( OwnerMB.transform.rotation, bar, Time.deltaTime * 5 );
            }
        }

        /*******************************************************/
        class SeekerState_TRACKING_RELENTLESS : SeekerState_TRACKING
        {
            public override Enum Name { get { return SeekerState.TRACKING_RELENTLESS; } }

            public override void OnUpdate()
            {
                //base.OnUpdate();

                trackPlayer();

                // Check if we're past the lock on point and straighten us out. We'll keep going 
                // forward at this point, either off the stage, or directly into the player, if they
                // fail to teleport behind us successfully.

                if ( OwnerMB.transform.position.y <= SURFACE_Y ) {
                    SnapTrajectory();
                }
            }

            ////TODO: This should lerp into position, not snap, but whatever...
            private void SnapTrajectory()
            {
                Transform player = GameController.instance.GameEnv.PlayerShip.transform;

                OwnerMB.transform.LookAt( player.position + new Vector3( 0, 0.8f, 0 ) );

                Vector3 rot = OwnerMB.transform.rotation.eulerAngles;
                rot.x = 0f;
                rot.y = ( OwnerMB.transform.rotation.eulerAngles.y > 135 ) ? 270.0f : 90.0f;

                OwnerMB.transform.rotation = Quaternion.Euler( rot );

                Owner.ChangeState( SeekerState.LOCKED );
            }
        }

        /*******************************************************/
        class SeekerState_LOCKED : State
        {
            public override Enum Name { get { return SeekerState.LOCKED; } }

            public override void OnUpdate()
            {
                // Ever forward. If we go off screen, Seeker.Update will catch
                // it and recycle us.

                Vector3 pos = OwnerMB.transform.position;

                pos += OwnerMB.transform.forward * Time.deltaTime * SPEED;
                OwnerMB.transform.position = pos;
            }
        }

        /************************/
        public void Awake()
        {
            AddState( new SeekerState_TRACKING() );
            AddState( new SeekerState_TRACKING_RELENTLESS() );
            AddState( new SeekerState_LOCKED() );
        }

        /************************/
        public void Update()
        {
            base.Update();

            if ( IsOffScreen() ) {
                InstaKill();
            }
        }

        /************************/
        private bool IsOffScreen()
        {
            return ( ( transform.position.x < ( SpawnMinX - 10.0f ) ) ||
                     ( transform.position.x > ( SpawnMaxX + 10.0f ) ) ) || _killme_flag;
        }

        /************************/
        public override void Respawn()
        {
            base.Respawn();
            _killme_flag = false;

            if ( UnityEngine.Random.Range( 0, 100 ) <= RELENTLESS_CHANCE ) {
                Debug.Log("REL");
                ChangeState( SeekerState.TRACKING_RELENTLESS );
            }
            else {
                Debug.Log( "NORM" );
                ChangeState( SeekerState.TRACKING );
            }
        }
    }
}