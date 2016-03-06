using UnityEngine;
using System.Collections;

/// <summary>
/// Lerps a target camera between a collection of GameObject coordinates
/// </summary>
public class NavPoints : MonoBehaviour
{
    [Range(0.1f, 5.0f)]
    public float CameraMoveSpeed = 1.0f;

    public GameObject[] NavPointObjects;
    public Camera TargetCamera;

    private Vector3 _fromPos = Vector3.zero, _toPos = Vector3.zero;
    private Quaternion _fromRot = Quaternion.identity, _toRot = Quaternion.identity;
    private bool _in_transit = false;

    // Use this for initialization
    void Start()
    {
        if ( TargetCamera == null ) {
            throw new UnityException( "No target camera supplied to NavPoint component" );
        }

        MoveToNavPoint( 0 );
    }

    //// Update is called once per frame
    //void Update()
    //{
    //}

    public void MoveToNavPoint( int i )
    {
        if ( i >= NavPointObjects.Length ) {
            throw new UnityException( "Request to move to invalid NavPoint (" + i + ")" );
        }

        if ( _in_transit ) return;

        // Have we ever moved? If not, this is our home.
        if ( _fromPos == Vector3.zero ) {
            _fromPos = NavPointObjects[ i ].transform.position;
            TargetCamera.transform.position = _fromPos;
            return;
        }

        // If we're already at this navpoint, ignore the request
        if ( TargetCamera.transform.position == NavPointObjects[ i ].transform.position ) return;

        _fromPos = TargetCamera.transform.position;
        _fromRot = TargetCamera.transform.rotation;

        _toPos = NavPointObjects[ i ].transform.position;
        _toRot = NavPointObjects[ i ].transform.rotation;

        StartCoroutine( "CoRoutine_LerpCamera" );
    }

    public IEnumerator CoRoutine_LerpCamera()
    {
        float t = 0;

        Vector3 pos;
        Quaternion rot;

        _in_transit = true;

        while ( t < 1.0f ) {
            t += Time.deltaTime * CameraMoveSpeed;

            pos = Vector3.Lerp( _fromPos, _toPos, t );
            rot = Quaternion.Lerp( _fromRot, _toRot, t ); ;

            TargetCamera.transform.position = pos;
            TargetCamera.transform.rotation = rot;
            yield return 0;
        }

        _in_transit = false;
    }
}
