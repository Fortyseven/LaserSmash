﻿using System.Collections.Generic;
using Game;
using UnityEngine;

//public class NewObjectPool
//{
//    interface IPoolItem
//    {
//        void InstaKill();
//        void Respawn();
//    }

//    public class PooledGameObject
//    {
//        public void InstaKill()
//        {
//            ;
//        }

//        public void Respawn()
//        {
//            ;
//        }
//    }

//    private List<PooledGameObject> _object_pool;
//    private PooledGameObject _prototype;
//    private int _max_items;
//    public int Length { get; private set; }

//    public ObjectPool(GameObject prototype, int max_items = 0)
//    {
//        Length = max_items;
//        _prototype = (PooledGameObject) prototype;
//        _object_pool = new List<PooledGameObject>(Length);

//    }

//    public GameObject GetInstance(Vector3 position, Quaternion rot, bool call_respawn = true)
//    {
//        for (int i = 0; i < _object_pool.Count; i++) {
//            if (!_object_pool[i].gameObject.activeInHierarchy)
//            {
//                GameObject spawnee = _object_pool[i];

//                spawnee.transform.position = position;
//                spawnee.transform.rotation = rot;
//                spawnee.SetActive(true);
//                if (call_respawn) ((IPoolItem)spawnee).Respawn();
//                return spawnee;
//            }
//        }
//    }
//}


//TODO: Rewrite objectpool in a way that doesn't use slow, awful, horrible SendMessage calls

public class ObjectPool
{
    public GameObject GameObjectSource;
    public int MaxCount;
    //ArrayList _items;
    private List<GameObject> _items;

    /*****************************/
    public ObjectPool( GameObject gobj, int max_count)
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
    public GameObject GetInstance()
    {
        return GetInstance( new Vector3( 0, 0, 0 ), Quaternion.identity );
    }

    /*****************************/
    public GameObject GetInstance( Vector3 position, Quaternion rot, bool call_respawn = true )
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
