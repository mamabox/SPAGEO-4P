/* INTERSECTION MANAGER
 * Creates the intersection colliders, tracks the valid intersections and input direction
 * 
 * 
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //used for from - in - select syntax

public class IntersectionManager : MonoBehaviour
{
    public GameObject intersectionPrefab;
    private GameManager gameManager;
    private PlayerController playerController;

    public string enteredFrom; // Cardinal direction from which the last intersection was entered

    public List<string> validCoord = new List<string>();    //Stores the coordinates of all the valid intersections for the city
    public string[] validDir = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" }; //Cardinal directions allowed for GO button - MOVE TO GAME MANAGER?
    public List<string> sessionRoute = new List<string>();  //stores the coordinates of the current route
    public List<string> sessionRouteDir = new List<string>();

    // Start is called before the first frame update

    [ContextMenu("Intersection Manager")] //To debug without needing to play
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        GenerateIntersections();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // GENERATE INTERSECTION COLLIDERS FOR CITY LAYOUT
    void GenerateIntersections()
    {
        // X = 4 TO 11, Y = -4 to -1
        for (int i = 4; i <= 11; i++)
        {
            for (int j = -4; j <= -1; j++)
            {
                addPrefab(i, j);
            }
        }

        // X = 0 to 11, Y = 0 to 3
        for (int i = 0; i <= 11; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                addPrefab(i, j);
            }
        }
        // X = 0 to 7, Y = 4 to 7
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 4; j <= 7; j++)
            {
                addPrefab(i, j);
            }
        }
        //Debug.Log(string.Join(", ", from coord in validCoord select coord)); // Debug array in one line (could use coord.attribute)
    }

    public void addPrefab(int x, int z)
    {
        Instantiate(intersectionPrefab, new Vector3(x * gameManager.blockSize, 1, z * gameManager.blockSize), intersectionPrefab.transform.rotation);
        validCoord.Add(x + "," + z);    //add the prefab's coordinate to the list of valid coordinates
    }

    // RETURNS TRUE IF COORDINATES ENTERED IN UI ARE VALID - Update to return within IF loop
    public bool CheckCoordValid()
    {

        // CHECK IF COORD are valid
        if (validCoord.Contains(gameManager.inputCoord.ToUpper()))
        {
            //Debug.Log("Coordinate valid)");
            return true;
        }
        else
        {
            //Debug.Log("Coordinate invalid)");
            return false;
        }
    }

    public void GotoCoordinates()
    {
        if (CheckCoordValid()) //CHANGE TO CASE BREAK?
        {
            // goto coordinate entered in UI inputs
            transform.position = new Vector3(gameManager.inputCoordX * gameManager.blockSize, 1, gameManager.inputCoordY * gameManager.blockSize);

            //update rotation based on UI input
            if (gameManager.inputDir.ToUpper() == "N")
            {
                playerController.currentRotation = new Vector3(0, 0, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "NE")
            {
                playerController.currentRotation = new Vector3(0, 45, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "E")
            {
                playerController.currentRotation = new Vector3(0, 90, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "SE")
            {
                playerController.currentRotation = new Vector3(0, 135, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "S")
            {
                playerController.currentRotation = new Vector3(0, 180, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "SW")
            {
                playerController.currentRotation = new Vector3(0, 225, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "W")
            {
                playerController.currentRotation = new Vector3(0, 270, 0);
            }
            else if (gameManager.inputDir.ToUpper() == "NW")
            {
                playerController.currentRotation = new Vector3(0, 315, 0);
            }
        }


    }

}

