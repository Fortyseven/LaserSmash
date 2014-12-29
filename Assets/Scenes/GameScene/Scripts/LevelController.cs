using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{
    public GameObject LightUnderlight;
    public GameObject LightAmbience;
    public GameObject LightToplight;

    void Start()
    {
        iTween.Init( this.gameObject );

        //      mLightAccent = GameObject.Find("AccentLight").GetComponent<Light>();
        //        mGOLightSide = GameObject.Find("SideLight");
        //        mGOLightAccent = GameObject.Find("AccentLight");
        //        mGOLightTop = GameObject.Find("TopLight");
        //      mLightTop = GameObject.Find("TopLight").GetComponent<Light>();
        //      mLightSide = GameObject.Find("SideLight").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug
        if ( Input.GetKey( "1" ) ) {
            SetLevel( 1 );
        }
        if ( Input.GetKey( "2" ) ) {
            SetLevel( 2 );
        }
        if ( Input.GetKey( "3" ) ) {
            SetLevel( 3 );
        }
        if ( Input.GetKey( "4" ) ) {
            SetLevel( 4 );
        }
        if ( Input.GetKey( "5" ) ) {
            SetLevel( 5 );
        }
    }

    void SetLevel( int level )
    {
        Color col;

        switch ( level ) {
            case 1:
                col = new Color( 1.0f, 0, 0 );
                break;
            case 2:
                col = new Color( 0, 1.0f, 0 );
                break;
            case 3:
                col = new Color( 0, 0, 1.0f );
                break;
            case 4:
                col = new Color( 1.0f, 0, 1.0f );
                break;
            case 5:
            default:
                col = new Color( 1.0f, 1.0f, 1.0f );
                break;
        }

        iTween.ColorTo( LightAmbience, col, 1.0f );
        iTween.ColorTo( LightUnderlight, col, 1.0f );
        //mLightAccent.color = col;
        //mLightSide.color = col;
        //mLightTop.color = col;
    }
}
