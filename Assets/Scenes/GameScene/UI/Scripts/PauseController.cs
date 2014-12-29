using UnityEngine;

//TODO: Add [1] and [9] pause easter egg

public class PauseController : MonoBehaviour
{
    public Canvas PauseCanvas;
    //    bool is_paused = false;

    void Awake()
    {
        PauseCanvas.gameObject.SetActive( false );
    }

    void Update()
    {

        if ( Input.GetKeyDown( KeyCode.Escape ) ) {
            if ( GameController.instance.State.Paused ) {
                OnPauseLeave();
            }
            else {
                OnPauseEnter();
            }
        }
    }

    private void OnPauseEnter()
    {
        GameController.instance.State.Paused = true;
        PauseCanvas.gameObject.SetActive( true );
    }

    private void OnPauseLeave()
    {
        GameController.instance.State.Paused = false;
        PauseCanvas.gameObject.SetActive( false );
    }

    public void OnClickQuit()
    {
        Time.timeScale = 1.0f;
        Application.LoadLevel( "MainMenu" );
#if UNITY_EDITOR
        //UnityEditor.EditorApplication.Beep();
#else
        Application.Quit();
#endif
    }
}
