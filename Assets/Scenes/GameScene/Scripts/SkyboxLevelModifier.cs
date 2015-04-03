using UnityEngine;

public class SkyboxLevelModifier : MonoBehaviour
{
    public void Update()
    {
        int score = GameController.instance.State.Score;

        //float hue = ( 1.0f / 10000 ) * score;
        float hue = 0.5f;
        float adj_hue = hue % 1.0f;

        RenderSettings.fogColor = HSVToRGB( adj_hue, 0.52f, 0.82f );
    }



    private Color HSVToRGB( float h, float s, float v, float a = 1.0f )
    {
        Color rgb = new Color();

        var chroma = s * v;
        var hdash = h / 60.0f;
        var x = chroma * ( 1.0f - Mathf.Abs( ( hdash % 2.0f ) - 1.0f ) );

        if ( hdash < 1.0f ) {
            rgb.r = chroma;
            rgb.g = x;
        }
        else if ( hdash < 2.0f ) {
            rgb.r = x;
            rgb.g = chroma;
        }
        else if ( hdash < 3.0f ) {
            rgb.g = chroma;
            rgb.b = x;
        }
        else if ( hdash < 4.0f ) {
            rgb.g = x;
            rgb.b = chroma;
        }
        else if ( hdash < 5.0f ) {
            rgb.r = x;
            rgb.b = chroma;
        }
        else if ( hdash <= 6.0f ) {
            rgb.r = chroma;
            rgb.b = x;
        }

        var min = v - chroma;

        rgb.r += min;
        rgb.g += min;
        rgb.b += min;
        rgb.a = a;

        return rgb;
    }

    //private static Color ColorFromHSV( float h, float s, float v, float a = 1 )
    //{
    //    // no saturation, we can return the value across the board (grayscale)
    //    if ( Mathf.Approximately( s, 0 ) )
    //        return new Color( v, v, v, a );

    //    // which chunk of the rainbow are we in?
    //    float sector = h / 60;

    //    // split across the decimal (ie 3.87 into 3 and 0.87)
    //    int i = (int)sector;
    //    float f = sector - i;

    //    float p = v * ( 1 - s );
    //    float q = v * ( 1 - s * f );
    //    float t = v * ( 1 - s * ( 1 - f ) );

    //    // build our rgb color
    //    Color color = new Color( 0, 0, 0, a );

    //    switch ( i ) {
    //        case 0:
    //            color.r = v;
    //            color.g = t;
    //            color.b = p;
    //            break;

    //        case 1:
    //            color.r = q;
    //            color.g = v;
    //            color.b = p;
    //            break;

    //        case 2:
    //            color.r = p;
    //            color.g = v;
    //            color.b = t;
    //            break;

    //        case 3:
    //            color.r = p;
    //            color.g = q;
    //            color.b = v;
    //            break;

    //        case 4:
    //            color.r = t;
    //            color.g = p;
    //            color.b = v;
    //            break;

    //        default:
    //            color.r = v;
    //            color.g = p;
    //            color.b = q;
    //            break;
    //    }

    //    return color;
    //}
}
