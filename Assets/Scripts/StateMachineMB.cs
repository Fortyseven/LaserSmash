/************************************************************************
** StateMachineMB.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using System;
using UnityEngine;
using System.Collections.Generic;

public class StateMachineMB : MonoBehaviour
{
    public abstract class State
    {
        public abstract Enum Name { get; }

        public StateMachineMB Owner { get; set; }
        protected MonoBehaviour OwnerMB { get { return Owner; } }
        public bool SkipUpdateOnZeroTimeScale { get; set; }

        #region Equality
        protected bool Equals( State other )
        {
            return Equals( Owner, other.Owner ) && SkipUpdateOnZeroTimeScale == other.SkipUpdateOnZeroTimeScale;
        }

        public override bool Equals( object obj )
        {
            if ( ReferenceEquals( null, obj ) ) return false;
            if ( ReferenceEquals( this, obj ) ) return true;
            if ( obj.GetType() != this.GetType() ) return false;
            return Equals( (State)obj );
        }

        public override int GetHashCode()
        {
            unchecked {
                return ( ( Owner != null ? Owner.GetHashCode() : 0 ) * 397 ) ^ SkipUpdateOnZeroTimeScale.GetHashCode();
            }
        }

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
        #endregion

        public virtual void Start()
        {
            ;
        }

        public virtual void OnStateEnter( State from_state )
        {
            ;
        }

        public virtual void OnStateExit( State to_state )
        {
            ;
        }

        public abstract void OnUpdate();

        public virtual void OnStateMessageReceived( object o )
        {
            throw new NotImplementedException( "Message \"" + o + "\" was received, but OnMessageReceived not implemented" );
        }

        internal void SendStateMessage( object o = null )
        {
            OnStateMessageReceived( o );
        }
    }

    public bool DebugMode { get; set; }
    public State CurrentState { get; private set; }
    protected Dictionary<Enum, State> States { get; private set; }
    protected bool InTransition { get; private set; }
    private bool SkipUpdateOnZeroTimeScale { get; set; }

    protected StateMachineMB()
    {
        DebugMode = false;
        CurrentState = null;
        States = new Dictionary<Enum, State>( 0 );
        InTransition = false;
        SkipUpdateOnZeroTimeScale = true;
    }

    // This ugly thing here is here to satisfy a bug(?) in Unity's 
    // onClick callback in the editor
    public void SendStateMessage( String o = null )
    {
        if ( CurrentState != null ) {
            CurrentState.SendStateMessage( o );
        }
    }

    public void SendStateMessage( object o = null )
    {
        if ( CurrentState != null ) {
            CurrentState.SendStateMessage( o );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected void Update()
    {
        if ( InTransition || CurrentState == null )
            return;

        if ( CurrentState.SkipUpdateOnZeroTimeScale && Time.timeScale == 0 ) {
            return;
        }

        CurrentState.OnUpdate();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    protected void AddState( State state )
    {
        States.Add( state.Name, state );
        state.Owner = this;

        // bubble up to new states whatever the default is; state can then override in Start
        state.SkipUpdateOnZeroTimeScale = this.SkipUpdateOnZeroTimeScale;

        state.Start();
    }

    public void ChangeState( State next_state )
    {
        ChangeState( next_state.Name );
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
