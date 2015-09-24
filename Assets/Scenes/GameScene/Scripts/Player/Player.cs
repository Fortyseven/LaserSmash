//#define TESTMODE

using System;
using UnityEngine;
using UnityEngine.UI;
using Game;
using Random = UnityEngine.Random;

public class Player : StateMachineMB
{
    private const float SHIP_SPEED = 13.0f;
    //private const float SHIP_X_BOUNDS = 13.0f;
    private const float LASER_Y_OFFSET_FROM_SHIP = 2.0f;
    private const float FIRE_DELAY = 0.33f;

    public GameObject LaserbeamPrefab = null;
    public GameObject DeathExplosionPrefab = null;

    public Image DeathPanel; // The red death filter while exploding
    public GameObject My_Mesh;


    //private bool _ready = false;

    public enum PlayerState { NORMAL, RESET, KILLED };

    /***********************************************************************************/
    public class PlayerState_NORMAL : State
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
    public class PlayerState_RESET : State
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
            ( (Player)Owner ).DeathPanel.gameObject.SetActive( false );
            OwnerMB.transform.position = _starting_position;
#endif
            OwnerMB.gameObject.SetActive( true );
            ( (Player)Owner ).My_Mesh.SetActive( true );
            OwnerMB.enabled = true;
            Owner.ChangeState( PlayerState.NORMAL );
        }

        public override void OnUpdate()
        {
            ;
        }
    }

    /***********************************************************************************/
    public class PlayerState_KILLED : State
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
            ( (Player)Owner ).My_Mesh.SetActive( false );
            ( (Player)OwnerMB ).enabled = false;

            Destroy(
                Instantiate( ( (Player)Owner ).DeathExplosionPrefab, OwnerMB.transform.position, Quaternion.identity ),
                3.0f );

            GameController.instance.GameEnv.Lives--;
            GameController.instance.GameEnv.AdjustScore( GameConstants.SCORE_PLAYERDEATH );

            if ( GameController.instance.GameEnv.Lives <= 0 ) {
                OwnerMB.gameObject.SetActive( false );
                GameController.instance.ChangeState( GameController.GameState.GAMEOVER );
            }
            else {
                ( (Player)Owner ).DeathPanel.gameObject.SetActive( true );
                GameController.instance.GameEnv.WaveCon.Paused = true;
                GameController.instance.GameEnv.WaveCon.Reset();

                _timeout_timer = 3.0f;
            }
        }

        public override void OnUpdate()
        {
            _timeout_timer -= Time.deltaTime;

            if ( _timeout_timer <= 0 ) {
                Owner.ChangeState( PlayerState.RESET );
            }
        }
    }

    /**************************************/
    public void Start()
    {
        enabled = false;

        AddState( new PlayerState_NORMAL() );
        AddState( new PlayerState_RESET() );
        AddState( new PlayerState_KILLED() );

        DeathPanel.gameObject.SetActive( false );

        // Just sit pretty until GameController changes us to Reset
    }

    /**************************************/
    //public void Done()
    //{
    //    Debug.LogError( "Done?" );
    //    Debug.Break();

    //    //        _ready = false;
    //    enabled = false;
    //}
}

