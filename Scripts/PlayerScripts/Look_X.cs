using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Look_X : MonoBehaviour
{
    public TextMeshPro sensitivity3dUi;
    public float sensitivityPref;
    public InputField inputFieldChanged;
    [SerializeField]
    private float _sensitivity = 1f;    //how fast we move our mouse
    private string sensitivitychange;
    public float parseSenesitivity = 1.5f;

    void Start () // Use this for initialization
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            parseSenesitivity = PlayerPrefs.GetFloat("Sensitivity");
        }
        Debug.Log("Sensitivity =" + parseSenesitivity);
    }
	
	
	void Update () // Update is called once per frame
    {
        MouseLookHorizontal(); // how we look side to side

    }
    

    
    void MouseLookHorizontal() // how we look side to side
    {
        float _mouseX = Input.GetAxisRaw("Mouse X");           // assign the input of moving mouse side to side to mouseX 

        Vector3 newRotation = transform.localEulerAngles;   // assign the transform value for rotation to newRotation
        newRotation.y += _mouseX * _sensitivity;            // our input multplyied by sensitivity + rotation along y axis
        transform.localEulerAngles = newRotation;           // assing the new euler angle value to roation
    }
    
    public void SensitivityInputField()
    {
        
        sensitivitychange = inputFieldChanged.GetComponent<InputField>().text;
        
        if(float.TryParse(sensitivitychange, out parseSenesitivity))
        {
            
            Debug.Log("everything is ok");
            Debug.Log("sensitivity changed" + sensitivitychange);
            _sensitivity = (parseSenesitivity / 2.27273f);
            PlayerPrefs.SetFloat("Sensitivity", parseSenesitivity);
        }
        else
        {
            Debug.Log("incorrect float value");
        }
    }
}
