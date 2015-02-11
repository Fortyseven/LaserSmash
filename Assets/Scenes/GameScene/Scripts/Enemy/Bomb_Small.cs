using Assets.Scenes.GameScene.Scripts.Enemy;
using Game;
using UnityEngine;

public class Bomb_Small : BaseBomb
{
    protected override int BaseScore { get { return GameConstants.SCORE_BOMB_SM; } }
    protected override float SpawnYOffset { get { return 15.0f; } }

    protected override float MinForce { get { return 150.0f; } }
    protected override float MaxForce { get { return 1000.0f; } }

}
