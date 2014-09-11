using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public Text BlinkyBit;

    // Use this for initialization
    void Start()
    {
        StartCoroutine( "Flash" );
    }

    public IEnumerator Flash()
    {
        while ( enabled ) {
            BlinkyBit.gameObject.SetActive( !BlinkyBit.gameObject.activeInHierarchy );
            yield return new WaitForSeconds( 0.5f );
        }
    }   
    // Update is called once per frame
    void Update()
    {

        if ( Input.anyKeyDown ) {
            Application.LoadLevel( "Game" );
        }
    }
}
