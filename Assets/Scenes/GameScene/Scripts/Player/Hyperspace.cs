/************************************************************************
** Hyperspace.cs
**
** Copyright (c) 2015, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent( typeof( StateMachine ) )]
    public class Hyperspace : MonoBehaviour
    {
        public enum BEAM_STATE
        {
            IDLE,
            BEAM_UP,
            BEAM_DOWN
        };

        public BEAM_STATE State { get; private set; }

        private struct MatState
        {
            public Material original_material;
            public Material material;

            public MatState( Material mat )
            {
                original_material = new Material( mat ); // new instance
                material = mat;
            }
        }

        private List<MatState>  _mat_states;
        private MeshRenderer[] _meshes;

        public void Start()
        {
            State = BEAM_STATE.IDLE;
            _meshes = GetComponentsInChildren<MeshRenderer>();
            _mat_states = new List<MatState>( _meshes.Length );

            // Ensure each material is an instance
            for ( int i = 0; i < _meshes.Length; i++ ) {
                MeshRenderer rend = _meshes[ i ];
                Material m = ( rend.material = new Material( rend.material ) );
                _mat_states.Add( new MatState( m ) );
            }
        }

        public void Update()
        {
            foreach ( MatState m in _mat_states ) {
                //m.material.SetColor( "_Color", Color.green );
                m.material.SetColor( "_EmissionColorUI", Color.cyan );
                m.material.SetFloat( "_EmissionScaleUI", 10.0f );
            }
        }

        public void StartBeaming()
        {
            StartCoroutine( "Co_BeamUp" );
        }

        public IEnumerator Co_BeamUp()
        {
            State = BEAM_STATE.BEAM_UP;
            for ( float i = 0; i < 1.0f; i += 0.01f ) {
                yield return new WaitForSeconds( 0.01f );
            }
            yield return null;
        }

        public IEnumerator Co_BeamDown()
        {
            State = BEAM_STATE.BEAM_DOWN;
            for ( float i = 0; i < 1.0f; i++ ) {
                yield return new WaitForSeconds( 0.01f );
            }
            State = BEAM_STATE.IDLE;
            yield return null;
        }
    }
}