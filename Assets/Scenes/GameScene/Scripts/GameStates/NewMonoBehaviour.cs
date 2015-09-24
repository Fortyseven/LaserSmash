using System;
using UnityEngine;

public class GameControllerState_PLAYER_DYING : StateMachineMB.State
{
    public override Enum Name { get { return GameController.GameState.PLAYER_DYING; } }

    public override void OnUpdate()
    {
        ;
    }

    public override void OnMessageReceived( object o )
    {
        ExplosionFinished();
    }

    private void ExplosionFinished()
    {
        Debug.Log( "Explosion Finished" );
    }
}
