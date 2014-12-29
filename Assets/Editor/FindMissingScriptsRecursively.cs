using UnityEngine;
using UnityEditor;

public class FindMissingScriptsRecursively : EditorWindow
{
    static int _go_count = 0, _components_count = 0, _missing_count = 0;

    [MenuItem( "Window/FindMissingScriptsRecursively" )]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow( typeof( FindMissingScriptsRecursively ) );
    }

    public void OnGUI()
    {
        if ( GUILayout.Button( "Find Missing Scripts in selected GameObjects" ) ) {
            FindInSelected();
        }
    }

    private static void FindInSelected()
    {
        GameObject[] go = Selection.gameObjects;
        _go_count = 0;
        _components_count = 0;
        _missing_count = 0;
        foreach ( GameObject g in go ) {
            FindInGO( g );
        }
        Debug.Log( string.Format( "Searched {0} GameObjects, {1} components, found {2} missing", _go_count, _components_count, _missing_count ) );
    }

    private static void FindInGO( GameObject g )
    {
        _go_count++;
        Component[] components = g.GetComponents<Component>();
        for ( int i = 0; i < components.Length; i++ ) {
            _components_count++;
            if ( components[ i ] == null ) {
                _missing_count++;
                string s = g.name;
                Transform t = g.transform;
                while ( t.parent != null ) {
                    s = t.parent.name + "/" + s;
                    t = t.parent;
                }
                Debug.Log( s + " has an empty script attached in position: " + i, g );
            }
        }
        // Now recurse through each child GO (if there are any):
        foreach ( Transform child_t in g.transform ) {
            //Debug.Log("Searching " + childT.name  + " " );
            FindInGO( child_t.gameObject );
        }
    }
}