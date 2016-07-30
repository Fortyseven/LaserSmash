using UnityEngine;

public class FadeInPanel : MonoBehaviour
{
    [Tooltip("Texture to use for fading.")]
    public Texture2D FadeTexture;
    [Tooltip("Speed of the fade")]
    public float FadeSpeed = 0.003f;
    [Tooltip("Are we fading out, or fading in?")]
    public bool FadeOut = true;
    [Tooltip("Immediately perform fade (otherwise wait for Trigger)")]
    public bool FadeOnStart = true;

    public delegate void OnFadeFinish_CB();
    public event OnFadeFinish_CB OnFadeFinish;

    private Rect _rect;
    private Color _color;
    private float _alpha = 0;
    private bool _triggered = false;
    private bool _onfadefinish_called = false;
    //private bool _did_one_frame = false;

    void OnGUI()
    {
        GUI.color = _color;
        GUI.depth = -1000;
        GUI.DrawTexture( _rect, FadeTexture );

        if ( _triggered /*&& !_did_one_frame*/ ) {

            if ( !_onfadefinish_called && ( _alpha >= 1.0f ) ) {
                Debug.Log( "Fade finished." );
                if ( OnFadeFinish != null ) {
                    OnFadeFinish();
                }
                _onfadefinish_called = true;
                _triggered = false;
            }
            _color.a = Mathf.Clamp( FadeOut ? _alpha : ( 1.0f - _alpha ), 0.0f, 1.0f );

            _alpha += FadeSpeed * Time.deltaTime;
            //_did_one_frame = true;
        }

    }

    public void Start()
    {
        Reset();
    }

    public void Reset()
    {
        _rect = new Rect( 0.0f, 0.0f, Screen.width, Screen.height );
        _color = new Color( 1.0f, 1.0f, 1.0f, ( FadeOut ? 0.0f : 1.0f ) );

        if ( FadeOnStart ) {
            Trigger();
        }
    }

    public void Trigger()
    {
        _triggered = true;
    }
}
