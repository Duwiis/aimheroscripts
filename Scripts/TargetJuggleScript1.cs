using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetJuggleScript1 : MonoBehaviour
{
    public Rigidbody _rigidbodyJuggleBig;

    public bool _powerMode = false;

    public float _forceXBig = 2f;
    public float _forceYBig = 6f;
    public float _forceZBig = 0f;
    public float _forceDownBig = -6f;
    public float _forcePull = -2f;
    public float _forcePush = 2f;
    public bool negativeZ = false;

    public float _speed = 5f;

	// Use this for initialization
	void Start ()
    {
        
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _forcePush *= 10;
            _forcePull *= 10;
            _forceDownBig *= 10;
            _forceXBig *= 10;
            _powerMode = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && _powerMode == true)
        {
            _forcePush /= 10;
            _forcePull /= 10;
            _forceDownBig /= 10;
            _forceXBig /= 10;
            _powerMode = false;
        }
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
        

        _rigidbodyJuggleBig.AddForce(0f, _forceYBig, 0f, ForceMode.Impulse);
    }
    public void JuggleHitLeftX()
    {
        _rigidbodyJuggleBig.AddForce(Camera.main.transform.right * -_forceXBig);
    }
    public void JuggleHitRightX()
    {
        
        _rigidbodyJuggleBig.AddForce(Camera.main.transform.right * _forceXBig);
    }
    public void JuggleHitDown()
    {
       
        _rigidbodyJuggleBig.AddForce(0f, _forceDownBig, 0f, ForceMode.Impulse);
    }
    public void JuggleHitPull()
    {
        _rigidbodyJuggleBig.AddForce(Camera.main.transform.forward * _forcePull);
    }
    public void JuggleHitPush()
    {
        _rigidbodyJuggleBig.AddForce(Camera.main.transform.forward * _forcePush);
    }


    public void ApplyForce(Rigidbody body)
    {
        // Vector3 direction = body.transform.position - transform.position;
        //body.AddForceAtPosition(direction.normalized, transform.position);

    }
}
