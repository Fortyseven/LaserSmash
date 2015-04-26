using System;
using UnityEngine;

public class Test_StateMachine : MonoBehaviour
{
    private bool Init_A = false;
    private bool Init_B = false;
    private bool Init_C = false;

    private bool Enter_A = false;
    private bool Enter_B = false;
    private bool Enter_C = false;

    private bool Exit_A = false;
    private bool Exit_B = false;
    private bool Exit_C = false;

    private bool Machine_A = false;
    private bool Machine_B = false;
    private bool Machine_C = false;

    private bool Update_A = false;
    private bool Update_B = false;
    private bool Update_C = false;

    private bool FUpdate_A = false;
    private bool FUpdate_B = false;
    private bool FUpdate_C = false;


    private StateMachine _state_machine;


    private class Behavior_A : StateBehavior
    {
        public override void OnEnter( Enum changing_from )
        {
            IntegrationTest.Assert( changing_from == null );
            ( (Test_StateMachine)Parent ).Enter_A = true;
        }

        public override void OnExit( Enum changing_to )
        {
            IntegrationTest.Assert( changing_to.Equals( BEHAVIORS.B ) );
            ( (Test_StateMachine)Parent ).Exit_A = true;
        }

        public override void OnUpdate()
        {
            ( (Test_StateMachine)Parent ).Update_A = true;
            Machine.SwitchStateTo( BEHAVIORS.B );
        }

        public override void OnFixedUpdate()
        {
            ( (Test_StateMachine)Parent ).FUpdate_A = true;
        }

        public override void Init()
        {
            base.Init();
            ( (Test_StateMachine)Parent ).Init_A = true;
            if ( Machine.Equals( ( Parent ).GetComponent<StateMachine>() ) ) {
                ( (Test_StateMachine)Parent ).Machine_A = true;
            }
        }
    }

    private class Behavior_B : StateBehavior
    {
        public override void OnEnter( Enum changing_from )
        {
            IntegrationTest.Assert( changing_from.Equals( BEHAVIORS.A ) );
            ( (Test_StateMachine)Parent ).Enter_B = true;
        }

        public override void OnExit( Enum changing_to )
        {
            IntegrationTest.Assert( changing_to.Equals( BEHAVIORS.C ) );
            ( (Test_StateMachine)Parent ).Exit_B = true;
        }

        public override void OnUpdate()
        {
            ( (Test_StateMachine)Parent ).Update_B = true;
            Machine.SwitchStateTo( BEHAVIORS.C );
        }

        public override void OnFixedUpdate()
        {
            ( (Test_StateMachine)Parent ).FUpdate_B = true;
        }

        public override void Init()
        {
            base.Init();
            ( (Test_StateMachine)Parent ).Init_B = true;
            if ( Machine.Equals( ( Parent ).GetComponent<StateMachine>() ) ) {
                ( (Test_StateMachine)Parent ).Machine_B = true;
            }
        }
    }

    private class Behavior_C : StateBehavior
    {
        public override void OnEnter( Enum changing_from )
        {
            IntegrationTest.Assert( changing_from.Equals( BEHAVIORS.B ) );
            ( (Test_StateMachine)Parent ).Enter_C = true;
        }

        public override void OnExit( Enum changing_to )
        {
            ( (Test_StateMachine)Parent ).Exit_C = true;
        }

        public override void OnUpdate()
        {
            ( (Test_StateMachine)Parent ).Update_C = true;
        }

        public override void OnFixedUpdate()
        {
            ( (Test_StateMachine)Parent ).FUpdate_C = true;
        }

        public override void Init()
        {
            base.Init();
            ( (Test_StateMachine)Parent ).Init_C = true;
            if ( Machine.Equals( ( Parent ).GetComponent<StateMachine>() ) ) {
                ( (Test_StateMachine)Parent ).Machine_C = true;
            }
        }
    }

    public enum BEHAVIORS
    {
        A,
        B,
        C
    }

    public void Start()
    {
        _state_machine = GetComponent<StateMachine>();

        _state_machine.Init( this );

        _state_machine.AddState<Behavior_A>( BEHAVIORS.A );
        _state_machine.AddState<Behavior_B>( BEHAVIORS.B );
        _state_machine.AddState<Behavior_C>( BEHAVIORS.C );

        _state_machine.SwitchStateTo( BEHAVIORS.A );
    }

    public void Update()
    {
        //Debug.Log( "update" );
        if ( FUpdate_A && Update_A
            && FUpdate_B && Update_B
            && FUpdate_C && Update_C
            ) {
            IntegrationTest.Assert( Init_A, "INIT_A" );
            IntegrationTest.Assert( Enter_A, "ENTER_A" );
            IntegrationTest.Assert( Exit_A, "EXIT_A" );
            IntegrationTest.Assert( Machine_A, "MACHINE_A" );

            IntegrationTest.Assert( Init_B, "INIT_B" );
            IntegrationTest.Assert( Enter_B, "ENTER_B" );
            IntegrationTest.Assert( Exit_B, "EXIT_B" );
            IntegrationTest.Assert( Machine_B, "MACHINE_B" );

            IntegrationTest.Assert( Init_C, "INIT_C" );
            IntegrationTest.Assert( Enter_C, "ENTER_C" );
            IntegrationTest.Assert( !Exit_C, "EXIT_C" );
            IntegrationTest.Assert( Machine_C, "MACHINE_C" );

            IntegrationTest.Pass( gameObject );
        }
    }
}
