using UnityEngine;
using System.Collections;
using Game;

public class Bomb : EnemyType 
{
    const float Y_OFFSET = 14.1f;
    const float Y_OFFSET_FLOOR = 1.0f;
    const float X_OFFSET_MAX = 11.0f;

    public GameObject ExplosionPrefab;
    AudioSource _audio;

    float _base_gravity_mult;
    bool _hit_surface;

    /******************************************************************/
    void Awake () 
    {
        _base_gravity_mult = rigidbody2D.gravityScale;
        _is_ready = false;
        _audio = GetComponent<AudioSource>();
    }
    
    /******************************************************************/
    void Update () 
    {
        if (!_is_ready) return;
        
        // Did we go off screen? Sweep it under the rug.
        if (Mathf.Abs(transform.position.x) > GameConstants.SCREEN_X_BOUNDS ) {
            Done(false);
            return;
        }
        
        // Did we hit the ground? Punish player, make noises, explode
        if ( transform.position.y < Y_OFFSET_FLOOR ) {
            GameController.instance.State.AdjustScore(-(GameConstants.SCORE_BOMB_LG /2));
            _hit_surface = true;
            Done();
            return;
        }

        _audio.pitch = transform.position.y / Y_OFFSET;
    }

    /******************************************************************/
    private void Done(bool explode = true)
    {
        //if (_hit_surface)
        
        rigidbody2D.velocity =  new Vector2(0,0);
        
        if (explode) Instantiate( ExplosionPrefab, transform.position, Quaternion.identity );
        
        _audio.Stop();
        Hibernate();
    }

    /******************************************************************/
    public void HitByLaser( Laserbeam laser )
    {
        GameController.instance.State.AdjustScore(GameConstants.SCORE_BOMB_LG);
        Destroy( laser.gameObject );
        Done();
    }

    /******************************************************************/
    public override void Respawn ()
    {
        Vector3 spawn_pos = new Vector3( Random.Range(-X_OFFSET_MAX, X_OFFSET_MAX), Y_OFFSET,0 );
        transform.position = spawn_pos;

        rigidbody2D.gravityScale = _base_gravity_mult * Random.Range(2.0f, 20.0f);
        _audio.Play();
        _hit_surface = false;
        _is_ready = true;
    }

    /******************************************************************/
    public override void InstaKill ()
    {
        this.gameObject.SetActive(false);
    }
}
