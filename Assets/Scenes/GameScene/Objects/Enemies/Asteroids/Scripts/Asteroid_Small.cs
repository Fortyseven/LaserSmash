using UnityEngine;
using System.Collections;
using Game;

public class Asteroid_Small : EnemyType
{   
    private const float Y_SPAWN_OFFSET = 15.5f;
    private const float MAX_X_OFFSET = -12f;
    private const float MIN_X_OFFSET = 12f;

    public AudioClip SoundHitSurface = null;
    private AudioSource _audio = null;
    
    public GameObject ExplosionPrefab = null;
    public GameObject HitSurfacePrefab = null;
    public GameObject PlayerObject = null;
    public GameObject AsteroidSmallPrefab = null;

    public GameObject ParticleEmitterPrefab = null;

    private float _gravity_multiplier = 0.0f;

    private GameObject _particle_trail = null;

    bool _hit_surface;

    private float _base_gravityscale;

    private bool _is_fragment = false;

    public bool IsFragment {
        set {
            _is_fragment = value;
        }
    }

    /*****************************/
    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _base_gravityscale = rigidbody2D.gravityScale;
        _is_fragment = false;
        _is_ready = false;
    }       

    /*****************************/
    public override void Respawn()
    {
        // Our spawner will take care of positioning us if we're a fragment
        if (!_is_fragment) {
            Vector3 start_pos = new Vector3(Random.Range(MIN_X_OFFSET, MAX_X_OFFSET), Y_SPAWN_OFFSET, 0);
            transform.position = start_pos;
            
            _gravity_multiplier = Random.Range(0, 5.0f);
            rigidbody2D.gravityScale = _base_gravityscale * _gravity_multiplier;
        }
        
        _particle_trail = Instantiate(ParticleEmitterPrefab, transform.position, Quaternion.identity) as GameObject;
        
        ParticleEmitter p = _particle_trail.GetComponentInChildren<ParticleEmitter>();
        p.minSize = 0.5f;
        p.maxSize = 1.0f;
        
        _hit_surface = false;
        _is_fragment = false;
        _is_ready = true;
    }
    
    public void RespawnFragment()
    {
        _is_fragment = true;
        Respawn();
    }

    /*****************************/
    void Update()
    {
        if (!_is_ready) return;

        // Did we go off screen? Sweep it under the rug.
        if (Mathf.Abs(transform.position.x) > GameConstants.SCREEN_X_BOUNDS ) {
            Done(false);
            return;
        }
        
        // Did we hit the ground? Punish player, make noises, explode
        if ( transform.position.y < GameConstants.SCREEN_Y_FLOOR ) {
            GameController.instance.State.AdjustScore(-(GameConstants.SCORE_ASTEROID_SM/2));
            _hit_surface = true;
            Done();
            return;
        }

        if (_particle_trail != null)
            _particle_trail.transform.position = this.transform.position;
    }

    /*****************************/
    private void Done(bool explode = true)
    {
        //if (_hit_surface)

        rigidbody2D.velocity =  new Vector2(0,0);

        if (explode) Instantiate( ExplosionPrefab, transform.position, Quaternion.identity );

        if (_particle_trail != null) {
            Destroy(_particle_trail, 1.0f);
        }

        Hibernate();
    }

    /*****************************/
    public void HitByLaser( Laserbeam laser )
    {
        GameController.instance.State.AdjustScore(GameConstants.SCORE_ASTEROID_SM);
        Destroy( laser.gameObject );
        Done();
    }

    public override void InstaKill ()
    {
        Done(false);
    }

}
