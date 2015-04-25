using System;
using UnityEngine;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour
{
    public Enum ActiveState { get; private set; }

    private Dictionary<Enum, StateBehavior> _state_behaviors;
    private MonoBehaviour _owner;

    public StateMachine()
    {
        ActiveState = null;
    }

    public void Init( MonoBehaviour owner )
    {
        _owner = owner;
    }

    public void SwitchStateTo( Enum state_enum )
    {
        if ( ActiveState != null ) {
            _state_behaviors[ ActiveState ].OnExit();
        }
        ActiveState = state_enum;
        _state_behaviors[ ActiveState ].OnEnter();
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
        if ( ActiveState == null || !gameObject.activeInHierarchy )
            return;

        _state_behaviors[ ActiveState ].OnUpdate();
    }

    public void FixedUpdate()
    {
        if ( ActiveState == null || !gameObject.activeInHierarchy )
            return;

        _state_behaviors[ ActiveState ].OnFixedUpdate();
    }
}