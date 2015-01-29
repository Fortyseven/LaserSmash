using Game;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

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
            SpawnParticleTrail();
        }
        else {
            base.Respawn();
        }

        ParticleEmitter p = _particle_trail.GetComponentInChildren<ParticleEmitter>();

        Debug.Assert( p != null, "p != null" );

        p.minSize = 0.5f;
        p.maxSize = 1.0f;

        _hit_surface = false;
        IsFragment = false;
        IsReady = true;
    }
}
