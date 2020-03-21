using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public bool juggleBigMode = false;

    public bool juggleMode = false;
    public int juggleScoreVal = 0;
    public GameObject _juggleZone;

    public string reflexAnimal;
    public int reflexLvl = 1;


    public float displaySens;
    public GameObject sensitivitySetter;
    public TextMeshPro sensitivity3dUi;
    public int highscorenumber;            // high score
    public bool gamestart = false;         // starts timer
    public int amttargets;                 // int for amount of targets hit
    public bool gamePaused = true;

    public float secondsCount;             // float for amount of real time
    public float reflexSecondsCount;             // float for amount of real time
    private int minuteCount;               // minutes past
    private int hourCount;                 // hours past

    [SerializeField]
    private Player Player;       // assigns player script
    [SerializeField]
    private TextMeshPro _targetsHit3D;     // assigns 3dui for targets hit
    [SerializeField]
    private TextMeshPro _timer3D;          // assigns 3dui for timer
    [SerializeField]
    private TextMeshProUGUI _targetsHitUi; // assigns canvasUi for targets hit
    [SerializeField]
    private TargetSpawner _targetSpawner;  // assigns the target spawner
    [SerializeField]
    private TextMeshPro _highScore;        // assigns high score 3dui
    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private GameObject _easymodeButton;
    [SerializeField]
    private TextMeshPro _reflexRank;
    [SerializeField]
    private TextMeshPro _reflexTimer;
    [SerializeField]
    private TextMeshPro _reflexStreak;
    [SerializeField]
    private TextMeshPro _3dUiJuggleScore;



    void Start () // Use this for initialization
    {
        
            //_juggleZone = GameObject.Find("ZoneJuggle");
        
        _pausePanel.SetActive(true);
        PausedGame();
    }
	
	
	void Update ()    // Update is called once per frame
    {
        if(juggleMode == true)
        {
            _juggleZone = GameObject.Find("ZoneJuggle");
            juggleScoreVal = Mathf.RoundToInt(_juggleZone.GetComponent<ZoneJuggleScript>().juggleScore * 10);
            _3dUiJuggleScore.text = "Juggle Score: " + juggleScoreVal;
        }
        

        Update3dUi(); // updates 3dui on target wall
        UpdateTimer3D();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_pausePanel.activeInHierarchy)
            {
                PausedGame();
            }
        }
        ReflexLevelAnimal();
        _reflexRank.text = reflexAnimal;
        

    }
    public void ReflexTimer()
    {
        reflexSecondsCount += Time.deltaTime;

        _reflexTimer.text = reflexSecondsCount + "s";
        // if the player hits an enemy target reset the timer
    }

    public void UpdateTimer3D()                    //starts the timer 
    {
        secondsCount += Time.deltaTime;     // secondscount = time
                                            /// timer3dUi displays time
        _timer3D.text = hourCount + "h:" + minuteCount + "m:" + (float)secondsCount + "s";

       

        if (secondsCount >= 60)             // if 60 seconds pass
        {
            minuteCount++;                  // add 1 minute
            secondsCount = 0;               // reset the seconds counter
        }
        else if (minuteCount >= 60)         // if minute passes 60
        {
            hourCount++;                    // add 1 hour
            minuteCount = 0;                // reset the minute count
        }
    }

    void Update3dUi()                                          // updates 3dui on target wall
    {
        _reflexStreak.text = "Reflex Streak: " + Player.reflexCounter;

        amttargets = Player.targetsHit;              // amount of targets hit by player
        _targetsHitUi.text = "Targets Hit: " + amttargets;     // updates canvasUi with amount of targets
        _targetsHit3D.text = "Targets Hit: " + amttargets;     // updates 3Dui with amount of targets

        if (amttargets > highscorenumber)                       // if targets hit is > highscore
        {
            highscorenumber++;                                 // increase the high score by 1
            _highScore.text = "HighScore: " + highscorenumber; // show that on the 3dui
        }

        
    }


    private void PausedGame()
    {
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
        Player._lockMovement = true;
        //disable scripts that still work while timescale is set to 0
        Cursor.visible = true;                         // unhides the cursor
        Cursor.lockState = CursorLockMode.None;        // unlocks the cursor
        Player._lockMovement = true;
        gamePaused = true;

    }
    private void ContinueGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
        //enable scripts again
        if (Player.clickerMode == false)
        {
            Cursor.visible = false;                       // hides the cursor
            Cursor.lockState = CursorLockMode.Locked;     // locks the cursor
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Player._lockMovement = false;
            
        }

        Player._lockMovement = false;
        
    }

    public void ResumePlay()
    {
        gamePaused = false;
        if(Player.clickerMode == false)
        {
            Cursor.visible = false;                       // hides the cursor
            Cursor.lockState = CursorLockMode.Locked;     // locks the cursor
        }

        Player._lockMovement = false;
        _pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ExitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }
    public void EasyModeEnabler()
    {
        if(_easymodeButton.activeInHierarchy == false)
        {
            _targetSpawner.easyMode = true;
            _easymodeButton.SetActive(true);
        }
        else
        {
            _targetSpawner.easyMode = false;
            _easymodeButton.SetActive(false);
        }
    }
    public void SensitivityButton()
    {
            sensitivity3dUi.text = Player.GetComponentInChildren<Look_X>().parseSenesitivity.ToString();
    }

    public void ClickerModeButton()
    {
        SceneManager.LoadScene("Clicker_Scene");
    }

    public void FpsModeButton()
    {
        SceneManager.LoadScene("SinglePlayer_AimBattle");
        Debug.Log("Clicked the button");
        
    }
    public void ReflexModeButton()
    {
        SceneManager.LoadScene("AimBattle_Reflex");
        
    }
    public void ClickerReflexModeButton()
    {
        SceneManager.LoadScene("Clicker_Scene_Reflex");
    }
    public void JuggleModeButton()
    {
        SceneManager.LoadScene("AimBattle_Juggle2");
    }
    public void ClickerJuggleModeButton()
    {
        SceneManager.LoadScene("Clicker_Scene_Juggle2");
    }
    public void JuggleModeTestButton()
    {
        SceneManager.LoadScene("AimBattle_JuggleBigMap");
    }

    public void ReflexLevelAnimal()
    {
        if(reflexLvl == 1)
        {
            reflexAnimal = "Coral";
        }
        if (reflexLvl == 2)
        {
            reflexAnimal = "Slug";
        }
        if (reflexLvl == 3)
        {
            reflexAnimal = "Giant Galapago Tortoise";
        }
        if (reflexLvl == 4)
        {
            reflexAnimal = "Tiger Beetle";
        }
        if (reflexLvl == 5)
        {
            reflexAnimal = "Komodo Dragon";
        }
        if (reflexLvl == 6)
        {
            reflexAnimal = "Hyena";
        }
        if (reflexLvl == 7)
        {
            reflexAnimal = "Tiger";
        }
        if (reflexLvl == 8)
        {
            reflexAnimal = "Kangaroo";
        }
        if (reflexLvl == 9)
        {
            reflexAnimal = "Shark";
        }
        if (reflexLvl == 10)
        {
            reflexAnimal = "Greyhound";
        }
        if (reflexLvl == 11)
        {
            reflexAnimal = "Blackbuck";
        }
        if (reflexLvl == 12)
        {
            reflexAnimal = "Springbok";
        }
        if (reflexLvl == 13)
        {
            reflexAnimal = "Swordfish";
        }
        if (reflexLvl == 14)
        {
            reflexAnimal = "Ostrich";
        }
        if (reflexLvl == 15)
        {
            reflexAnimal = "Cheetah";
        }
        if (reflexLvl == 16)
        {
            reflexAnimal = "Grey-headed Albatross";
        }
        if (reflexLvl == 17)
        {
            reflexAnimal = "Black Marlin";
        }
        if (reflexLvl == 18)
        {
            reflexAnimal = "Spur-winged Goose";
        }
        if(reflexLvl == 19)
        {
            reflexAnimal = "Rock Dove";
        }
        if (reflexLvl == 20)
        {
            reflexAnimal = "Golden Eagle";
        }
        if (reflexLvl == 21)
        {
            reflexAnimal = "F1 RaceCar";
        }
        if(reflexLvl == 22)
        {
            reflexAnimal = "Peregrine Falcon";
        }
        if (reflexLvl == 23)
        {
            reflexAnimal = "V150";
        }
        if (reflexLvl == 24)
        {
            reflexAnimal = "Challenger 2";
        }
        if (reflexLvl == 25)
        {
            reflexAnimal = "Rare Bear";
        }
        if (reflexLvl >= 26)
        {
            reflexAnimal = "SR-71A Blackbird";
        }
        if (reflexLvl == 27)
        {
            reflexAnimal = "X-15A-2";
        }
        if (reflexLvl == 28)
        {
            reflexAnimal = "Super Roadrunner";
        }
        if (reflexLvl == 29)
        {
            reflexAnimal = "HTV-2";
        }
        if (reflexLvl == 30)
        {
            reflexAnimal = "Cassini";
        }
        if (reflexLvl == 31)
        {
            reflexAnimal = "Galileo";
        }
        if (reflexLvl == 32)
        {
            reflexAnimal = "Juno";
        }
        if (reflexLvl == 33)
        {
            reflexAnimal = "Helios B";
        }
        if (reflexLvl == 34)
        {
            reflexAnimal = "You";
        }
        if (reflexLvl == 35)
        {
            reflexAnimal = "Otherworldly Being";
        }
        if (reflexLvl == 36)
        {
            reflexAnimal = "Lucy";
        }
        if (reflexLvl == 37)
        {
            reflexAnimal = "Beckham";
        }
        if (reflexLvl == 38)
        {
            reflexAnimal = "Taylor";
        }

    }
}
