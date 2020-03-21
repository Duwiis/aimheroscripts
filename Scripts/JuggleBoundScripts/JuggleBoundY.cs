using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggleBoundY : MonoBehaviour
{
    public float boundForce;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody)
        {
            other.attachedRigidbody.AddForce(Vector3.down * boundForce);
        }
    }
}
