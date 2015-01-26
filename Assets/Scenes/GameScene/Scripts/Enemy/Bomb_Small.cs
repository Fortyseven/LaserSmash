using Assets.Scenes.GameScene.Scripts.Enemy;
using Game;
using UnityEngine;

public class Bomb_Small : BaseBomb
{
    protected override int BaseScore { get { return GameConstants.SCORE_BOMB_SM; } }
    protected override float SpawnYOffset { get { return 15.0f; } }

    protected const float MIN_WEIGHT = 20.0f;
    protected const float MAX_WEIGHT = 40.0f;

    public override void Respawn()
    {
        rigidbody2D.gravityScale = _base_gravity_mult * Random.Range( MIN_WEIGHT, MAX_WEIGHT );
        base.Respawn();
    }
}
