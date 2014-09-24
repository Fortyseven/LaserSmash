using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public void Menu_PLAY()
    {
        Application.LoadLevel( "Game" );
    }

    public void Menu_HELP()
    {
        Application.LoadLevel("Help");
    }
}
