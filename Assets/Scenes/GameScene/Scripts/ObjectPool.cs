/************************************************************************
** ObjectPool.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ObjectPool
    {
        public GameObject GameObjectSource;
        public int MaxCount;
        //ArrayList _items;
        private List<GameObject> _items;

        /*****************************/
        public ObjectPool( GameObject gobj, int max_count )
        {
            GameObjectSource = gobj;
            MaxCount = max_count;
            Awake();
        }

        /*****************************/
        void Awake()
        {
            _items = new List<GameObject>( 0 );
        }

        /*****************************/
        public void Reset()
        {
            foreach ( GameObject g in _items ) {
                if ( g.activeInHierarchy )
                    g.SendMessage( "InstaKill", SendMessageOptions.DontRequireReceiver );
            }
        }

        /*****************************/
        public GameObject SpawnInstance()
        {
            return SpawnInstance( new Vector3( 0, 0, 0 ), Quaternion.identity );
        }

        /*****************************/
        public GameObject SpawnInstance( Vector3 position, Quaternion rot, bool call_respawn = true )
        {
            // Have we hit max allocation? Instantiate, add to the pool, and return
            if ( _items.Count < MaxCount ) {
                GameObject obj = GameObject.Instantiate( GameObjectSource, position, rot ) as GameObject;
                if ( obj != null ) {
                    if ( call_respawn )
                        obj.GetComponent<GenericEnemy>().Respawn();
                    //obj.SendMessage( "Respawn", SendMessageOptions.DontRequireReceiver );
                    _items.Add( obj );
                    return obj;
                }

                Debug.Log( "Instantiation failed" );
            }
            // Find an inactive gameobject
            for ( int i = 0; i < _items.Count; i++ ) {
                GameObject obj = _items[ i ];
                if ( !obj.activeInHierarchy ) {
                    obj.transform.position = position;
                    obj.transform.rotation = rot;
                    obj.SetActive( true );
                    if ( call_respawn )
                        obj.GetComponent<GenericEnemy>().Respawn();
                    //obj.SendMessage( "Respawn", SendMessageOptions.DontRequireReceiver );
                    return obj;
                }
            }

            // Worst case
            return null;
        }
    }
}