using UnityEngine;

namespace Game
{
    public class CameraEvents : MonoBehaviour
    {
        public void OnIntroCameraDone()
        {
            GameController.instance.OnSceneIntroAnimationComplete();
        }
    }
}