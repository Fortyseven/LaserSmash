using System;
using UnityEngine;

public class GameControllerState_INTRO_ANIM : StateMachineMB.State
{
    public override Enum Name { get { return GameController.NewGameState.INTRO_ANIM; } }

    public override void OnUpdate()
    {
        ;
    }

    public override void OnMessageReceived( object o )
    {
        Owner.ChangeState( GameController.NewGameState.RUNNING );
    }
}
