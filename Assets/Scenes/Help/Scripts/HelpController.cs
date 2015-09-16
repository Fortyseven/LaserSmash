using UnityEngine;

public class HelpController : MonoBehaviour
{

    public void Update()
    {
        if ( Input.anyKeyDown ) {
            Init.instance.ChangeState( Init.GameStates.MAIN_MENU );
        }
    }
}
