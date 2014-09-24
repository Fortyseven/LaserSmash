using UnityEngine;
using System.Collections;

public class FlybyController : MonoBehaviour
{
    public GameObject[] Meshes;
    int cur_mesh;

    /************************************************/
    // Use this for initialization
    void Start()
    {
        cur_mesh = -1;
        NextMesh();
    }
    
    /************************************************/
    // Update is called once per frame
    void Update()
    {
    
    }

    /************************************************/
    void NextMesh()
    {
        cur_mesh++;

        for ( int i = 0; i < Meshes.Length; i++ ) {
            if ( i == (cur_mesh % Meshes.Length) ) {
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
