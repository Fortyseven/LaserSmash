using System;
using UnityEngine;
using System.Collections.Generic;

public class StateMachineMB : MonoBehaviour
{
    public abstract class State
    {
        public abstract Enum Name { get; }

        public StateMachineMB Owner { get; set; }
        public MonoBehaviour OwnerMB { get { return (MonoBehaviour)Owner; } }

        public abstract void Start();

        public virtual void Reset()
        {
            ;
        }

        public abstract void OnStateEnter( State from_state );
        public abstract void OnStateExit( State to_state );
        public abstract void OnUpdate();
    }

    public State CurrentState { get; private set; }
    protected Dictionary<Enum, State> States { get; private set; }

    protected bool InTransition { get; private set; }



    /// <summary>
    /// 
    /// </summary>
    protected void Start()
    {
        CurrentState = null;
        States = new Dictionary<Enum, State>( 0 );
    }

    /// <summary>
    /// 
    /// </summary>
    protected void Update()
    {
        if ( InTransition )
            return;

        if ( CurrentState != null )
            CurrentState.OnUpdate();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    public void AddState( State state )
    {
        States.Add( state.Name, state );
        state.Owner = this;
        state.Start();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next_state"></param>
    public void ChangeState( Enum next_state )
    {
        InTransition = true;

        if ( CurrentState != null ) {
            CurrentState.OnStateExit( States[ next_state ] );
        }

        States[ next_state ].OnStateEnter( CurrentState );
        CurrentState = States[ next_state ];

        InTransition = false;
    }
}
