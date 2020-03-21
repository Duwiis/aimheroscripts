using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    
    public GameObject _juggleTarget;

    public int reflexCounter = 0;
    public bool reflexTargetHit = false;

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot;

    public UiManager _uiManager;
    

    public bool clickerMode = false;
    public int targetsHit;                          // amount of targets hit by player

    [SerializeField]
    private TextMeshPro _targetsHit3D;              // assigns 3dui
    [SerializeField]
    private TextMeshProUGUI _targetsHitUi;          // assigns canvasUi

    public  bool _lockMovement = false;             // if false we can move, if true we cannot move
    [SerializeField]                               
    private float _speed = 5f;                      // how fast are we
    [SerializeField]
    private float _gravity = 9.81f;                 // how much gravity pulls the player down
    [SerializeField]
    private CharacterController _controller;        // this so we can assign our player <character controller> to this script
    [SerializeField]
    private TargetSpawner _targetSpawner;   // assigns the targetspawner
    [SerializeField]
    private Look_X _look_X;                         // assigns the lookx script
    [SerializeField]
    private Look_Y _look_Y;                         // assigns the looky script


	void Start ()            // Use this for initialization
    {
        //_juggleTarget = GameObject.Find("TargetJuggle");
        /*if (clickerMode == false)
        {
            CursorInvisible();   // hides the cursor
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;          
        }*/
        
        
    }

	void Update ()              // Update is called once per frame
    {
        
        if(_uiManager.juggleBigMode == false)
        {
            CalculateMovement();    // moves player
        }
        
        
        CursorControl();        // shows the cursor

        if(clickerMode == false)
        {
            Shooting();             // shoots raycast
           
        }
        else
        {
            _lockMovement = true;
            MouseCursorShooting();
        }
        
    }

    /// <summary>
    /// ///////////////////////////////////////////////////// Cursor Control
    void CursorInvisible()                                 // hides the cursor, invoked in Start
    {
        Cursor.visible = false;                            // hides the mouse cursor 
        Cursor.lockState = CursorLockMode.Locked;          // locks the mouse to center of screen
    }
    void CursorControl()                                   // shows the cursor, invoked in Update
    {
        /*if (Input.GetKeyDown(KeyCode.Escape))              // pressing escape shows the cursor
        {
            Cursor.visible = true;                         // unhides the cursor
            Cursor.lockState = CursorLockMode.None;        // unlocks the cursor
            _lockMovement = true;
            
            
        }*/

        /*if(Input.GetKeyDown(KeyCode.BackQuote))           // when the ` key is pressed hide the cursor and lock it
        {
            Cursor.visible = false;                       // hides the cursor
            Cursor.lockState = CursorLockMode.Locked;     // locks the cursor
            _lockMovement = false;
        }*/

        if (_lockMovement == true)                        // disables look scripts so where we are looking doesn't change
        {
            _look_X.enabled = false;                      // disables look_x script 
            _look_Y.enabled = false;                      // disables look_y script
        }
        else                                              // if lockmovement is false (which means we can move) re enable the scripts
        {
            _look_X.enabled = true;                       // enables look_x script
            _look_Y.enabled = true;                       // enables look_y script
        }

    }
    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
    /// ///////////////////////////////////////////////////// End of Cursor Control
    /// </summary>
    /// 
    /// <summary>
    ////////////////////////////////////////////////////////////////////// Movement Control
    void CalculateMovement()                                            // moves player, invoked in update
    {
        float horizontalInput = Input.GetAxis("Horizontal");            // assigns horizontal input
        float verticalInput = Input.GetAxis("Vertical");                // assigns vertical input
                                                                        /// player move direction
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput); 
        Vector3 velocity = direction * _speed;                          // where we are going and how fast
        velocity.y -= _gravity;                                         // "up" movement - gravity

        velocity = transform.transform.TransformDirection(velocity);    // player moves based on their position
        _controller.Move(velocity * Time.deltaTime);                    // where we are moving in real time

    }
    /// </summary>
    ////////////////////////////////////////////////////////////////////// End of Movement Control
    /// 
    /// <summary>
    /// ////////////////////////////////////////////////////// Shooting Control
    void Shooting()                                         // shoots raycast
    {
        if (Input.GetMouseButtonDown(0) && _uiManager.gamePaused == false)                    // pressing left mouse button shoots a raycast
        {                                                   /// the origin of the raycast = center of screen
            Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitInfo;                             // gets info on what the raycast hit

            if (Physics.Raycast(rayOrigin, out hitInfo, Mathf.Infinity))    // if the ray hits something 
            {                                               /// say the name of what it hit in console
                
                 
                
                if(hitInfo.transform.tag == ("Target"))        // if raycast his a tag target
                {                                              // say in console we hit a target
                    if(_targetSpawner.reflexMode == true)
                    {
                        
                        reflexTargetHit = true;
                        _targetSpawner.ReflexModeController();
                        Destroy(hitInfo.transform.gameObject);
                    }
                                                               /// move the target to a random position
                    hitInfo.transform.position = new Vector3(Random.Range(-7f, 7f), Random.Range(-2.5f, 7f), Random.Range(2.7f,7.00005f));
                    targetsHit++;                              // increase targets hit counter by 1
                }
                if(hitInfo.transform.tag == "EnemyTarget")     // if raycast hits enemy target
                {
                    if (_targetSpawner.reflexMode == true)
                    {
                        _uiManager.reflexSecondsCount = 0;
                        if (reflexCounter < 3)
                        {
                            reflexCounter++;
                        }
                        if(reflexCounter == 3)
                        {
                            reflexCounter = 0;
                            _uiManager.reflexLvl++;
                        }
                        
                        _targetSpawner.SpawnTargetCommand();
                    }
                    Destroy(hitInfo.transform.gameObject);     // destroy the target
                    _targetSpawner.enemyTargetCount--; // subtract 1 from the enemy target count
                }
                if (hitInfo.transform.tag == "Juggle")
                {
                    _juggleTarget.GetComponent<TargetJuggleScript>().JuggleHitByPlayer();

                    if (Input.GetKey(KeyCode.Q))
                    {
                        _juggleTarget.GetComponent<TargetJuggleScript>().JuggleHitLeftX();
                    }
                    if (Input.GetKey(KeyCode.E))
                    {
                        _juggleTarget.GetComponent<TargetJuggleScript>().JuggleHitRightX();
                    }
                    if (Input.GetKey(KeyCode.B))
                    {
                        _juggleTarget.GetComponent<TargetJuggleScript>().JuggleHitDown();
                    }
                    //_juggleTarget.GetComponent<TargetJuggleScript>().ApplyForce(hitInfo.rigidbody);
                }
                if (hitInfo.transform.tag == "JuggleBig")
                {
                   
                        _juggleTarget.GetComponent<TargetJuggleScript1>().JuggleHitByPlayer();

                    if (Input.GetKey(KeyCode.A))
                    {
                        _juggleTarget.GetComponent<TargetJuggleScript1>().JuggleHitLeftX();
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        _juggleTarget.GetComponent<TargetJuggleScript1>().JuggleHitRightX();
                    }
                    if (Input.GetKey(KeyCode.B))
                    {
                        _juggleTarget.GetComponent<TargetJuggleScript1>().JuggleHitDown();
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        _juggleTarget.GetComponent<TargetJuggleScript1>().JuggleHitPull();
                    }
                    if (Input.GetKey(KeyCode.W))
                    {
                        _juggleTarget.GetComponent<TargetJuggleScript1>().JuggleHitPush();
                    }
                    //_juggleTarget.GetComponent<TargetJuggleScript>().ApplyForce(hitInfo.rigidbody);
                }
            }

        }
    }
    ///</summary>
    ///////////////////////////////////////////////////////// End of Shooting Control
    ///
    void MouseCursorShooting()
    {
        if (Input.GetMouseButtonDown(0) && _uiManager.gamePaused == false)                    // pressing left mouse button shoots a raycast
        {                                                   /// the origin of the raycast = center of screen
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;                             // gets info on what the raycast hit

            if (Physics.Raycast(rayOrigin, out hitInfo))    // if the ray hits something 
            {                                               /// say the name of what it hit in console



                if (hitInfo.transform.tag == ("Target"))        // if raycast his a tag target // in reflex mode destroy the target
                {                                              // say in console we hit a target
                    if (_targetSpawner.reflexMode == true)
                    {
                        if (_targetSpawner.reflexMode == true)
                        {
                            reflexTargetHit = true;
                            _targetSpawner.ReflexModeController();
                            Destroy(hitInfo.transform.gameObject);
                        }
                    }
                    /// move the target to a random position
                    hitInfo.transform.position = new Vector3(Random.Range(-7f, 7f), Random.Range(-2.5f, 7f), Random.Range(2.7f, 7.00005f));
                    targetsHit++;                              // increase targets hit counter by 1
                }
                if (hitInfo.transform.tag == "EnemyTarget")     // if raycast hits enemy target
                {
                    if (_targetSpawner.reflexMode == true)
                    {
                        _uiManager.reflexSecondsCount = 0;
                        if (reflexCounter < 3)
                        {
                            reflexCounter++;
                        }
                        if (reflexCounter == 3)
                        {
                            reflexCounter = 0;
                            _uiManager.reflexLvl++;
                        }
                        _targetSpawner.SpawnTargetCommand();
                    }
                    Destroy(hitInfo.transform.gameObject);     // destroy the target
                    _targetSpawner.enemyTargetCount--; // subtract 1 from the enemy target count
                }
                if (hitInfo.transform.tag == "Juggle")
                {
                    _juggleTarget.GetComponent<TargetJuggleScript>().JuggleHitByPlayer();

                    if (Input.GetKey(KeyCode.Q))
                    {
                        _juggleTarget.GetComponent<TargetJuggleScript>().JuggleHitLeftX();
                    }
                    if (Input.GetKey(KeyCode.E))
                    {
                        _juggleTarget.GetComponent<TargetJuggleScript>().JuggleHitRightX();
                    }
                    if (Input.GetKey(KeyCode.B))
                    {
                        _juggleTarget.GetComponent<TargetJuggleScript>().JuggleHitDown();
                    }

                }
            }
        }
    }
}
