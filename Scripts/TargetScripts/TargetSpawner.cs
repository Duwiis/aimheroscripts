using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{

    public bool reflexMode; //disable all target spawners

    public bool easyMode = false;                                       // enables easy mode
    public bool lvl5Started = false;                                    // starts level 5    
    public bool lvl4Started = false;                                    // starts level 4
    public bool lvl3Started = false;                                    // starts level 3    
    public bool lvl2Started = false;                                    // starts level 2
    public bool enemyTargetSpawned = false;                             // has the spawn command started
    public int enemyTargetCount = 0;                                    // amount of enemies on screen

    public EnemyTargetController enemyTargetController;                 // asigns the enemy target controller

    [SerializeField]
    private GameObject _enemyTarget;                                     // assign the enemy target
    [SerializeField]
    private GameObject _targetBase;                                      // define the gameobject for spawning
    [SerializeField]
    private Vector3 middleOfWallTarget = new Vector3(0, 3.75f, 7.00005f);// spawn target in middle of walltarget
    [SerializeField]
    private UiManager _uiManager;                                        // assigns the uiManager   
    [SerializeField]
    private Player player;



    void Start()                // Use this for initialization
    {
        SpawnTargetCommand();    // spawns target in middle of walltarget

    }

    void Update()                                         // Update is called once per frame
    {
        
        if (reflexMode != true)
        {
            LevelController();                             // starts spawning and makes game harder
        }
        

        if (_uiManager.amttargets == 0 && reflexMode != true)  // if ui resets to 0
        {
            StopAllCoroutines();                           // stop all enemy spawn coroutines
        }
    }

    public void SpawnTargetCommand() // invoked in start               // spawn target in middle of walltarget
    {
        /// spawn the target
        Instantiate(_targetBase, middleOfWallTarget, Quaternion.identity);
    }

    public void EnemyTargetSpawner() // invoked in update by LevelController
    {
        /// spawns enemy target in a random location on the wall
        Instantiate(_enemyTarget, new Vector3(Random.Range(-7f, 7f), Random.Range(-2.5f, 7f), middleOfWallTarget.z), Quaternion.identity);
        enemyTargetCount++;                      // increases the enemy target counter
        enemyTargetSpawned = true;               // allows enemytargetenumerator to continue
        StartCoroutine(enemyTargetEnumerator()); // starts enemytargetenumertor
    }

    public void EasyModeEnemyController()
    {
        /// spawns enemy target in a random location on the wall
        Instantiate(_enemyTarget, new Vector3(Random.Range(-7f, 7f), Random.Range(-2.5f, 7f), middleOfWallTarget.z), Quaternion.identity);
        enemyTargetCount++;                      // increases the enemy target counter
        enemyTargetSpawned = true;               // allows enemytargetenumerator to continue
        StartCoroutine(easyModeEnemySpawns());   // starts enemytargetenumertor
    }
    public IEnumerator easyModeEnemySpawns()
    {
        while (enemyTargetSpawned == true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 5f));
            /// spawns a target at a random location on the target wall
            Instantiate(_enemyTarget, new Vector3(Random.Range(-7f, 7f), Random.Range(-2.5f, 7f), middleOfWallTarget.z), Quaternion.identity);
            enemyTargetCount++;                // adds 1 to the enemy target counter
        }
    }

    public IEnumerator enemyTargetEnumerator() // invoked by enemytarget spawner
    {

        while (enemyTargetSpawned == true)     // only runs if a target has been spawned by spawner
        {                                      /// waits between 2 seconds and 4 seconds
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            /// spawns a target at a random location on the target wall
            Instantiate(_enemyTarget, new Vector3(Random.Range(-7f, 7f), Random.Range(-2.5f, 7f), middleOfWallTarget.z), Quaternion.identity);
            enemyTargetCount++;                // adds 1 to the enemy target counter
        }

    }
    public IEnumerator enemyTargetEnumeratorLvl2() // invoked by LevelController in Update
    {
        while (lvl2Started == true && enemyTargetSpawned == true)   // if lvl 2 started and an enemy has spawned
        {
            yield return new WaitForSeconds(Random.Range(.5f, 2f)); // wait between .5 seconds and 2 seconds
                                                                    /// spawn a target at a random point
            Instantiate(_enemyTarget, new Vector3(Random.Range(-7f, 7f), Random.Range(-2.5f, 7f), middleOfWallTarget.z), Quaternion.identity);
            enemyTargetCount++;                                     // increases the target count
        }
    }

    void LevelController() // invoked in update            // controls the level count
    {
        /// no enemy targets on screen and the amount of 
        /// targets hit is 5 and and enemy has not 
        /// and enemy has not spawned
        if (enemyTargetCount == 0 && _uiManager.amttargets == 5 && enemyTargetSpawned == false)
        {
            if (easyMode == true)
            {
                EasyModeEnemyController();
            }
            else
            {
                EnemyTargetSpawner();                      // spawns enemy, increases count, and enemyspawned true        
            }
        }                                                  /// targets hit 10 and level 2 not started      
        if (_uiManager.amttargets == 20 && lvl2Started == false && easyMode == false)
        {
            lvl2Started = true;                            // starts lvl 2             
            StartCoroutine(enemyTargetEnumeratorLvl2());   // spawns an extra enemy at random spawn time

        }                                                  /// targets hit 20, and level 3 not started
        if (_uiManager.amttargets == 40 && lvl3Started == false && easyMode == false)
        {
            lvl3Started = true;                            // starts lvl 3

        }                                                  /// targets hit 30, and level 4 not started
        if (_uiManager.amttargets == 60 && lvl4Started == false && easyMode == false)
        {
            lvl4Started = true;                            // starts lvl 4
        }                                                  /// targets hit 40 and level 5 not started
        if (_uiManager.amttargets == 80 && lvl5Started == false && easyMode == false)
        {
            lvl5Started = true;                            // starts lvl 5
        }
    }
    public void EasyModeStart()
    {
        easyMode = true;
    }

    public void ReflexModeController()
    {
        
        // at a random time between 0 and 3 seconds spawn a enemy target (or make a different target called reflex target but I dont think you have to)
        StartCoroutine(RelexEnemyTargetSpawnEnumerator());
        // when the player hits the enemy target respawn the target base in the midde (so run the spawntarget start command i think)
        // the reflex target should have a lifetime of 4 seconds in the first level
        // after 3 (or 5) perfect hits (the player hit the target before it died) increase the level
        // level 2 should be 3 seconds
        // level 3 should be 2.5 seconds
        // then after level 3 go down .1 seconds
    }
    public IEnumerator RelexEnemyTargetSpawnEnumerator()
    {

        yield return StartCoroutine(ReflexModeEnumerator(Random.Range(.4f, 3f)));
    }

    public IEnumerator ReflexModeEnumerator(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime); //Random.Range(0f, 3f)
        Instantiate(_enemyTarget, new Vector3(Random.Range(-7f, 7f), Random.Range(-2.5f, 7f), middleOfWallTarget.z), Quaternion.identity);
        
        

    }
    
}
