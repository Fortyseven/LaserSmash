using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    public float FlashDelayInSeconds = 1.0f;
    public bool StartEnabled = true;

    Text _renderer;
    float _timeout;
    //    float _initial_alpha;
    bool _is_on;
    bool _paused;

    void Start()
    {
        _renderer = GetComponent<Text>();
        _timeout = Time.time + FlashDelayInSeconds;
        //        _initial_alpha = _renderer.GetAlpha();

        ResetFlashers();
    }

    void Update()
    {
        if ( _paused )
            return;

        if ( Time.time >= _timeout ) {
            _is_on = !_is_on;
            _renderer.enabled = _is_on;
            _timeout = Time.time + FlashDelayInSeconds;
        }
    }

    public void Go()
    {
        Debug.Log( "Enabling Flashers" );
        _paused = false;
        _is_on = true;
        _renderer.enabled = true;
        _timeout = Time.time + FlashDelayInSeconds;
    }

    public void ResetFlashers()
    {
        Debug.Log( "Resetting Flashers" );
        _is_on = StartEnabled;
        _paused = false;

        if ( !_is_on ) {
            _renderer.enabled = false;
            _paused = true;
        }
    }
}
