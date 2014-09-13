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

    WaveController _wavecon;

    ObjectPool[] _pools;

    // Use this for initialization
    void Start()
    {
        _PoolAsteroidLG = new ObjectPool(GOAsteroidLarge, 10);
        _wavecon = GetComponentInChildren<WaveController>();

        Debug.Log(_wavecon);

        _pools = new ObjectPool[5];

        _pools[0] = _wavecon.GetPoolForName("Asteroid Large");
        _pools[1] = _wavecon.GetPoolForName("Asteroid Small");
        _pools[2] = _wavecon.GetPoolForName("UFO");
        _pools[3] = _wavecon.GetPoolForName("Bomb Large");
        _pools[4] = _wavecon.GetPoolForName("Asteroid Large");

    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            _pools[0].GetInstance();
        }
        if (Input.GetKeyDown("2")) {
            _pools[1].GetInstance();
        }
        if (Input.GetKeyDown("3")) {
            _pools[2].GetInstance();
        }
        if (Input.GetKeyDown("4")) {
            _pools[3].GetInstance();
        }
        if (Input.GetKeyDown("5")) {
            _pools[4].GetInstance();
        }
        if (Input.GetKeyDown("6")) {
            _pools[5].GetInstance();
        }
    }

//    void SpawnAsteroidLarge()
//    {
//        Vector3 where = new Vector3( Random.Range( -GameConstants.SCREEN_X_BOUNDS, GameConstants.SCREEN_X_BOUNDS ), GameConstants.SCREEN_Y_GEN_OFFSET, 0 );
//        //Instantiate( GOAsteroidLarge, where, Quaternion.identity );
//        _PoolAsteroidLG.GetInstance(where, Quaternion.identity);
//    }
//
//    void SpawnAsteroidSmall()
//    {
//        Vector3 where = new Vector3( Random.Range( -GameConstants.SCREEN_X_BOUNDS, GameConstants.SCREEN_X_BOUNDS ), GameConstants.SCREEN_Y_GEN_OFFSET, 0 );
//        Instantiate( GOAsteroidSmall, where, Quaternion.identity );
//    }
//
//    void SpawnUFO()
//    {
//        Vector3 where = new Vector3( 0,0,0 );
//        Instantiate( GOUFO, where, Quaternion.identity );
//    }
//
//    void SpawnBombLg()
//    {
//        Instantiate(GOBombLG, new Vector3(Random.Range(-GameConstants.SCREEN_X_BOUNDS,GameConstants.SCREEN_X_BOUNDS), GameConstants.SCREEN_Y_GEN_OFFSET, 0), Quaternion.identity);
//    }
//
//    void SpawnBombSm()
//    {
//        Instantiate(GOBombSM, new Vector3(Random.Range(-GameConstants.SCREEN_X_BOUNDS,GameConstants.SCREEN_X_BOUNDS), GameConstants.SCREEN_Y_GEN_OFFSET, 0), Quaternion.identity);
//    }
//
//    void SpawnSatellite()
//    {
//        Debug.Log("Not implemented.");
//    }
}
