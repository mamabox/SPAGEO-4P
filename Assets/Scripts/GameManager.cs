using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //for button
using System.IO; //for Paths

using System.Linq; //for Array Contains
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    private IntersectionManager intersectionManager;
    private RouteManager routeManager;
    private PlayerController playerController;
    
    public Canvas canvas;
    public int blockSize = 35; //define the city's block size in meters

    // UI Elements
    public TextMeshProUGUI routeText;
    public TextMeshProUGUI routeDirText;
    public TextMeshProUGUI backwardsForceText;
    public TextMeshProUGUI cardinalDirection;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI rotationText;
    public TextMeshProUGUI routeValidationText;

    public TMP_InputField posXInputField;
    public TMP_InputField posYInputField;
    public TMP_InputField dirInputField;

    public Button goto_Btn;

    public string inputCoord;
    public int inputCoordX = 0;
    public int inputCoordY = 0;
    public float inputRot = 0;
    public string inputDir;

    public string screenshotPath;
    public string trackMovementPath;

    public bool keyboardShortcutsEnabled = true;
    public bool validationCheck = false;

    public string routeSeparator = ",";    //UI display only

    public int selectedSequence = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        intersectionManager = GetComponent<IntersectionManager>();
        routeManager = GetComponent<RouteManager>();
        playerController = player.GetComponent<PlayerController>();

        goto_Btn.onClick.AddListener(gotoBtnHandler);

        canvas.enabled = true;  //Enable the UI
        setSavePaths(); //Set paths for saving data

    }

    // Update is called once per frame
    void Update()
    {
        //UI updates
        backwardsForceText.text = "Backwards Force (I & O) = " + player.GetComponent<PlayerController>().backwardsStepForce;
        positionText.text = "POS (X,Z) = (" + player.transform.position.x.ToString("F2") + "," + player.transform.position.z.ToString("F2") + ")";
        rotationText.text = "ROT (Y) = " + player.transform.rotation.eulerAngles.y.ToString("F2"); //display rotation in euler angles with two digits
        routeText.text = "R : "+ String.Join(routeSeparator, intersectionManager.sessionRoute);
        routeDirText.text = "RD: "+ String.Join(routeSeparator, intersectionManager.sessionRouteDir);
        if (validationCheck)
        {
            routeValidationText.text = ("Valid= " + routeManager.validationInfo.isValid+ " - errorat #: " + routeManager.validationInfo.errorAt + " - endReached= " + routeManager.validationInfo.endReached + " - length: " + routeManager.validationInfo.routeLength);
        }
        else
        {
            routeValidationText.text = "Validation (spacebar)";
        }
    }

    public void UpdateCoordinates()
    {
        if ((posXInputField.text != "") && (posYInputField.text != ""))
        {
            inputCoord = posXInputField.text + routeManager.coordSeparator + posYInputField.text;
            inputCoordX = int.Parse(posXInputField.text);
            inputCoordY = int.Parse(posYInputField.text);
            inputDir = dirInputField.text;
        }
    }

    public void InputFieldActive(bool setting)
    {
        //Debug.Log("DisableKeyboardShortucts = " + setting);
        keyboardShortcutsEnabled = !setting;
    }

    public void RestartSession()
    {
        Debug.Log("Restart the session");
    }

    public void setSavePaths()
    {
        screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Exports/Screenshots/");
        trackMovementPath = Path.Combine(Directory.GetCurrentDirectory(), "Exports/TrackMovements/");
    }

    void gotoBtnHandler()
    {
        Debug.Log("Inside button handler");
        UpdateCoordinates();
        intersectionManager.GotoCoord(inputCoord, inputDir);
    }

    public void newAttempt()
    {
        // ADD CLEAR UI
        validationCheck = false;
        intersectionManager.sessionRoute.Clear();
        intersectionManager.sessionRouteDir.Clear();
        playerController.playerHasMoved = false;
        //intersectionManager.GotoCoord("1.5_4", "E");
        intersectionManager.GotoCoord(routeManager.routeStart.ElementAt(0), routeManager.routeStart.ElementAt(1));
    }

}
