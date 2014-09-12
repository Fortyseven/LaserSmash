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

    private ObjectPool[] _pools;

    public WaveController( GameObject parent_go )
    {
    }

    public void Awake()
    {
        CreatePools();
        Reset();
    }

    public void Reset()
    {

    }

    public void Update()
    {
        for(int i = 0; i < Waves.Length; i++) {
            GameObject g = Waves[i].Pool.GetInstance();
            if (g == null) break;
        }
    }

    void CreatePools()
    {
        _pools = new ObjectPool[Waves.Length];
        for(int i = 0; i < Waves.Length; i++) {
            Waves[i].Pool = new ObjectPool(Waves[i].GameObjectPrefab, MAX_OBJECT_PER_WAVEDEF);
        }
    }

}
