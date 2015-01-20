using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public bool isLeftPressed = false;
    public bool isRightPressed = false;

    public void RightDown()
    {
        isRightPressed = true;
    }

    public void RightUp()
    {
        isRightPressed = false;
    }   
}
