﻿using UnityEngine;
using System.Collections;

public class ObjectPool
{

    public GameObject GameObjectSource;
    public int MaxCount;
    ArrayList _items;

    public ObjectPool( GameObject gobj, int max_count )
    {
        GameObjectSource = gobj;
        MaxCount = max_count;
        Awake();
    }

    void Awake()
    {
        _items = new ArrayList( 0 );
    }

    public GameObject GetInstance()
    {
        return GetInstance( new Vector3( 0, 0, 0 ), Quaternion.identity );
    }

    public GameObject GetInstance( Vector3 position, Quaternion rot )
    {
        // Have we hit max allocation? Instantiate, add to the pool, and return
        if ( _items.Count < MaxCount ) {
            Debug.Log(string.Format("Creating new; currently have {0} allocated out of {1}.", _items.Count, MaxCount));
            GameObject obj = UnityEngine.GameObject.Instantiate( GameObjectSource, position, rot ) as GameObject;
            _items.Add( obj );
            return obj;
        }
        for ( int i = 0; i < _items.Count; i++ ) {
            GameObject obj = (GameObject)_items[ i ];
            if ( !obj.activeInHierarchy ) {
                obj.transform.position = position;
                obj.transform.rotation = rot;
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }
}
