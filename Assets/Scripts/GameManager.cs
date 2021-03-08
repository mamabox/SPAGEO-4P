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
    
    public Canvas canvas;
    public int blockSize = 35; //define the city's block size in meters

    // UI Elements
    public TextMeshProUGUI RouteText;
    public TextMeshProUGUI RouteDirText;
    public TextMeshProUGUI backwardsForceText;
    public TextMeshProUGUI cardinalDirection;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI rotationText;

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

    private string routeSeparator = ",";    //UI display only

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        intersectionManager = GetComponent<IntersectionManager>();
        routeManager = GetComponent<RouteManager>();

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
        RouteText.text = "R : "+ String.Join(routeSeparator, intersectionManager.sessionRoute);
        RouteDirText.text = "RD: "+ String.Join(routeSeparator, intersectionManager.sessionRouteDir);
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

}
