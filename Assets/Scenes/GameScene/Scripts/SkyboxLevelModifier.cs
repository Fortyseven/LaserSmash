using UnityEngine;

public class SkyboxLevelModifier : MonoBehaviour
{
    private const float SCORE_WRAP_POINT = 10000;

    public void Update()
    {
        //FIXME: We don't need to run this every frame; scale it back

        float hue = ( 1.0f / SCORE_WRAP_POINT ) * GameController.instance.GameEnv.Score;
        float adj_hue = hue % 1.0f; // loop
        //Debug.Log( adj_hue );
        RenderSettings.fogColor = HSVtoRGB( adj_hue, 0.35f, 0.82f ); //HSVToRGB( adj_hue, 0.52f, 0.82f );
    }

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
