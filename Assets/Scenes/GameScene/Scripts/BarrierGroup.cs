/************************************************************************
** BarrierGroup.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using UnityEngine;

namespace Game
{
    public class BarrierGroup : StateMachineMB
    {
        public Barrier BarrierLeft;
        public Barrier BarrierRight;
        public GameObject ExplosionPrefab;

        public void Zapped( Vector3 position )
        {
            if ( Mathf.Sign( position.x ) < 0 ) {
                BarrierLeft.Zap();
            }
            else {
                BarrierRight.Zap();
            }

            SpawnSpark( position );
        }

        private void SpawnSpark( Vector3 position )
        {
            Instantiate( ExplosionPrefab, position, Quaternion.identity );
        }
    }
}