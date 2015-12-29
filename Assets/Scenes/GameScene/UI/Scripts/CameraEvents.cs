/************************************************************************
** CameraEvents.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

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