using UnityEngine;

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
