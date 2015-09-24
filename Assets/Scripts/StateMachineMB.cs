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

        public static bool operator ==( State a, Enum b )
        {
            if ( System.Object.ReferenceEquals( a, b ) ) {
                return true;
            }

            if ( ( (object)a == null ) || ( (object)b == null ) ) {
                return false;
            }

            return a.Name.Equals( b );
        }

        public static bool operator !=( State a, Enum b )
        {
            return !( a == b );
        }

        public virtual void Start()
        {
            ;
        }

        //public virtual void Reset()
        //{
        //    ;
        //}

        public virtual void OnStateEnter( State from_state )
        {
            ;
        }

        public virtual void OnStateExit( State to_state )
        {
            ;
        }

        public abstract void OnUpdate();

        public virtual void OnMessageReceived( object o )
        {
            throw new NotImplementedException( "Message was received, but OnMessageReceived not implemented" );
        }

        internal void SendMessage( object o = null )
        {
            OnMessageReceived( o );
        }
    }

    public bool DebugMode { get; set; }
    public State CurrentState { get; private set; }
    protected Dictionary<Enum, State> States { get; private set; }
    protected bool InTransition { get; private set; }

    public StateMachineMB()
    {
        DebugMode = false;
        CurrentState = null;
        States = new Dictionary<Enum, State>( 0 );
        InTransition = false;
    }

    /// <summary>
    /// 
    /// </summary>
    //protected void Start()
    //{
    //    States = new Dictionary<Enum, State>( 0 );
    //}

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
    /// Change to the specified state. Exit and Enter callbacks will be called where applicable.
    /// </summary>
    /// <param name="next_state"></param>
    public void ChangeState( Enum next_state )
    {
        if ( DebugMode ) {
            if ( CurrentState != null )
                Debug.LogWarning( "### ChangeState " + CurrentState.Name + " -> " + next_state );
            else
                Debug.LogWarning( "### ChangeState INIT " + next_state );
        }
        State from_state = CurrentState;

        InTransition = true;

        if ( from_state != null ) {
            from_state.OnStateExit( States[ next_state ] );
        }

        CurrentState = States[ next_state ];

        States[ next_state ].OnStateEnter( from_state );

        InTransition = false;
    }
}
