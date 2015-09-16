using Game;
using UnityEngine;

//TODO: Add [1] and [9] pause easter egg

public class PauseController : MonoBehaviour
{
    private const string INPUT_PAUSE = "Pause";

    public Canvas PauseCanvas;

    public void Awake()
    {
        PauseCanvas.gameObject.SetActive( false );
    }

    public void Update()
    {
        if ( Input.GetButtonDown( INPUT_PAUSE ) ) {
            //FIXME
            //if ( GameController.instance.Status.Paused ) {
            //    OnPauseLeave();
            //}
            //else {
            //    OnPauseEnter();
            //}
        }
    }

    private void OnPauseEnter()
    {
        //GameController.instance.Status.Paused = true;
        //PauseCanvas.gameObject.SetActive( true );

        //GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
        //for ( int i = 0; i < enemies.Length; i++ ) {
        //    enemies[ i ].GetComponent<GenericEnemy>().OnPause();
        //}
    }

    private void OnPauseLeave()
    {
        //GameController.instance.Status.Paused = false;
        //PauseCanvas.gameObject.SetActive( false );

        //GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
        //for ( int i = 0; i < enemies.Length; i++ ) {
        //    enemies[ i ].GetComponent<GenericEnemy>().OnResume();
        //}
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
