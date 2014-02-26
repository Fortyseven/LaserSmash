using UnityEngine;
using System.Collections;

public class WaveGenerator : MonoBehaviour
{
    public GameObject AsteroidLarge = null;
    public GameObject AsteroidSmall = null;
    public GameObject SpinnerBombLarge = null;
    public GameObject SpinnerBombSmall = null;
    private  bool mInitialized = false;
    private  GameControl mGame = null;

    // temp
    public GameObject UFOPrefab = null;
    private GameObject my_ufo = null;

    public void init( GameControl game_control )
    {
        mGame = game_control;
        mInitialized = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if ( !mInitialized )
            return;
        
        if ( Input.GetKeyDown( "b" ) ) {
            SpawnBomb();
        }

        if (Input.GetKeyDown("u")) {
            if (my_ufo == null) {
                my_ufo = Instantiate(UFOPrefab, new Vector3(0,0), Quaternion.identity) as GameObject;
            } else {
                Destroy(my_ufo);
                my_ufo = null;
            }
        }


        if ( true ) {
            if ( Random.Range( 0, 1000 ) > 990 )
                SpawnAsteroidLarge();
        }
    }
        
    void SpawnAsteroidLarge()
    {
        Vector3 where = new Vector3( Random.Range( -1.5f, 1.5f ), 1.5f, 0 );
        Instantiate( AsteroidLarge, where, Quaternion.identity );
    }


    void SpawnBomb()
    {
        Instantiate(SpinnerBombLarge, new Vector3(Random.Range(-1.0f,1.0f), 1.0f, 0), Quaternion.identity);
    }
}
