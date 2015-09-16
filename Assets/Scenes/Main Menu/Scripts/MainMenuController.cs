using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public void Awake()
    {
        Init.Construct(true);
    }

    public void Menu_PLAY()
    {
        Init.instance.ChangeState( Init.GameStates.GAME_LOOP );
    }

    public void Menu_HELP()
    {
        Init.instance.ChangeState( Init.GameStates.HELP );
    }
}
