using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetJuggleScript : MonoBehaviour
{
    public Rigidbody _rigidbodyJuggle;

    public float _forceX = 0f;
    public float _forceY = 0f;
    public float _forceZ = 0f;
    public float _forceDown = 0f;

    public Vector3 _forceVector3 = new Vector3(1f, 1f, 1f);

    public float _thrust;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
       //_rigidbodyJuggle.AddRelativeForce(Vector3.up * _thrust);
        // when this target is hit
        // it moves up a certain amount

        // if the target passes the to o the target wall reset it

        // if the target passes the right or left side of hte target wall reset it

        // if the target drops below the bottom of the target wall reset it

        // the player scores points if the target is inside a certain area of the target wall
    }

    public void JuggleHitByPlayer()
    {
        _rigidbodyJuggle.AddForce(0f, _forceY, 0f, ForceMode.Impulse);
    }
    public void JuggleHitLeftX()
    {
        _rigidbodyJuggle.AddForce(-_forceX, 0f, 0f, ForceMode.Impulse);
    }
    public void JuggleHitRightX()
    {
        _rigidbodyJuggle.AddForce(_forceX, 0f, 0f, ForceMode.Impulse);
    }
    public void JuggleHitDown()
    {
        _rigidbodyJuggle.AddForce(0f, _forceDown, 0f, ForceMode.Impulse);
    }


    public void ApplyForce(Rigidbody body)
    {
        // Vector3 direction = body.transform.position - transform.position;
        //body.AddForceAtPosition(direction.normalized, transform.position);

    }
}
