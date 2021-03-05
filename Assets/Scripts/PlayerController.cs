using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using System.IO;
//using UnityEngine.InputSystem.Utilities;
using System.Linq;

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
    private bool firstIntersectionExit = false;

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
        // canvas = GameObject.Find("Canvas");


        //Record start position and rotation
        startPosition = transform.position;
        startRotation = transform.eulerAngles;

        currentRotation = startRotation;

        //lastCoord = (transform.position.x / gameManager.blockSize).ToString("F2");
        startCoord = new float[] { (transform.position.x / gameManager.blockSize), (transform.position.z / gameManager.blockSize)};
        lastIntersection = startCoord;
        
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

        //PLAYER INPUT - SHORTCUTS
        if (gameManager.keyboardShortcutsEnabled)   // if inputFields are not active
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))    //Return to start 
            {
                transform.position = startPosition;
                currentRotation = startRotation;

                //ADD WAIT until you can move again

            }

            //Restart session
            if (Input.GetKeyDown(KeyCode.R))
            {
                gameManager.RestartSession();
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

            //Location shortcuts
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
        }

     

        //Take screenshots
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeScreenshot();
        }



        //Limit player's movement to the plane
        //if (transform.position.x < 0)
        //{
        //    transform.position = new Vector3(0, transform.position.y, transform.position.z);
        //}
        //if (transform.position.z < 0)
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //}
    }
    //void OnMove(InputValue input)
    //{
    //    _controls.Player.Move.performed += cntx => inputVec = cntx.ReadValue<Vector2>();
    //    _controls.Player.Move.canceled += cntx => inputVec = Vector2.zero;

    //    Vector2 inputVec = new Vector2();
    //    //InputValue input,
    //    inputVec = input.Get<Vector2>();
    //    verticalInput = inputVec.y;
    //    horizontalInput = inputVec.x;

    //    float inputV = input.Get<float>();
    //    verticalInput = inputV;
    //}

    //void OnRotate(InputValue input)
    //{
    //    float inputH = input.Get<float>();
    //    horizontalInput = inputH;
    //}

    //void OnBackwardsForce(InputValue input)
    //{
    //    float inputFloat = input.Get<float>();
    //    if (inputFloat > 0)
    //    {
    //        backwardsStepForce += 10;
    //    }
    //    else
    //    {
    //        backwardsStepForce -= 10;
    //    }
    //}

    private void OnTriggerEnter(Collider other) // When player enters an intersection
    {
        if (firstIntersectionExit)  // if the player did not start in an intersection
        {
            Debug.Log("Player has entered intersection (" + other.GetComponent<Intersection>().coordString + ")");
            calculateDirection(lastIntersection, other.GetComponent<Intersection>().coordinates);   //calculate the direction between the last intersection the one juste entered
        }

    }

    private void OnTriggerExit(Collider other)  // When player leaves an intersection
    {
        Debug.Log("Player has left intersection (" + other.GetComponent<Intersection>().coordString + ")");
        lastIntersection = other.GetComponent<Intersection>().coordinates;  // the intersection left becomes the last intersection
        firstIntersectionExit = true;   // first time the player exits an intersection
    }

    public void setSavePaths()
    {
        string screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Exports/Screenshots/");
        string trackMovementPath = Path.Combine(Directory.GetCurrentDirectory(), "Exports/TrackMovements/");
    }

    void TakeScreenshot()
    {
        var direction = ""; //initialize

        if (!System.IO.Directory.Exists(screenshotPath)) ;
        System.IO.Directory.CreateDirectory(screenshotPath);

        //if ((gameManager.inputRot == 0) || (gameManager.inputRot == 360))
        //{
        //    direction = "N";
        //}
        //else if ((gameManager.inputRot == 45) || (gameManager.inputRot == -315))
        //{
        //    direction = "NE";
        //}
        //else if ((gameManager.inputRot == 90) || (gameManager.inputRot == -270))
        //{
        //    direction = "E";
        //}
        //else if ((gameManager.inputRot == 135) || (gameManager.inputRot == -225))
        //{
        //    direction = "SE";
        //}
        //else if ((gameManager.inputRot == 180) || (gameManager.inputRot == -180))
        //{
        //    direction = "S";
        //}
        //else if ((gameManager.inputRot == 225) || (gameManager.inputRot == -135))
        //{
        //    direction = "SW";
        //}
        //else if ((gameManager.inputRot == 270) || (gameManager.inputRot == -90))
        //{
        //    direction = "W";
        //}
        //else if ((gameManager.inputRot == 315) || (gameManager.inputRot == -45))
        //{
        //    direction = "NW";
        //}

        //var screenshotName = "Screenshot_" + System.DateTime.Now.ToString("HH-mm-ss") + ".png";
        var screenshotName = gameManager.inputCoordX+ "."+ gameManager.inputCoordY + gameManager.inputDir + ".png";

        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(screenshotPath, screenshotName));
        Debug.Log(screenshotPath + screenshotName);
    }

    public void GotoCoordinates(int posX, int posY, int rot)

    //CHECK IF COORDINATES ARE VALID

    //IF VALID

    {
        transform.position = new Vector3(posX * gameManager.blockSize, 1, posY * gameManager.blockSize);
    }

    public void calculateDirection(float[] lastIntersection, float[] thisIntersection)
    {
        Debug.Log("Last intersection: " + string.Join(", ", from coord in lastIntersection select coord));
        Debug.Log("This intersection: " + string.Join(", ", from coord in thisIntersection select coord));

        if (lastIntersection != thisIntersection) //if the the last intersection is not the same as this one
        {
            if ((lastIntersection[0] - thisIntersection[0]) > 0)
            {
                cardinalDirection = "west";
                //Debug.Log("West");
            }
            else if (((lastIntersection[0] - thisIntersection[0]) < 0))
            {
                cardinalDirection = "east";
                //Debug.Log("East");
            }
            else if ((lastIntersection[1] - thisIntersection[1]) > 0)
            {
                cardinalDirection = "south";
                //Debug.Log("South");
            }
            else
            {
                cardinalDirection = "north";
                //Debug.Log("North");
            }
            Debug.Log(cardinalDirection);
            gameManager.cardinalDirection.text = "Went " + cardinalDirection + " from (" + string.Join(",", from coord in lastIntersection select coord) + ") to ("+ string.Join(",", from coord in thisIntersection select coord) + ")";
        }
        
        else
        {
            Debug.Log("Same intersecion");
        }



    }

    public void GotoCoordinates()
    {
        if (gameManager.checkCoordValid())
        {
            // goto Coordinate
            transform.position = new Vector3(gameManager.inputCoordX * gameManager.blockSize, 1, gameManager.inputCoordY * gameManager.blockSize);

            //update Rotation

            //currentRotation = new Vector3(0, gameManager.inputRot, 0);

            if (gameManager.inputDir.ToUpper() == "N")
            {
                currentRotation = new Vector3(0, 0, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "NE")
            {
                currentRotation = new Vector3(0, 45, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "E")
            {
                currentRotation = new Vector3(0, 90, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "SE")
            {
                currentRotation = new Vector3(0, 135, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "S")
            {
                currentRotation = new Vector3(0, 180, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "SW")
            {
                currentRotation = new Vector3(0, 225, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "W")
            {
                currentRotation = new Vector3(0, 270, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "NW")
            {
                currentRotation = new Vector3(0, 315, 0);
            }
        }


    }
}
