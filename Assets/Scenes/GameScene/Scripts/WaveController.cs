//#define TESTMODE

using System;
using Game;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * HOW TO USE: The prefab with this component attached defines all of the enemies 
 * that will be spawned, and creates pools for them on initialization. 
 */
[UsedImplicitly]
[RequireComponent( typeof( AudioSource ) )]
public class WaveController : MonoBehaviour
{
    private const int MAX_OBJECT_PER_WAVEDEF = 20;

    /*************************************************/
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
        public WaveDefinition( string name, float[] spawn_freq, ObjectPool pool, bool disabled )
        {
            Name = name;

            GameObjectPrefab = Resources.Load<GameObject>( "Enemies/" + name + "" );
            if ( GameObjectPrefab == null ) {
                Debug.LogError( "LoadAssetAtPath returned null" );
            }

            Debug.Assert( spawn_freq.Length == GameConstants.MAX_MULTIPLIER, "[" + name + "] spawn_freq.Length != GameConstants.MAX_MULTIPLIER" );

            SpawnFrequencyByLevel = spawn_freq;
            //LevelFrequencyLow = level_freq_low;
            //LevelFrequencyHigh = level_freq_high;

            Pool = pool;
            Disabled = disabled;
        }
    }

    /*************************************************/
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

            if ( Input.GetKey( KeyCode.LeftAlt ) ) {
                if ( Input.GetKeyDown( KeyCode.Alpha1 ) ) {
                    GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_1X;
                }
                if ( Input.GetKeyDown( KeyCode.Alpha2 ) ) {
                    GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_2X;
                }
                if ( Input.GetKeyDown( KeyCode.Alpha3 ) ) {
                    GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_3X;
                }
                if ( Input.GetKeyDown( KeyCode.Alpha4 ) ) {
                    GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_4X;
                }
                if ( Input.GetKeyDown( KeyCode.Alpha5 ) ) {
                    GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_5X;
                }
                if ( Input.GetKeyDown( KeyCode.Alpha6 ) ) {
                    GameController.instance.GameEnv.Score = GameConstants.SCORE_THRESH_6X;
                }

                if ( Input.GetKeyDown( KeyCode.Q ) ) {
                    GameController.instance.GameEnv.AdjustScore( 75 );
                }

                if ( Input.GetKeyDown( KeyCode.U ) ) {
                    GetPoolForName( "UFO" ).GetInstance();
                }
                if ( Input.GetKeyDown( KeyCode.R ) ) {
                    GetPoolForName( "Seeker" ).GetInstance();
                }
            }
        }

        if ( Time.time >= _next_spawn_time ) {
            // Heartbeat throb
            _audio.Play();
            _next_spawn_time = Time.time + _cur_spawn_timeout;
            SpawnTick();
        }
    }

    /*****************************/
    private void SpawnTick()
    {
        for ( int i = 0; i < Enemies.Length; i++ ) {
            if ( Enemies[ i ].Disabled )
                continue;

            var odds = Enemies[i].SpawnFrequencyByLevel[GameController.instance.GameEnv.Multiplier-1];
            var rand = Random.Range(0.0f, 100.0f);

            Debug.Log( "odds " + odds + " / " + rand );

            if ( rand < odds ) {
                GameObject g = Enemies[ i ].Pool.GetInstance();
            }
            // NOTED: There is a slim chance nothing will spawn.
        }
    }

    /*****************************/
    public void Init()
    {
        Enemies = new[] {
            new WaveDefinition("UFO",
                                    new[] {0.0f, 0.0f, 0.0f, 5.0f, 8.0f, 10.0f },
                                    null, false),
            new WaveDefinition("Asteroid_LG",
                                    new[] {40.0f, 45.0f, 50.0f, 55.0f, 60.0f, 70.0f },
                                    null, false),
            new WaveDefinition("Asteroid_SM",
                                    new[] {40.0f, 45.0f, 50.0f, 55.0f, 60.0f, 70.0f },
                                    null, false)

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