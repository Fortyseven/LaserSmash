using UnityEngine;
using System.Collections;

public class CameraEvents : MonoBehaviour
{
    public void OnIntroCameraDone()
    {
        GameController.instance.OnSceneIntroAnimationComplete();
    }
}
