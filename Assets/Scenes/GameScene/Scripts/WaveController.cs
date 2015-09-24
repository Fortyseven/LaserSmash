//#define TESTMODE

using System;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * HOW TO USE: The prefab with this component attached defines all of the enemies 
 * that will be spawned, and creates pools for them on initialization. 
 */
[RequireComponent( typeof( AudioSource ) )]
public class WaveController : MonoBehaviour
{
    public const int MAX_OBJECT_PER_WAVEDEF = 20;

    [Serializable]
    public struct WaveDefinition
    {
        public readonly string Name;                 // Textual description of enemy
        public readonly GameObject GameObjectPrefab; // Prefab associated with enemy 
        public readonly float[] SpawnFrequencyByLevel;   // random % chance of spawning per level

        //        public readonly float[] LevelFrequencyLow;   // Lower bound of random % chance of spawning per level
        //        public readonly float[] LevelFrequencyHigh;  // Upper bound of random % chance of spawning per level
        public ObjectPool Pool;             // not exposed in editor //TODO:Should this be private?
        public bool Disabled;               // Will disable this enemy for debugging purposes

        //public WaveDefinition( string name, string asset_path, float[] level_freq_low, float[] level_freq_high, ObjectPool pool, bool disabled )
        public WaveDefinition( string name, string asset_path, float[] spawn_freq, ObjectPool pool, bool disabled )
        {
            Name = name;

            GameObjectPrefab = Resources.Load<GameObject>( "Enemies/" + asset_path + "" );
            if ( GameObjectPrefab == null ) {
                Debug.LogError( "LoadAssetAtPath returned null" );
            }

            SpawnFrequencyByLevel = spawn_freq;
            //LevelFrequencyLow = level_freq_low;
            //LevelFrequencyHigh = level_freq_high;

            Pool = pool;
            Disabled = disabled;
        }
    }

    private WaveDefinition[] Enemies;

    public bool Paused { get; set; }

    private AudioSource _audio;

    private float _cur_spawn_timeout;
    private float _next_spawn_time;
    private bool _initialized = false;

    /*****************************/
    public void Reset()
    {
        if ( !_initialized )
            throw new UnityException( "Reset called on non-initalized WaveCon" );

        _cur_spawn_timeout = GameConstants.MULT_TIMEOUT_RAMP[ 0 ];
        _next_spawn_time = Time.time + _cur_spawn_timeout;
        for ( int i = 0; i < Enemies.Length; i++ ) {
            Enemies[ i ].Pool.Reset();
        }
    }

    /*****************************/
    void CreatePools()
    {
        for ( int i = 0; i < Enemies.Length; i++ ) {
            Enemies[ i ].Pool = new ObjectPool( Enemies[ i ].GameObjectPrefab, MAX_OBJECT_PER_WAVEDEF );
        }
    }

    /*****************************/
    public ObjectPool GetPoolForName( string pool_name )
    {
        if ( pool_name == null )
            throw new ArgumentNullException( "pool_name" );

        for ( int i = 0; i < Enemies.Length; i++ ) {
            if ( Enemies[ i ].Name.Equals( pool_name ) ) {
                return Enemies[ i ].Pool;
            }
        }
        throw new UnityException( "Could not provide object pool for " + pool_name );
    }

    /*****************************/
    public void Update()
    {
        if ( Paused || !_initialized )
            return;

        _cur_spawn_timeout = GameConstants.MULT_TIMEOUT_RAMP[ GameController.instance.GameEnv.Multiplier - 1 ];

        //TODO: Invert this for production
        if ( !GameController.instance.DebugMode ) {

            if ( Input.GetKeyDown( KeyCode.Alpha1 ) ) {
                //GameController.instance.State.Multiplier = 1;
                GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_1X;
            }
            if ( Input.GetKeyDown( KeyCode.Alpha2 ) ) {
                //GameController.instance.State.Multiplier = 2;
                GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_2X;
            }
            if ( Input.GetKeyDown( KeyCode.Alpha3 ) ) {
                //GameController.instance.State.Multiplier = 3;
                GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_3X;
            }
            if ( Input.GetKeyDown( KeyCode.Alpha4 ) ) {
                //GameController.instance.State.Multiplier = 4;
                GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_4X;
            }
            if ( Input.GetKeyDown( KeyCode.Alpha5 ) ) {
                //GameController.instance.State.Multiplier = 5;
                GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_5X;
            }
            if ( Input.GetKeyDown( KeyCode.Alpha6 ) ) {
                //GameController.instance.State.Multiplier = 6;
                GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_6X;
            }

            if ( Input.GetKeyDown( KeyCode.U ) ) {
                GetPoolForName( "UFO" ).GetInstance();
            }
        }

        return;

        if ( Time.time >= _next_spawn_time ) {
            // Heartbeat throb
            _audio.Play();
            _next_spawn_time = Time.time + _cur_spawn_timeout;
            SpawnTick();
        }
    }

    private void SpawnTick()
    {
        for ( int i = 0; i < Enemies.Length; i++ ) {
            if ( Enemies[ i ].Disabled )
                continue;
            //float chance = Random.Range( Enemies[ i ].LevelFrequencyLow[ 0 ], Enemies[ i ].LevelFrequencyHigh[ 0 ] );


            //TODO: Level in SpawnFrequencyByLevel?!
            if ( Random.Range( 0.0f, 100.0f ) < Enemies[ i ].SpawnFrequencyByLevel[ 0 ] ) {
                GameObject g = Enemies[ i ].Pool.GetInstance();

                // We've hit max spawned items; no more left in pool
                if ( g != null )
                    return;
            }

            // NOTED: There is a slim chance nothing will spawn.
        }
    }

    public void Init()
    {
        Enemies = new[] {
            new WaveDefinition("UFO", "UFO",
                                        new[] {100.0f, 100.0f, 1.0f, 1.0f, 1.0f },
                                        null, false),
            //new WaveDefinition("UFO", "UFO", 
            //                            new[] {100.0f, 100.0f, 1.0f, 1.0f, 1.0f }, 
            //                            new[] {100.0f, 100.0f, 100.0f, 1.0f, 1.0f }, 
            //                            null, false),
            //new WaveDefinition("foo", null, new float[5] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, new float[5] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, null, false),
            //new WaveDefinition("foo", null, new float[5] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, new float[5] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, null, false),
            //new WaveDefinition("foo", null, new float[5] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, new float[5] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, null, false),
            //new WaveDefinition("foo", null, new float[5] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, new float[5] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, null, false),
            //new WaveDefinition("foo", null, new float[5] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, new float[5] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, null, false)
        };
        _audio = GetComponent<AudioSource>();
        CreatePools();
        Paused = true;
        _initialized = true;
        Reset();
    }
}