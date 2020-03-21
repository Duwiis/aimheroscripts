using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneJuggleBigMapScript : MonoBehaviour
{
    

    public float juggleScore;

    public float secondsCount;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        ScorePositionChange();
    }
    private void OnTriggerStay(Collider other)
    {
        
        // for every second inside zone score goes up
        secondsCount += Time.deltaTime;
        juggleScore += Time.deltaTime;

    }

    void ScorePositionChange()
    {
        if (secondsCount > .01f)
        {
            secondsCount = 0;
            transform.position = new Vector3(Random.Range(-33f, 33f), Random.Range(-5f, 20f), Random.Range(-33,33f));
        }
    }
}
