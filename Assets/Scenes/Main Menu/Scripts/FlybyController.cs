/************************************************************************
** FlybyController.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using UnityEngine;

namespace Game
{
    public class FlybyController : MonoBehaviour
    {
        public GameObject[] Meshes;
        private int _cur_mesh;

        /************************************************/
        // Use this for initialization
        public void Start()
        {
            _cur_mesh = -1;
            NextMesh();
        }

        /************************************************/

        private void NextMesh()
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
}