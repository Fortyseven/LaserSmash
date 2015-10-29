using Assets.Scenes.GameScene.Scripts.Enemy;
using Game;
using UnityEngine;

public class Bomb_Large : BaseBomb
{
    protected override int BaseScore { get { return GameConstants.SCORE_BOMB_LG; } }
    protected override float SpawnYOffset { get { return 16.0f; } }

    protected override float MinSpeed { get { return 3.5f; } }
    protected override float MaxSpeed { get { return 10.0f; } }
}
