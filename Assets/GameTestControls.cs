using UnityEngine;
using System.Collections;
using Game;

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
            SpawnAsteroidLarge();
        }
        if (Input.GetKeyDown("2")) {
            SpawnAsteroidSmall();
        }
        if (Input.GetKeyDown("3")) {
            SpawnUFO();
        }
        if (Input.GetKeyDown("4")) {
            SpawnBombLg();
        }
        if (Input.GetKeyDown("5")) {
            SpawnBombSm();
        }
        if (Input.GetKeyDown("6")) {
            SpawnSatellite();
        }
    }

    void SpawnAsteroidLarge()
    {
        Vector3 where = new Vector3( Random.Range( -GameConstants.SCREEN_X_BOUNDS, GameConstants.SCREEN_X_BOUNDS ), GameConstants.SCREEN_Y_GEN_OFFSET, 0 );
        Instantiate( EnemyObjects[0], where, Quaternion.identity );
    }

    void SpawnAsteroidSmall()
    {
        Vector3 where = new Vector3( Random.Range( -GameConstants.SCREEN_X_BOUNDS, GameConstants.SCREEN_X_BOUNDS ), GameConstants.SCREEN_Y_GEN_OFFSET, 0 );
        Instantiate( EnemyObjects[1], where, Quaternion.identity );
    }


    void SpawnUFO()
    {
        Vector3 where = new Vector3( Random.Range( -GameConstants.SCREEN_X_BOUNDS, GameConstants.SCREEN_X_BOUNDS ), GameConstants.SCREEN_Y_GEN_OFFSET, 0 );
        Instantiate( EnemyObjects[2], where, Quaternion.identity );
    }

    void SpawnBombLg()
    {
        Instantiate(EnemyObjects[3], new Vector3(Random.Range(-GameConstants.SCREEN_X_BOUNDS,GameConstants.SCREEN_X_BOUNDS), GameConstants.SCREEN_Y_GEN_OFFSET, 0), Quaternion.identity);
    }

    void SpawnBombSm()
    {
        Instantiate(EnemyObjects[4], new Vector3(Random.Range(-GameConstants.SCREEN_X_BOUNDS,GameConstants.SCREEN_X_BOUNDS), GameConstants.SCREEN_Y_GEN_OFFSET, 0), Quaternion.identity);
    }

    void SpawnSatellite()
    {
        Debug.Log("Not implemented.");
    }
}
