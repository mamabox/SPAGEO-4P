using System.Collections;
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

    //public string screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Exports/Screenshots/");
    //public string trackMovementPath = Path.Combine(Directory.GetCurrentDirectory(), "Exports/TrackMovements/");

    private readonly int xRange = 350; // Ground plane size (x-axis) * 10
    private readonly int yRange = 350; // Ground plane size (y-axis) * 10

    private Camera playerCamera; //Needed?
    private Rigidbody playerRb;
    private GameManager gameManager;
    private IntersectionManager intersectionManager;
    private RouteManager routeManager;
    private ScreenshotManager screenshotManager;


    public Vector3 startPosition; //Used to reset to initial position
    public Vector3 startRotation; //User to reset to initial rotatin
    public Vector2 inputVec;
    public float[] startCoord;
    public float[] lastIntersection; //coordinate of the last intersection the player went through - MOVE TO intersectionmanager.cs
    public string cardinalDirection; //MOVE TO intersectionmanager.cs

    public Vector3 currentRotation; //Player's current rotation
    private Vector3 moveVec;

    public float horizontalInput; //Value of horizontal input
    public float verticalInput; //Value of vertical input

    public bool tookStep = false;
    //public bool firstIntersectionExit = false; //not needed?
    //private bool firstIntersectionEnter = false;
    //public bool intersectionStart = true;

    //private bool firstDirectionNeed = false;

    //public bool startInSegment = false;
    public bool playerFirstMove = false;

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
        gameManager = GetComponent<GameManager>();
        intersectionManager = FindObjectOfType<GameManager>().GetComponent<IntersectionManager>();
        routeManager = FindObjectOfType<GameManager>().GetComponent<RouteManager>();
        screenshotManager = FindObjectOfType<GameManager>().GetComponent<ScreenshotManager>();

        intersectionManager.GotoCoord(routeManager.routeStart[0], routeManager.routeStart[1]);

        //Record start position and rotation
        startPosition = transform.position;
        startRotation = transform.eulerAngles;

        currentRotation = startRotation;

        //lastCoord = (transform.position.x / gameManager.blockSize).ToString("F2");
        startCoord = new float[] { (transform.position.x / gameManager.blockSize), (transform.position.z / gameManager.blockSize)};
        //lastIntersection = startCoord; //restore insted of below
        lastIntersection = new float[] { (float)System.Math.Round(startCoord[0], 1), (float)System.Math.Round(startCoord[1], 1) }; //temporay - only used to simplify UI display



    }

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
            playerFirstMove = true;
        }
        else if (verticalInput < 0 && !tookStep)
        {
            tookStep = true;
            playerRb.AddRelativeForce(Vector3.back * backwardsStepForce, ForceMode.Impulse);
            playerFirstMove = true;
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
                screenshotManager.TakeScreenshot();
            }

            //Start/End route recording
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("Session status changed (Started or Ended)");
                routeManager.validatePath(intersectionManager.sessionRoute);
            }

            //New attempt
            if (Input.GetKeyDown(KeyCode.X))
            {
                //Debug.Log("Session status changed (Started or Ended)");
                Debug.Log("session Route cleared");
                gameManager.newAttemp();
            }
        }
    }

    // WHEN PLAYER ENTERS AN INTERSECTION
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER (" + other.GetComponent<Intersection>().coordString + ")");
        intersectionManager.OnIntersectionEnter(other);
    }

    // WHEN PLAYER LEAVES AN INTERSECTION
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("EXIT (" + other.GetComponent<Intersection>().coordString + ")");
        intersectionManager.OnIntersectionExit(other);
    }



}
