/************************************************************************
** GameTestControls.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using UnityEngine;

namespace Game
{
    public class GameTestControls : MonoBehaviour
    {
        private WaveController _wavecon;
        private ObjectPool[] _pools;

        public void Start()
        {
            Debug.Log( "Game Test Scene Controls Added" );
            //  _PoolAsteroidLG = new ObjectPool( GOAsteroidLarge, 10 );
            _wavecon = GetComponentInChildren<WaveController>();

            _pools = new ObjectPool[ 6 ];

            _pools[ 0 ] = _wavecon.GetPoolForName( "Asteroid Large" );
            _pools[ 1 ] = _wavecon.GetPoolForName( "Asteroid Small" );
            _pools[ 2 ] = _wavecon.GetPoolForName( "UFO" );
            _pools[ 3 ] = _wavecon.GetPoolForName( "Bomb Large" );
            _pools[ 4 ] = _wavecon.GetPoolForName( "Bomb Small" );
            _pools[ 5 ] = _wavecon.GetPoolForName( "Seeker" );
        }

        public void Update()
        {
            if ( Input.GetKeyDown( "1" ) ) {
                _pools[ 0 ].GetInstance();
            }
            if ( Input.GetKeyDown( "2" ) ) {
                _pools[ 1 ].GetInstance();
            }
            if ( Input.GetKeyDown( "3" ) ) {
                _pools[ 2 ].GetInstance();
            }
            if ( Input.GetKeyDown( "4" ) ) {
                _pools[ 3 ].GetInstance();
            }
            if ( Input.GetKeyDown( "5" ) ) {
                _pools[ 4 ].GetInstance();
            }
            if ( Input.GetKeyDown( "6" ) ) {
                _pools[ 5 ].GetInstance();
            }
        }
    }
}