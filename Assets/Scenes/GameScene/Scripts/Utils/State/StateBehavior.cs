using System;
using UnityEngine;

public class StateBehavior : MonoBehaviour
{
    public MonoBehaviour Parent { get; protected internal set; }
    public StateMachine Machine { get; protected internal set; }

    public virtual void Init()
    {
    }

    public virtual void OnEnter( Enum changing_from )
    {
    }

    public virtual void OnExit( Enum changing_to )
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }
}
