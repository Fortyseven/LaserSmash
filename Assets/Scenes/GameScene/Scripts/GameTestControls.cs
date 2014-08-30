using UnityEngine;
using System.Collections;
using Game;

public class GameTestControls : MonoBehaviour
{
//    /public GameObject[] EnemyObjects;
    public GameObject GOAsteroidLarge;
    public GameObject GOAsteroidSmall;
    public GameObject GOUFO;
    public GameObject GOBombLG;
    public GameObject GOBombSM;
    public GameObject GOSatellite;


    ObjectPool _PoolAsteroidLG;


    // Use this for initialization
    void Start()
    {
        _PoolAsteroidLG = new ObjectPool(GOAsteroidLarge, 10);
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
        //Instantiate( GOAsteroidLarge, where, Quaternion.identity );
        _PoolAsteroidLG.GetInstance(where, Quaternion.identity);
    }

    void SpawnAsteroidSmall()
    {
        Vector3 where = new Vector3( Random.Range( -GameConstants.SCREEN_X_BOUNDS, GameConstants.SCREEN_X_BOUNDS ), GameConstants.SCREEN_Y_GEN_OFFSET, 0 );
        Instantiate( GOAsteroidSmall, where, Quaternion.identity );
    }

    void SpawnUFO()
    {
        Vector3 where = new Vector3( 0,0,0 );
        Instantiate( GOUFO, where, Quaternion.identity );
    }

    void SpawnBombLg()
    {
        Instantiate(GOBombLG, new Vector3(Random.Range(-GameConstants.SCREEN_X_BOUNDS,GameConstants.SCREEN_X_BOUNDS), GameConstants.SCREEN_Y_GEN_OFFSET, 0), Quaternion.identity);
    }

    void SpawnBombSm()
    {
        Instantiate(GOBombSM, new Vector3(Random.Range(-GameConstants.SCREEN_X_BOUNDS,GameConstants.SCREEN_X_BOUNDS), GameConstants.SCREEN_Y_GEN_OFFSET, 0), Quaternion.identity);
    }

    void SpawnSatellite()
    {
        Debug.Log("Not implemented.");
    }
}
