//#define TESTMODE

using UnityEngine;
using System.Collections;
using Game;

public class WaveController : MonoBehaviour
{
    public const int MAX_OBJECT_PER_WAVEDEF = 20;

    [System.Serializable]
    public class WaveDefinition {
        public string Name;
        public GameObject GameObjectPrefab;
        public float[] LevelFrequencyLow;
        public float[] LevelFrequencyHigh;
        public ObjectPool Pool;
        public bool Disabled;
    }

    public WaveDefinition[] Waves;

    bool _is_paused = false;
    float _curSpawnTimeout;

    public bool Paused {
        set { _is_paused = value; }
    }

    float _next_spawn_time;

    /*****************************/
    public void Awake()
    {
        CreatePools();
        Reset();
    }

    /*****************************/
    public void Reset()
    {
        _curSpawnTimeout = GameConstants.MULT_TIMEOUT_RAMP[0];
        _next_spawn_time = Time.time + _curSpawnTimeout ;
        for(int i = 0; i < Waves.Length; i++) {
            Waves[i].Pool.Reset();
        }
    }

    /*****************************/
    void CreatePools()
    {
//        _pools = new ObjectPool[Waves.Length];
        for(int i = 0; i < Waves.Length; i++) {
            Waves[i].Pool = new ObjectPool(Waves[i].GameObjectPrefab, MAX_OBJECT_PER_WAVEDEF);
        }
    }

    /*****************************/
    public ObjectPool GetPoolForName( string name )
    {
        for(int i = 0; i < Waves.Length; i++) {
            if (Waves[i].Name.Equals(name)) {
                return Waves[i].Pool;
            }
        }
        throw new UnityException("Could not provide object pool for " + name);
    }

    /*****************************/
    public void Update()
    {
#if !TESTMODE
        if (_is_paused) return;
        _curSpawnTimeout = GameConstants.MULT_TIMEOUT_RAMP[GameController.instance.State.Multiplier-1];
#endif

//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.Alpha1)) {
//            Waves[0].Pool.GetInstance();
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha2)) {
//            Waves[1].Pool.GetInstance();
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha3)) {
//            Waves[2].Pool.GetInstance();
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha4)) {
//            Waves[3].Pool.GetInstance();
//        }
//
//#else
#if !TESTMODE
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            GameController.instance.State.Multiplier = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            GameController.instance.State.Multiplier = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            GameController.instance.State.Multiplier = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            GameController.instance.State.Multiplier = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            GameController.instance.State.Multiplier = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            GameController.instance.State.Multiplier = 6;
        }
#endif
//#endif


        if (Time.time <= _next_spawn_time) return;

        _next_spawn_time = Time.time + _curSpawnTimeout;

        for(int i = 0; i < Waves.Length; i++) {
            if (Waves[i].Disabled) continue;
            float chance = Random.Range(Waves[i].LevelFrequencyLow[0], Waves[i].LevelFrequencyHigh[0]);

            if (Random.Range(0, 100) <= chance) {
                GameObject g = Waves[i].Pool.GetInstance();
                if (g == null) break;
            }
        }
    }
   
}