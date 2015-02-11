using Assets.Scenes.GameScene.Scripts.Enemy;
using Game;
using UnityEngine;

public class Bomb_Large : BaseBomb
{
    protected override int BaseScore { get { return GameConstants.SCORE_BOMB_LG; } }
    protected override float SpawnYOffset { get { return 16.0f; } }

    protected override float MinForce { get { return 150.0f; } }
    protected override float MaxForce { get { return 1000.0f; } }
}
