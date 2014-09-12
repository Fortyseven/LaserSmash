using UnityEngine;
using System.Collections;

public abstract class EnemyType : MonoBehaviour
{
    public abstract void Respawn();
    public abstract void InstaKill();

    protected bool _is_ready = false;

    public void Hibernate()
    {
        gameObject.SetActive( false );
        transform.position.Set( 0, 0, 0 );
        _is_ready = false;
    }
}
