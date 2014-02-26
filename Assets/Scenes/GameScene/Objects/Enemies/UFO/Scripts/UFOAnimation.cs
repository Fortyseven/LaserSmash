using UnityEngine;
using System.Collections;

public class UFOAnimation : MonoBehaviour
{
    float c = 0.0f;
    float pos_x = 0.0f;
    int pos_dir = 1;
    
    void Update()
    {
        //transform.Rotate(Vector3.up, 1.0f );
        
        transform.rotation = Quaternion.Euler( Mathf.Sin( c * 3f ) * 3.0f, c * 20, 0 );
        c += Time.deltaTime;
        
        Vector3 pos = transform.position;
        
        pos_x += pos_dir * Time.deltaTime;
        if (Mathf.Abs(pos_x) > 2.0f) pos_dir = -pos_dir;
        pos.Set( pos_x, 0, 0 ); 
        
        transform.position = pos;
        
        GetComponent<AudioSource>().pan = pos_x;
        
    }
}
