using System;
using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour
{
    public void Init<T>()
    {
        if ( !typeof( T ).IsEnum )
            throw new UnityException( "State generic is not enum" );

        Debug.Log( Enum.GetValues( typeof( T ) ) );


    }
    public void Update()
    {

    }
}
