using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq; //for Array Contains
using TMPro;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    public Canvas canvas;
    public int blockSize = 35; //define the city's block size in meters

    // UI Elements
    public TextMeshProUGUI backwardsForceText;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI rotationText;

    public TMP_InputField posXInputField;
    public TMP_InputField posYInputField;
    public TMP_InputField rotInputField;
    public TMP_InputField dirInputField;

    public string inputCoord;
    public int inputCoordX = 0;
    public int inputCoordY = 0;
    public float inputRot = 0;
    public string inputDir;

    public bool keyboardShortcutsEnabled = true;

    public string[] validDir = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };
    public List<string> validCoord = new List<string>();



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        canvas.enabled = false;
        buildCoordArray();

    }

    // Update is called once per frame
    void Update()
    {
        //UI updates
        backwardsForceText.text = "Backwards Force (I & O) = " + player.GetComponent<PlayerController>().backwardsStepForce;
        positionText.text = "POS (X,Z) = " + player.transform.position.x.ToString("F2") + "," + player.transform.position.z.ToString("F2") + ")";
        rotationText.text = "ROT (Y) = " + player.transform.rotation.eulerAngles.y.ToString("F2"); //display rotation in euler angles with two digits
    }

    public void UpdateCoordinates()
    {
        inputCoord = posXInputField.text + "." + posYInputField.text ;
        Debug.Log("InputCoord = " + inputCoord);
        inputCoordX = int.Parse(posXInputField.text);
        inputCoordY = int.Parse(posYInputField.text);
        //inputRot = int.Parse(rotInputField.text);
        inputDir = dirInputField.text;

    }

    public bool checkCoordValid()
    {
        bool coordValid = false;

        if (validDir.Contains(inputDir.ToUpper()))
        {
            Debug.Log("Direction valid)");
            coordValid = true;
        }
        else
        {
            Debug.Log("Direction invalid)");
        }

        if (validCoord.Contains(inputCoord.ToUpper()))
        {
            Debug.Log("Coordinate valid)");
            coordValid = true;
        }
        else
        {
            Debug.Log("Coordinate invalid)");
        }

        // CHECK IF COORD are valid

        return coordValid;
    }

    public void buildCoordArray()
    {
        // Y = -4 to -1
        for (int i = 4; i <= 11; i++)
        {
            for (int j = -4; j <= -1; j++)
            {
                //Debug.Log(i + "." + j);
                validCoord.Add(i + "." + j);
            }
        }

        // Y = 0 to 3
        for (int i = 0; i <= 11; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                //Debug.Log(i + "." + j);
                validCoord.Add(i + "." + j);
            }
        }
        // Y = 4 to 7
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 4; j <= 7; j++)
            {
                //Debug.Log(i + "." + j);
                validCoord.Add(i + "." + j);
            }
        }

      
        //foreach (string coord in validCoord)
        //{
        //    Debug.Log(coord);
        //}
       
    }

    public void inputFieldActive(bool setting)
    {
        Debug.Log("DisableKeyboardShortucts = " + setting);
        keyboardShortcutsEnabled = !setting;
    }

}
