/************************************************************************
** SkyboxLevelModifier.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using UnityEngine;

namespace Game
{
    public class SkyboxLevelModifier : MonoBehaviour
    {
        private const float SCORE_WRAP_POINT = 10000;

        public void Update()
        {
            //FIXME: We don't need to run this every frame; scale it back

            float hue = (1.0f/SCORE_WRAP_POINT)*GameController.instance.GameEnv.Score;
            float adj_hue = hue%1.0f; // loop
            //Debug.Log( adj_hue );
            RenderSettings.fogColor = Utils.HSVtoRGB( adj_hue, 0.35f, 0.82f );
        }
    }
}