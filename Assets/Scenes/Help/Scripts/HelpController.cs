/************************************************************************
** HelpController.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using UnityEngine;

namespace Game
{
    public class HelpController : MonoBehaviour
    {
        public void Start()
        {
            Init.Construct( true );
        }

        public void Update()
        {
            if ( Input.anyKeyDown ) {
                Init.instance.ChangeState( Init.GameStates.MAIN_MENU );
            }
        }
    }
}
