using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Look_Y : MonoBehaviour //assign this to the maincamera
{
    public TextMeshPro sensitivity3dUi;
    private string sensitivitychange;
    public float parseSenesitivity = 1.5f;
    public InputField inputFieldChanged;
    public float parseSensitivityDisplay;

    [SerializeField]
    private float _sensitivity = 1f;    //how fast we move our mouse
	
	void Start () // Use this for initialization
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            parseSenesitivity = PlayerPrefs.GetFloat("Sensitivity");
        }
        Debug.Log("Sensitivity =" + parseSenesitivity);

        if(sensitivity3dUi != null)
        {
            sensitivity3dUi.text += parseSenesitivity;
        }
        
    }
	
	
	void Update () // Update is called once per frame
    {
        MouseLookVerticle(); // how we look side to side
    }

    
    void MouseLookVerticle() // how we look side to side
    {
        float _mouseY = Input.GetAxisRaw("Mouse Y");           // assign the input of moving mouse up and down to mousey 

        Vector3 newRotation = transform.localEulerAngles;   // assign the transform value for rotation to newRotation
        newRotation.x -= _mouseY * _sensitivity;            // our input multplyied by sensitivity + rotation along x axis
        transform.localEulerAngles = newRotation;           // assing the new euler angle value to roation
    }
    public void SensitivityInputField()
    {

        sensitivitychange = inputFieldChanged.GetComponent<InputField>().text;

        if (float.TryParse(sensitivitychange, out parseSenesitivity))
        {
            Debug.Log("everything is ok");
            Debug.Log("sensitivity changed" + sensitivitychange);
            _sensitivity = (parseSenesitivity / 2.27273f);
        }
        else
        {
            Debug.Log("incorrect float value");
        }
    }
}
