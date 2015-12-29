/************************************************************************
** MainMenuController.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using UnityEngine;

namespace Game
{
    public class MainMenuController : MonoBehaviour
    {
        public void Awake()
        {
            Init.Construct( true );
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
}
