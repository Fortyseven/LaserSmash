using System;
using System.Text;
using UnityEngine;
using UnityEditor;

public class BT_UnusedAssetReport : ScriptableObject
{
    private static string[] _existing_assets;

    [MenuItem( "Tools/Bytes Templar/Generate Unused Asset Report" )]
    static void DoIt()
    {
        _existing_assets = UnityEditor.AssetDatabase.GetAllAssetPaths();
        GameObject[] _gameobjects = UnityEditor.Tools.FindObjectsOfType<GameObject>();
        foreach ( GameObject go in _gameobjects ) {
            var comps = go.GetComponents( typeof( Component ) );
            Debug.Log( go.name + " -----------------" );
            foreach ( Component n in comps ) {
                CheckComponentForReferences( n );
            }
        }
    }

    private static void CheckComponentForReferences( Component n )
    {
        Debug.Log( "  # " + n.GetType() );

        // Skip some obvious, numerous non-candidates
        if ( n.GetType() == typeof( Transform ) )
            return;

        var properties = n.GetType().GetProperties();
        foreach ( var prop in properties ) {
            try {
                string prop_name = prop.Name;

                if ( prop_name.Equals( "tag" ) || prop_name.Equals( "name" ) )
                    continue;
                Type prop_type = prop.GetType();

                if (
                    ( prop.GetType() == typeof( Matrix4x4 ) ) ||
                    ( prop.GetType() == typeof( Vector2 ) ) ||
                    ( prop.GetType() == typeof( Vector3 ) ) ||
                    ( prop.GetType() == typeof( Vector4 ) ) ||
                    ( prop.GetType() == typeof( Single ) ) ||
                    ( prop.GetType() == typeof( Double ) ) ||
                    ( prop.GetType() == typeof( Color ) )
                ) {
                    continue;
                }
                object val = n.GetType().GetProperty( prop_name ).GetValue( n, null );
                Debug.Log( prop.ReflectedType + " XXX" );
                Debug.Log( "       |" + prop + " = " + val );
            }
            catch ( System.NotSupportedException ns ) {
                // Eat it
            }
            catch ( Exception e ) {
                Debug.Log( "EXCEPT: " + e );
            }
        }
    }
}