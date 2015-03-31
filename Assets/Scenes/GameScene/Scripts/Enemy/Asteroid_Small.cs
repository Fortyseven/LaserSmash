using Game;
using UnityEngine;

public class Asteroid_Small : BaseAsteroid
{
    protected override int BaseScore { get { return GameConstants.SCORE_ASTEROID_LG; } }

    public bool IsFragment { get; set; }

    /*****************************/
    public override void Awake()
    {
        base.Awake();
        IsFragment = false;
    }

    /*****************************/
    // We're respawned manually by Asteroid_Large, or by the WaveGenerator
    public override void Respawn()
    {
        // Our spawner will take care of positioning us if we're a fragment, otherwise
        // we need to roll our own particle trail
        if ( IsFragment ) {
            //Debug.Log( "SM: Spawned as fragment" );
            SpawnParticleTrail();
        }
        else {
            //Debug.Log( "SM: Spawned as normal small rock" );
            base.Respawn();
        }

        ParticleEmitter p = _particle_trail.GetComponentInChildren<ParticleEmitter>();      

        p.minSize = 0.5f;
        p.maxSize = 1.0f;

        _hit_surface = false;
        IsFragment = false;
        IsReady = true;
    }
}
