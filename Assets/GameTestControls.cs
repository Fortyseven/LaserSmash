using UnityEngine;
using System.Collections;

public class GameTestControls : MonoBehaviour
{
    public GameObject[] EnemyObjects;

    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            Debug.Log("spawn");
        }
    }
}
