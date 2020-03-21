using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetController : MonoBehaviour
{
    public bool targetAlive = true;
    public int reflexLevel;

    public GameObject _player;              //assigns the player gameobject // not the prefab!
    public GameObject targetspawner;        //assigns the target gameobject // not the prefab!
    public GameObject _enemyTarget;         //assgins the enemytarget prefab
    public GameObject _uiManager;

    public bool spawnedEnemy = false;       // true if the target spawned an enemy
	
	void Start ()                               // Use this for initialization
    {
        

        _uiManager = GameObject.Find("UI");
        reflexLevel = _uiManager.GetComponent<UiManager>().reflexLvl;
        
        
                                                // assigns the player, not prefab
        _player = GameObject.Find("Player");    ///assigns the target spawner, not prefab
        targetspawner = GameObject.Find("Target Spawner");

        DifficultyIncreases();                  // increase difficulty by making enemies smaller
        SpawnEnemyMultiplierOn();               // spawns enemies closer to player
        GameOver();                             // resets levels and destroys enemies
        if(targetspawner.GetComponent<TargetSpawner>().reflexMode == true)
        {
            

            ReflexModeDestroy();
        }
    }
	void Update ()  // Update is called once per frame
    {
        if (targetAlive == true)
        {
            _uiManager.GetComponent<UiManager>().ReflexTimer();
        }
    }

    private void SpawnEnemyMultiplierOn()               // spawns enemies closer to the player
    {
        if(spawnedEnemy == false && targetspawner.GetComponent<TargetSpawner>().reflexMode == false)                       // if this target has spawned an enemy
        {
            StartCoroutine(EnemyTargetMultiplier());    // wait 2 seconds and spawn another
        }
    }

    IEnumerator EnemyTargetMultiplier() //invoked by spawnenemymultiplierOn, spawns enemies closer to the player
    {                                                   /// if easy mode is enabled 
        if (targetspawner.GetComponent<TargetSpawner>().easyMode == true)
        {
            yield return new WaitForSeconds(4f);        // wait 4 seconds
                                                        /// spawn an enemy at a random point, closer to the player
            Instantiate(_enemyTarget, new Vector3(Random.Range(-7f, 7f), Random.Range(-2.5f, 7f), this.gameObject.transform.position.z - 1f), Quaternion.identity);
                                                        /// increases the enemy counter
            targetspawner.GetComponent<TargetSpawner>().enemyTargetCount++;
            spawnedEnemy = true;                        // so that the target cannot spawn more than 1 enemy
        }       
        else                                            // if easy mode is disabled
        {                   
            yield return new WaitForSeconds(2f);        // wait 2 seconds
                                                        /// spawn an enemy at random point, closer to the player
            Instantiate(_enemyTarget, new Vector3(Random.Range(-7f, 7f), Random.Range(-2.5f, 7f), this.gameObject.transform.position.z - 1f), Quaternion.identity);
                                                        /// increases the enemy counter
            targetspawner.GetComponent<TargetSpawner>().enemyTargetCount++;
            spawnedEnemy = true;                        // so that the target cannot spawn more than 1 enemy
        }
    }

    public void DestroyAllEnemies()// invoked in GameOver
    {
        //stop the timer
        _uiManager.GetComponent<UiManager>().reflexSecondsCount = 0;

        GameObject[] enemys = GameObject.FindGameObjectsWithTag("EnemyTarget"); // finds all enemy targets

        foreach (GameObject enemy in enemys)
        { 
            Destroy(enemy);                                                     // destroys them
        }
    }

    void DifficultyIncreases() // invoked at start, makes enemies smaller as more targets are hit
    {
                                                /// if lvl 3 started
        if (targetspawner.GetComponent<TargetSpawner>().lvl3Started == true)
        {                                       /// make the enemy targets smaller
            gameObject.transform.localScale = new Vector3(.9f, .9f, .9f);
        }                                       /// lvl 4 started
        if (targetspawner.GetComponent<TargetSpawner>().lvl4Started == true)
        {                                       /// make the targets even smaller
            gameObject.transform.localScale = new Vector3(.8f, .8f, .8f);
        }                                       /// lvl 5 started
        if (targetspawner.GetComponent<TargetSpawner>().lvl5Started == true)
        {                                       /// even smaller
            gameObject.transform.localScale = new Vector3(.7f, .7f, .7f);
        }
    }

    void GameOver() // invoked in start
    {
        if (transform.position.z < 3)           // if a target reaches a certain point restart the game
        {                                       // resets the hit counter
            _player.GetComponent<Player>().targetsHit = 0;
            spawnedEnemy = false;               // stops the target from spawning another enemy
                                                /// resets the enemy target spawner so it spawns at 5 hits
            targetspawner.GetComponent<TargetSpawner>().enemyTargetSpawned = false;
                                                /// resets the enemy target count
            targetspawner.GetComponent<TargetSpawner>().enemyTargetCount = 0;
                                                /// all below resets levels to stop coroutines
            targetspawner.GetComponent<TargetSpawner>().lvl2Started = false;
            targetspawner.GetComponent<TargetSpawner>().lvl3Started = false;
            targetspawner.GetComponent<TargetSpawner>().lvl4Started = false;
            targetspawner.GetComponent<TargetSpawner>().lvl5Started = false;
            DestroyAllEnemies();                // destroys all the enemy targets
        }
    }
    void ReflexModeDestroy()
    {
        Debug.Log("ReflexModeDestroy");
        // we need coroutines to destroy this gameobject after a certain amount of time
        // so at lvl 1 the target will destroy itself in 4 seconds
        StartCoroutine(TimerReflexModeDestroy());

    }
    IEnumerator TimerReflexModeDestroy()
    {
        if (reflexLevel == 1)
        {
            Debug.Log("LVL1 Enemy");
            yield return new WaitForSeconds(4f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 2)
        {
            Debug.Log("LVL2 Enemy");
            yield return new WaitForSeconds(3f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 3)
        {
            Debug.Log("LVL3 Enemy");
            yield return new WaitForSeconds(2.8f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 4)
        {
            Debug.Log("LVL4 Enemy");
            yield return new WaitForSeconds(2.6f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 5)
        {
            Debug.Log("LVL5 Enemy");
            yield return new WaitForSeconds(2.4f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 6)
        {
            Debug.Log("LVL6 Enemy");
            yield return new WaitForSeconds(2.2f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 7)
        {
            Debug.Log("LVL7 Enemy");
            yield return new WaitForSeconds(2.0f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 8)
        {
            Debug.Log("LVL8 Enemy");
            yield return new WaitForSeconds(1.8f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 9)
        {
            Debug.Log("LVL9 Enemy");
            yield return new WaitForSeconds(1.6f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 10)
        {
            Debug.Log("LVL10 Enemy");
            yield return new WaitForSeconds(1.4f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 11)
        {
            Debug.Log("LVL11 Enemy");
            yield return new WaitForSeconds(1.2f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 12)
        {
            Debug.Log("LVL12 Enemy");
            yield return new WaitForSeconds(1.0f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if(reflexLevel == 13)
        {
            Debug.Log("LVL13 Enemy");
            yield return new WaitForSeconds(.9f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 14)
        {
            Debug.Log("LVL14 Enemy");
            yield return new WaitForSeconds(.8f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 15)
        {
            Debug.Log("LVL16 Enemy");
            yield return new WaitForSeconds(.7f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 16)
        {
            Debug.Log("LVL16 Enemy");
            yield return new WaitForSeconds(.6f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 17)
        {
            Debug.Log("LVL17 Enemy");
            yield return new WaitForSeconds(.5f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 18)
        {
            Debug.Log("LVL18 Enemy");
            yield return new WaitForSeconds(.4f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 19)
        {
            Debug.Log("LVL19 Enemy");
            yield return new WaitForSeconds(.39f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 20)
        {
            Debug.Log("LVL20 Enemy");
            yield return new WaitForSeconds(.38f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 21)
        {
            Debug.Log("LVL21 Enemy");
            yield return new WaitForSeconds(.37f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 22)
        {
            Debug.Log("LVL22 Enemy");
            yield return new WaitForSeconds(.36f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 23)
        {
            Debug.Log("LVL23 Enemy");
            yield return new WaitForSeconds(.35f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 24)
        {
            Debug.Log("LVL24 Enemy");
            yield return new WaitForSeconds(.34f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 25)
        {
            Debug.Log("LVL25 Enemy");
            yield return new WaitForSeconds(.33f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 26)
        {
            Debug.Log("LVL26 Enemy");
            yield return new WaitForSeconds(.32f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 27)
        {
            Debug.Log("LVL27 Enemy");
            yield return new WaitForSeconds(.31f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 28)
        {
            Debug.Log("LVL28 Enemy");
            yield return new WaitForSeconds(.30f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 29)
        {
            Debug.Log("LVL29 Enemy");
            yield return new WaitForSeconds(.29f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 30)
        {
            Debug.Log("LVL30 Enemy");
            yield return new WaitForSeconds(.28f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 31)
        {
            Debug.Log("LVL31 Enemy");
            yield return new WaitForSeconds(.27f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 32)
        {
            Debug.Log("LVL32 Enemy");
            yield return new WaitForSeconds(.26f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 33)
        {
            Debug.Log("LVL33 Enemy");
            yield return new WaitForSeconds(.25f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 34)
        {
            Debug.Log("LVL34 Enemy");
            yield return new WaitForSeconds(.24f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 35)
        {
            Debug.Log("LVL35 Enemy");
            yield return new WaitForSeconds(.23f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 36)
        {
            Debug.Log("LVL36 Enemy");
            yield return new WaitForSeconds(.22f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 37)
        {
            Debug.Log("LVL37 Enemy");
            yield return new WaitForSeconds(.21f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }
        if (reflexLevel == 38)
        {
            Debug.Log("LVL38 Enemy");
            yield return new WaitForSeconds(.20f);
            targetspawner.GetComponent<TargetSpawner>().SpawnTargetCommand();
            _player.GetComponent<Player>().reflexCounter = 0;
            DestroyAllEnemies();
        }

    }
}


