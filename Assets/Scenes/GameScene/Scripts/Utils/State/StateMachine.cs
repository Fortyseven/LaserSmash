using System;
using UnityEngine;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour
{
    public Enum ActiveState { get; private set; }
    private enum DefaultState { NOT_STARTED = -1 };

    private Dictionary<Enum, StateBehavior> _state_behaviors;
    private MonoBehaviour _owner;

    public StateMachine()
    {
        ActiveState = DefaultState.NOT_STARTED;
    }

    public void Init( MonoBehaviour owner )
    {
        _owner = owner;
    }

    public void SwitchStateTo( Enum new_state )
    {
        Enum old_state = ActiveState;

        if ( !old_state.Equals( DefaultState.NOT_STARTED ) ) {
            _state_behaviors[ old_state ].OnExit( new_state );
        }
        _state_behaviors[ new_state ].OnEnter( old_state );

        ActiveState = new_state;
    }

    public void Awake()
    {
        _state_behaviors = new Dictionary<Enum, StateBehavior>( 0 );
    }

    public void AddState<TClass>( Enum id )
    {
        //StateBehavior be = Activator.CreateInstance<TClass>() as StateBehavior;
        var be = _owner.gameObject.AddComponent( typeof( TClass ) ) as StateBehavior;


        _state_behaviors.Add( id, be );

        _state_behaviors[ id ].Parent = _owner;
        _state_behaviors[ id ].Machine = this;
        _state_behaviors[ id ].Init();
    }

    public void Update()
    {
        if ( ActiveState.Equals( DefaultState.NOT_STARTED ) || !gameObject.activeInHierarchy )
            return;

        _state_behaviors[ ActiveState ].OnUpdate();
    }

    public void FixedUpdate()
    {
        if ( ActiveState.Equals( DefaultState.NOT_STARTED ) || !gameObject.activeInHierarchy )
            return;

        _state_behaviors[ ActiveState ].OnFixedUpdate();
    }
}