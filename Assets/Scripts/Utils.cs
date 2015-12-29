/************************************************************************
** Utils.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using UnityEngine;

namespace Game
{
    class Utils
    {
        // Yoinked from https://goo.gl/v1ui5E
        public static Color32 HSVtoRGB( float h, float s, float v )
        {
            int H = (int)(h * 6);
            float f = h * 6 - H;
            float p = v * (1 - s);
            float q = v * (1 - f * s);
            float t = v * (1 - (1 - f) * s);

            float r=0;
            float g=0;
            float b=0;

            switch ( H ) {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                case 5: r = v; g = p; b = q; break;
            }
            return new Color32( (byte)( 255 * r ), (byte)( 255 * g ), (byte)( 255 * b ), 255 );
        }
    }
}
