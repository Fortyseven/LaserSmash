using UnityEngine;
using UnityEditor;

public class DropAll : ScriptableObject
{
    [MenuItem( "Custom/DropAll" )]
    public static void DropAllObjects()
    {
        Transform[] all = Selection.transforms;
        
        for ( int i = 0; i < all.Length; i++ ) {

            //Check the base first
            if ( all[ i ].collider != null )
                CheckCollider( all[ i ] );

            //Then, check the children

            foreach ( Transform child in all[ i ] ) {
                CheckCollider( child );
            }
        }
    }

    static void CheckCollider( Transform input )
    {
        RaycastHit[] hits = 
                Physics.RaycastAll( new Vector3( input.position.x, input.position.y - input.gameObject.collider.bounds.size.y / 2, input.position.z ),
                                    -Vector3.up,
                                    1000.0F );
        int z = 0;
        while ( z < hits.Length ) {
            RaycastHit hit = hits[ z ];
            z++;
            input.Translate( new Vector3( 0, 0, hit.distance ) );
        }
    }
}