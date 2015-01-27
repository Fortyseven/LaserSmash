using Game;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class Asteroid_Small : BaseAsteroid
{
    protected override int BaseScore { get { return GameConstants.SCORE_ASTEROID_LG; } }

    public bool IsFragment { get; set; }

    /*****************************/
    protected override void Awake()
    {
        base.Awake();
        IsFragment = false;
    }

    /*****************************/
    public override void Respawn()
    {
        // Our spawner will take care of positioning us if we're a fragment
        if ( !IsFragment ) {
            base.Respawn();

            //_gravity_multiplier = Random.Range( 0, 5.0f );
            //rigidbody2D.gravityScale = _base_gravityscale * _gravity_multiplier;
        }

        _particle_trail = Instantiate( ParticleEmitterPrefab, transform.position, Quaternion.identity ) as GameObject;

        Debug.Assert( _particle_trail != null, "_particle_trail != null" );

        ParticleEmitter p = _particle_trail.GetComponentInChildren<ParticleEmitter>();

        Debug.Assert( p != null, "p != null" );

        p.minSize = 0.5f;
        p.maxSize = 1.0f;

        _hit_surface = false;
        IsFragment = false;
        IsReady = true;
    }

    /*****************************/
    public void RespawnFragment()
    {
        IsFragment = true;
        Respawn();
    }

    /*****************************/
}
