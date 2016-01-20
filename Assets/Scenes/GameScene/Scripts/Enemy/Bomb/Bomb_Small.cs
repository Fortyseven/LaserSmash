using Game;
using UnityEngine;

namespace Game
{
    public class Bomb_Small : BaseBomb
    {
        protected override int BaseScore { get { return GameConstants.SCORE_BOMB_SM; } }

        protected override float SpawnYOffset { get { return 15.0f; } }

        //protected override float MinSpeed { get { return 3.5f; } }
        //protected override float MaxSpeed { get { return 10.0f; } }
    }
}