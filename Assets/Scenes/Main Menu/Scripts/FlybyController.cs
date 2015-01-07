using UnityEngine;

public class FlybyController : MonoBehaviour
{
    public GameObject[] Meshes;
    int _cur_mesh;

    /************************************************/
    // Use this for initialization
    public void Start()
    {
        _cur_mesh = -1;
        NextMesh();
    }

    /************************************************/
    void NextMesh()
    {
        _cur_mesh++;

        for ( int i = 0; i < Meshes.Length; i++ ) {
            if ( i == ( _cur_mesh % Meshes.Length ) ) {
                Meshes[ i ].SetActive( true );
            }
            else {
                Meshes[ i ].SetActive( false );
            }
        }
    }

    /************************************************/
    public void OnAnimationEnd()
    {
        NextMesh();
    }

}
