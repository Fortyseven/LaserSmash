using UnityEngine;
using System.Collections;

public class HelpController : MonoBehaviour
{

    public void Update()
    {
        if ( Input.anyKeyDown ) {
            Application.LoadLevel( "MainMenu" );
        }
    }
}
