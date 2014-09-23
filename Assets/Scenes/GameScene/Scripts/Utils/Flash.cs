using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    public float FlashDelayInSeconds = 1.0f;
    public bool StartEnabled = true;

    Text _renderer;
    float _timeout;
//    float _initial_alpha;
    bool _isOn;
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
        if (_paused) return;

        if ( Time.time >= _timeout ) {
            _isOn = !_isOn;
            _renderer.enabled = _isOn;
            _timeout = Time.time + FlashDelayInSeconds;
        }
    }

    public void Go()
    {
        Debug.Log("Enabling Flashers");
        _paused = false;
        _isOn = true;
        _renderer.enabled = true;
        _timeout = Time.time + FlashDelayInSeconds;
    }

    public void ResetFlashers()
    {
        Debug.Log("Resetting Flashers");
        _isOn = StartEnabled;
        _paused = false;
        
        if (!_isOn) {
            _renderer.enabled = false;
            _paused = true;
        }
    }
}
