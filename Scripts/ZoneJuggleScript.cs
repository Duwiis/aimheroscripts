using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneJuggleScript : MonoBehaviour
{
    public float rangeOfZoneY = 2;
    public float speedOfZone = 3;
    public int direction = 1;
    public int juggleTimer;

    public float zoneForce;

    public float juggleScore;

    public float secondsCount;

    public float xForce;
    public float yForce;
    


    public GameObject respawnScript;

    // Use this for initialization
    void Start ()
    {
        respawnScript = GameObject.Find("JuggleTargetSpawner");

        
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        
        xForce = Random.Range(-0.5f, 0.5f);
        yForce = Random.Range(-0.5f, 0.5f);
        ScorePositionChange();

        if(juggleScore > 20)
        {
            Lvl1DifficultyIncrease();
        }
        if(juggleScore > 40)
        {
            Lvl2DifficultyIncrease();
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody)
        {
            other.attachedRigidbody.AddForce(new Vector3(xForce, yForce) * zoneForce);
        }
        // for every second inside zone score goes up
        secondsCount += Time.deltaTime;
        juggleScore += Time.deltaTime; 

    }

    void ScorePositionChange()
    {
        if (secondsCount > 5)
        {
            secondsCount = 0;
            transform.position = new Vector3(Random.Range(-7.3f, 7.3f), Random.Range(-3.2f, 9f), 7.5f);
        }
    }

    void Lvl1DifficultyIncrease()
    {
        gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
    }

    void Lvl2DifficultyIncrease()
    {
        

        if (transform.position.x >= 7.3f)
        {
            direction *= -1;
        }
        if (transform.position.x <= -7.3f)
        {
            direction *= -1;
        }
        float movementX = speedOfZone * Time.deltaTime * direction;
        transform.position += new Vector3(movementX, 0, 0);
        //gameObject.transform.position.x = new Vector3 (Mathf.PingPong(Time.deltaTime * speedOfZone, 7)
        /*if (gameObject.transform.position.x <= -7.3f)
        {
            gameObject.transform.position = new Vector3(Mathf.PingPong(Time.deltaTime * speedOfZone, 3), transform.position.y, transform.position.z);

        }

        if(gameObject.transform.position.x >= 7.3f)
        {
            gameObject.transform.position = Vector3.left * Time.deltaTime;
        }*/
    }

    // increase the score by 10 for each second inside the zone
    // after it reaches 50 change position but keep the score
}
