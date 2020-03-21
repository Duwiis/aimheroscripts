using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggleWallBounceScript : MonoBehaviour
{
    public float wallForce;
    public float wallForceCornerZ;
    public float wallForceCornerX;
    public bool xWall;
    public bool zWall;
    public bool cornerwall;
    public bool yWall;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    private void OnCollisionEnter(Collision other)
    {
        if(zWall == true && cornerwall != true)
        {
            other.rigidbody.AddForce(0, 0, wallForce, ForceMode.VelocityChange);
        }
        if(xWall == true && cornerwall != true)
        {
            other.rigidbody.AddForce(wallForce, 0, 0, ForceMode.VelocityChange);
        }
        if(cornerwall == true)
        {
            other.rigidbody.AddForce(wallForceCornerX, 0, wallForceCornerZ, ForceMode.VelocityChange);
        }
        if(yWall == true)
        {
            other.rigidbody.AddForce(0, wallForce, 0, ForceMode.VelocityChange);
        }
    }
}
