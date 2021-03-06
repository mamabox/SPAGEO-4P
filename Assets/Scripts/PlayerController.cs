﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using System.IO;
//using UnityEngine.InputSystem.Utilities;
using System.Linq;
using UnityEngine.SceneManagement; // for restart scene
using System; //for Math.Round

/**
 * SIMPLE PLAYER CONTROLLER
 * 
 * horizontal input = move
 * vertical input = rotate
 * 
 */

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f; //Player's walking speed
    public float lookSpeed = 50.0f; //Player's turning speed
    public int backwardsStepForce = 500;

    public string screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Exports/Screenshots/");
    public string trackMovementPath = Path.Combine(Directory.GetCurrentDirectory(), "Exports/TrackMovements/");

    private readonly int xRange = 350; // Ground plane size (x-axis) * 10
    private readonly int yRange = 350; // Ground plane size (y-axis) * 10

    private Camera playerCamera;
    private Rigidbody playerRb;
    private GameManager gameManager;
    private IntersectionManager intersectionManager;
    //    public Canvas canvas;   //Move to GameManager.cs

    //private PlayerControl _controls;

    public Vector3 startPosition; //Used to reset to initial position
    public Vector3 startRotation; //User to reset to initial rotatin
    public Vector2 inputVec;
    public float[] startCoord;
    public float[] lastIntersection ; //coordinate of the last intersection the player went through
    public string cardinalDirection;

    public Vector3 currentRotation; //Player's current rotation
    private Vector3 moveVec;

    public float horizontalInput; //Value of horizontal input
    public float verticalInput; //Value of vertical input

    public bool tookStep = false;
    private bool firstIntersectionExit = false; //not needed?
    public bool firstIntersectionEnter = false;

    private void Awake()
    {
        // _controls = new PlayerControl();
    }
    [ContextMenu ("PlayerController")]
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main; //Set playerCamera to camera with 'main'tag
        playerRb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        intersectionManager = FindObjectOfType<GameManager>().GetComponent<IntersectionManager>();
        // canvas = GameObject.Find("Canvas");


        //Record start position and rotation
        startPosition = transform.position;
        startRotation = transform.eulerAngles;

        currentRotation = startRotation;

        //lastCoord = (transform.position.x / gameManager.blockSize).ToString("F2");
        startCoord = new float[] { (transform.position.x / gameManager.blockSize), (transform.position.z / gameManager.blockSize)};
        //lastIntersection = startCoord;
        lastIntersection = new float[] { (float)System.Math.Round(startCoord[0], 1), (float)System.Math.Round(startCoord[1], 1) }; //temporay - only used to simplify UI display
        
    }

        //setSavePaths(); //Set the saving paths for screenshots and player movements
    

    // Update is called once per frame
    void FixedUpdate()
    {
        //PLAYER INPUT - MOVEMENT

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


        if (verticalInput > 0)
        {
            transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * speed);
            tookStep = false;
        }
        else if (verticalInput < 0 && !tookStep)
        {
            tookStep = true;
            playerRb.AddRelativeForce(Vector3.back * backwardsStepForce, ForceMode.Impulse);
        }

        currentRotation.y += horizontalInput * Time.deltaTime * lookSpeed;
        transform.eulerAngles = new Vector3(0, currentRotation.y, 0);

        //PLAYER INPUT - KEYBOARD SHORTCUTS
        if (gameManager.keyboardShortcutsEnabled)   // if inputFields are not active
        {
            //Return to start position
            if (Input.GetKeyDown(KeyCode.Alpha0))    
            {
                transform.position = startPosition;
                currentRotation = startRotation;

                //ADD WAIT until you can move again

            }

            //Restart session
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reload current scene
                //SceneManager.LoadScene("Game"); //Load scene called Game
                //gameManager.RestartSession();
            }

            //Close game
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            //Change the backwards force
            if (Input.GetKeyDown(KeyCode.I) && (backwardsStepForce > 300))
            {
                backwardsStepForce -= 10;
            }
            if (Input.GetKeyDown(KeyCode.O) && (backwardsStepForce < 700))
            {
                backwardsStepForce += 10;
            }

            //Location shortcuts - temporary for debugging
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                transform.position = new Vector3(0, 1, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                transform.position = new Vector3(0, 1, 245);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                transform.position = new Vector3(245, 1, 245);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                transform.position = new Vector3(245, 1, 105);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                transform.position = new Vector3(385, 1, 105);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                transform.position = new Vector3(385, 1, -140);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                transform.position = new Vector3(140, 1, -140);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                transform.position = new Vector3(140, 1, 0);
            }

            //Hide Canvas
            if (Input.GetKeyDown(KeyCode.M))
            {
                gameManager.canvas.enabled = !gameManager.canvas.enabled;
            }

            //Take screenshot
            if (Input.GetKeyDown(KeyCode.S))
            {
                TakeScreenshot();
            }

            //Start/End session
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Session status changed (Started or Ended)");
            }
        }
    }

    // WHEN PLAYER ENTERS AN INTERSECTION
    private void OnTriggerEnter(Collider other)
    {
        if (firstIntersectionEnter)  // if not the first time entering an intersection
        {
            calculateDirection(lastIntersection, other.GetComponent<Intersection>().coordinates);   //calculate the direction between the last intersection the one just entered
            if (intersectionManager.sessionRoute.Last() != other.GetComponent<Intersection>().coordString) //if no intersection has been entered this is not the last intersection entered, add it to the session route
            {
                intersectionManager.sessionRoute.Add(other.GetComponent<Intersection>().coordString);
                intersectionManager.sessionRouteDir.Add(other.GetComponent<Intersection>().coordString + cardinalDirection);
            }

        }
        else    //if this this is the first time enterring the intersection
        {
            //intersectionManager.sessionRoute.Add(other.GetComponent<Intersection>().coordString);
            //intersectionManager.sessionRouteDir.Add(other.GetComponent<Intersection>().coordString + cardinalDirection);
            firstIntersectionEnter = true;
        }


        Debug.Log("Player has entered intersection (" + other.GetComponent<Intersection>().coordString + ")");
        Debug.Log("Route: " + String.Join(";", intersectionManager.sessionRoute));
        Debug.Log("Route with direction: " + String.Join(";", intersectionManager.sessionRouteDir));




    }

    // WHEN PLAYER LEAVES AN INTERSECTION
    private void OnTriggerExit(Collider other)
    {



        lastIntersection = other.GetComponent<Intersection>().coordinates;  // the intersection left becomes the last intersection

        if (!firstIntersectionExit) // if the playe started in the intersection
        {
            intersectionManager.sessionRoute.Add(other.GetComponent<Intersection>().coordString);
            intersectionManager.sessionRouteDir.Add(other.GetComponent<Intersection>().coordString + cardinalDirection);
            firstIntersectionExit = true;
        }
        // firstIntersectionExit = true;   // first time the player exits an intersection

        Debug.Log("Player has left intersection (" + other.GetComponent<Intersection>().coordString + ")");
        Debug.Log("Route: " + String.Join(";", intersectionManager.sessionRoute));
        Debug.Log("Route with direction: " + String.Join(";", intersectionManager.sessionRouteDir));

    }


    void TakeScreenshot()
    {

        if (!System.IO.Directory.Exists(gameManager.screenshotPath))
        {
            System.IO.Directory.CreateDirectory(gameManager.screenshotPath);

        }

        //var screenshotName = "Screenshot_" + System.DateTime.Now.ToString("HH-mm-ss") + ".png";
        var screenshotName = gameManager.inputCoordX+ "."+ gameManager.inputCoordY + gameManager.inputDir + ".png";

        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(gameManager.screenshotPath, screenshotName));
        Debug.Log(gameManager.screenshotPath + screenshotName);
    }

    //public void GotoCoordinates(int posX, int posY, int rot) - DELETE

    ////CHECK IF COORDINATES ARE VALID

    ////IF VALID

    //{
    //    transform.position = new Vector3(posX * gameManager.blockSize, 1, posY * gameManager.blockSize);
    //}

    public void calculateDirection(float[] lastIntersection, float[] thisIntersection)
    {
        //Debug.Log("Last intersection: " + string.Join(", ", from coord in lastIntersection select coord));
        //Debug.Log("This intersection: " + string.Join(", ", from coord in thisIntersection select coord));

        if (lastIntersection != thisIntersection) //if the the last intersection is not the same as this one
        {
            if ((lastIntersection[0] - thisIntersection[0]) > 0)
            {
                cardinalDirection = "W";
                //Debug.Log("West");
            }
            else if (((lastIntersection[0] - thisIntersection[0]) < 0))
            {
                cardinalDirection = "E";
                //Debug.Log("East");
            }
            else if ((lastIntersection[1] - thisIntersection[1]) > 0)
            {
                cardinalDirection = "S";
                //Debug.Log("South");
            }
            else
            {
                cardinalDirection = "N";
                //Debug.Log("North");
            }
            //Debug.Log(cardinalDirection);
            gameManager.cardinalDirection.text = "(" + string.Join(",", from coord in lastIntersection select coord) + ") to ("+ string.Join(",", from coord in thisIntersection select coord) + ") - " + cardinalDirection;    // Would String.Join("; ", myArray) work? - TO TEST

        }

        else
        {
            Debug.Log("Same intersecion");
        }



    }


}
