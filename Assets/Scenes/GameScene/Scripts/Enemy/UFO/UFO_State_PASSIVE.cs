using System;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using Random = UnityEngine.Random;
using State = Game.StateMachineMB.State;

namespace Game
{
    /*******************************************************************************/
    class UFO_State_PASSIVE : State
    {
        private const float PASSIVE_SPEED = 10.0f;
        private const float PERCENT_CHANCE_OF_CHARGING_BEAM = 0.3f;
        private const float PERCENT_CHANCE_OF_ENERGY_BALL = 100.0f;
        private const float MIN_LASER_ATTACK_RANGE = -10.0f;
        private const float MAX_LASER_ATTACK_RANGE = 10.0f;
        private const float ENERGYBALL_TIMEOUT = 0.75f;

        public override Enum Name { get { return UFO.UFOState.PASSIVE; } }

        private float _energy_ball_timeout = 0;

        /**************************************/
        public override void OnStateEnter( State from_state )
        {
            ( (UFO)Owner ).Speed = PASSIVE_SPEED;
        }

        /**************************************/
        public override void OnUpdate()
        {
            // Don't do anything but fly if the game isn't actively being played
            if ( !GameController.instance.CurrentState.Name.Equals( GameController.GameState.RUNNING ) )
                return;

            _energy_ball_timeout -= Time.deltaTime;

            // Occasionally fire down hot death
            float my_x = OwnerMB.transform.position.x;
            if ( _energy_ball_timeout <= 0 && Random.Range( 0, 100 ) <= PERCENT_CHANCE_OF_ENERGY_BALL ) {
                FireEnergyBall();
            }
            // Once in a while, fire a big beam
            else if ( ( my_x >= MIN_LASER_ATTACK_RANGE ) && ( my_x <= MAX_LASER_ATTACK_RANGE ) &&
                            Random.Range( 0, 100 ) <= PERCENT_CHANCE_OF_CHARGING_BEAM ) {
                Owner.ChangeState( UFO.UFOState.ATTACKING );
            }
        }

        /**************************************/
        private void FireEnergyBall()
        {
            // TODO: pool this
            UnityEngine.Object.Instantiate( ( (UFO)( Owner ) ).EnergyBallPrefab, Owner.transform.position, Quaternion.identity );
            _energy_ball_timeout = ENERGYBALL_TIMEOUT;
        }
    }
}
