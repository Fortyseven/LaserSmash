using UnityEngine;
using System.Collections;

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
    }

    public WaveDefinition[] Waves;

//    private ObjectPool[] _pools;

    float _next_spawn_time;

    public WaveController( GameObject parent_go )
    {
    }

    /*****************************/
    public void Awake()
    {
        CreatePools();
        Reset();
    }

    /*****************************/
    public void Reset()
    {
        _next_spawn_time = Time.time + 0.25f;
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
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Waves[0].Pool.GetInstance();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Waves[1].Pool.GetInstance();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Waves[2].Pool.GetInstance();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            Waves[3].Pool.GetInstance();
        }

#endif

        if (Time.time <= _next_spawn_time) return;

        _next_spawn_time = Time.time + 0.25f;

        for(int i = 0; i < Waves.Length; i++) {
            float chance = Random.Range(Waves[i].LevelFrequencyLow[0], Waves[i].LevelFrequencyHigh[0]);

            Debug.Log(chance + "% chance of " + Waves[i].Name);
            if (Random.Range(0, 100) <= chance) {
                GameObject g = Waves[i].Pool.GetInstance();
                if (g == null) break;
            }
        }
    }
   
}
