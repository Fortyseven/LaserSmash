using UnityEditor;
using UnityEngine;

public class SkyboxLevelModifier : MonoBehaviour
{
    private Material _skybox_mat;
    private Color _skytint_start;

    public void Start()
    {
        _skybox_mat = new Material( GetComponent<Skybox>().material );

        // Make a copy so we don't trample the original material file (wtf, right?)
        GetComponent<Skybox>().material = _skybox_mat;
        _skytint_start = _skybox_mat.GetColor( "_SkyTint" );

        //Debug.Log( _skytint_start );
        //ColorToHSV( _skytint_start, out h, out s, out v );
        //Debug.Log( h );
    }

    public void Update()
    {
        int score = GameController.instance.State.Score;
        float hue = (( 1.0f / 10000.0f ) * score) % 1.0f;

        Color col = EditorGUIUtility.HSVToRGB( (float)hue, 0.52f, 0.82f );
        //Color col = ColorFromHSV( (float)hue, 0.52f, 0.82f );
        Debug.Log( hue + ", " + col + " - " + score );
        //_skybox_mat.SetColor( "_SkyTint", col );
        RenderSettings.fogColor = col;
    }

    private static Color ColorFromHSV( float h, float s, float v, float a = 1 )
    {
        Debug.Log( string.Format( "h = {0}, s = {1}, v = {2}", h, s, v ) );
        // no saturation, we can return the value across the board (grayscale)
        if ( Mathf.Approximately( s, 0 ) )
            return new Color( v, v, v, a );

        // which chunk of the rainbow are we in?
        float sector = h / 60;

        // split across the decimal (ie 3.87 into 3 and 0.87)
        int i = (int)sector;
        float f = sector - i;

        float p = v * ( 1 - s );
        float q = v * ( 1 - s * f );
        float t = v * ( 1 - s * ( 1 - f ) );

        // build our rgb color
        Color color = new Color( 0, 0, 0, a );

        switch ( i ) {
            case 0:
                color.r = v;
                color.g = t;
                color.b = p;
                break;

            case 1:
                color.r = q;
                color.g = v;
                color.b = p;
                break;

            case 2:
                color.r = p;
                color.g = v;
                color.b = t;
                break;

            case 3:
                color.r = p;
                color.g = q;
                color.b = v;
                break;

            case 4:
                color.r = t;
                color.g = p;
                color.b = v;
                break;

            default:
                color.r = v;
                color.g = p;
                color.b = q;
                break;
        }

        return color;
    }

    private static void ColorToHSV( Color color, out float h, out float s, out float v )
    {
        float min = Mathf.Min( Mathf.Min( color.r, color.g ), color.b );
        float max = Mathf.Max( Mathf.Max( color.r, color.g ), color.b );
        float delta = max - min;

        // value is our max color
        v = max;

        // saturation is percent of max
        if ( !Mathf.Approximately( max, 0 ) )
            s = delta / max;
        else {
            // all colors are zero, no saturation and hue is undefined
            s = 0;
            h = -1;
            return;
        }

        // grayscale image if min and max are the same
        if ( Mathf.Approximately( min, max ) ) {
            v = max;
            s = 0;
            h = -1;
            return;
        }

        // hue depends which color is max (this creates a rainbow effect)
        if ( Mathf.Approximately( color.r, max ) )
            h = ( color.g - color.b ) / delta;         	// between yellow & magenta
        else if ( Mathf.Approximately( color.g, max ) )
            h = 2 + ( color.b - color.r ) / delta; 		// between cyan & yellow
        else
            h = 4 + ( color.r - color.g ) / delta; 		// between magenta & cyan

        // turn hue into 0-360 degrees
        h *= 60;
        if ( h < 0 )
            h += 360;
    }
}
