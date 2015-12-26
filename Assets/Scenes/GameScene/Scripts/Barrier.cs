using System;
using UnityEngine;
using System.Collections;

public class Barrier : StateMachineMB
{
    private const float BARRIER_FADE_SPEED = 1.5f;

    protected enum BarrierState { IDLE, ZAPPED };

    private Material _my_material;
    protected Color _color;

    /***************************************************************************/
    protected class BarrierState_IDLE : State
    {
        public override Enum Name { get { return BarrierState.IDLE; } }
        public override void OnStateEnter( State from_state )
        {
            ( (Barrier)Owner )._color.a = 0.0f;
            ( (Barrier)Owner )._my_material.color = ( (Barrier)Owner )._color;
        }

        public override void OnUpdate()
        {
        }
    }

    /***************************************************************************/
    protected class BarrierState_ZAPPED : State
    {
        public override Enum Name { get { return BarrierState.ZAPPED; } }


        public override void OnStateEnter( State from_state )
        {
            base.OnStateEnter( from_state );
            ( (Barrier)Owner )._color.a = 1.0f;
        }

        public override void OnUpdate()
        {
            ( (Barrier)Owner )._my_material.color = ( (Barrier)Owner )._color;

            if ( ( (Barrier)Owner )._color.a > 0 )
                ( (Barrier)Owner )._color.a -= BARRIER_FADE_SPEED * Time.deltaTime;
            else {
                Owner.ChangeState( BarrierState.IDLE );
            }
        }
    }

    /***************************************************************************/
    public void Awake()
    {
        // Clone shared material so we can modify it independently
        _my_material = new Material( gameObject.GetComponent<Renderer>().sharedMaterial );
        gameObject.GetComponent<Renderer>().sharedMaterial = _my_material;

        AddState( new BarrierState_IDLE() );
        AddState( new BarrierState_ZAPPED() );
        ChangeState( BarrierState.IDLE );
    }

    public void Zap()
    {
        ChangeState( BarrierState.ZAPPED );
    }
}