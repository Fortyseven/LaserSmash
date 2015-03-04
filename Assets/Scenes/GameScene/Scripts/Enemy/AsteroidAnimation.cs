using UnityEngine;

public class AsteroidAnimation : MonoBehaviour
{
    private float _m_x_rot_speed = 0;
    //    private float mYRotSpeed = 0;

    void Start()
    {
        Color c = gameObject.GetComponent<Renderer>().material.color;

        c.r = Random.Range( 0.1f, 0.6f );
        c.g = Random.Range( 0.1f, 0.6f );
        c.b = Random.Range( 0.1f, 0.6f );

        gameObject.GetComponent<Renderer>().material.color = c;

        _m_x_rot_speed = Random.Range( -90.0f, 90.0f );
        //mYRotSpeed = Random.Range (-mXRotSpeed, mXRotSpeed);

        //transform.localScale.Scale (new Vector3(Random.Range(0.05f,2.0f),1.0f,1.0f));
    }


    // Update is called once per frame
    void Update()
    {
        //transform.Rotate (Vector3.left, mXRotSpeed * Time.deltaTime);
        transform.Rotate( Vector3.up, _m_x_rot_speed * Time.deltaTime );
    }
}