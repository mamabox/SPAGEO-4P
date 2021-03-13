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
    private GameObject player;
    private RouteManager routeManager;

    public string enteredFrom; // Cardinal direction from which the last intersection was entered
    public string cardinalDirection; //MOVE TO intersectionmanager.cs

    public List<string> validCoord = new List<string>();    //Stores the coordinates of all the valid intersections for the city
    public string[] validDir = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" }; //Cardinal directions allowed for GO button - MOVE TO GAME MANAGER?
    public List<string> sessionRoute = new List<string>();  //stores the coordinates of the current route
    public List<string> sessionRouteDir = new List<string>();

    // Start is called before the first frame update

    [ContextMenu("Intersection Manager")] //To debug without needing to play
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        routeManager = GetComponent<RouteManager>();
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
                AddPrefab(i, j);
            }
        }

        // X = 0 to 11, Y = 0 to 3
        for (int i = 0; i <= 11; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                AddPrefab(i, j);
            }
        }
        // X = 0 to 7, Y = 4 to 7
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 4; j <= 7; j++)
            {
                AddPrefab(i, j);
            }
        }
        //Debug.Log(string.Join(", ", from coord in validCoord select coord)); // Debug array in one line (could use coord.attribute)
    }

    public void AddPrefab(int x, int z)
    {
        Instantiate(intersectionPrefab, new Vector3(x * gameManager.blockSize, 1, z * gameManager.blockSize), intersectionPrefab.transform.rotation);
        validCoord.Add(x + routeManager.coordSeparator + z);    //add the prefab's coordinate to the list of valid coordinates
    }

    // RETURNS TRUE IF COORDINATES ARE VALID for an intersection
    private bool IsIntersectionCoordValid(string coord)
    {

        // CHECK IF COORD are valid
        if (validCoord.Contains(coord.ToUpper()))
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

    // RETURNS TRUE IF DIRECTION ARE VALID
    private bool IsDirValid(string dir)
    {

        // CHECK IF COORD are valid
        if (validDir.Contains(dir.ToUpper()))
        {
            //Debug.Log("Direction valid)");
            return true;
        }
        else
        {
            //Debug.Log("Direction invalid)");
            return false;
        }
    }

    //RETURNS TRUE IF coordinates are valid for segments - WIP - FOR NOW just returns if is in range of city
    public bool IsCoordValid(string coord)
    {
        string[] coordArray = coord.Split(char.Parse(routeManager.coordSeparator));
        float coordX = float.Parse(coordArray[0]);
        float coordY = float.Parse(coordArray[1]);

        // X = 4 TO 11, Y = -4 to -1
        if ((coordY >= -4) && (coordY < 0))
        {
            if ((coordX >= 4) && (coordX <= 11))
            {
                return true;
            }
        }
        // X = 0 to 11, Y = 0 to 3
        else if ((coordY >= 0) && (coordY < 4))
        {
            if ((coordX >= 0) && (coordX <= 11))
            {
                return true;
            }
        }
        // X = 0 to 7, Y = 4 to 7
        else if ((coordY >= 4) && (coordY <= 7))
        {
            if ((coordX >= 0) && (coordX <= 7))
            {
                return true;
            }
        }
            return false;

    }

    public void GotoCoord(string coordStr, string dir)
    {
        string[] coordArray = coordStr.Split(char.Parse(routeManager.coordSeparator));
        //string[] _coord = coord.Split('.');

        if (IsCoordValid(coordStr))
        {
            player.transform.position = new Vector3(float.Parse(coordArray[0]) * gameManager.blockSize, 1, float.Parse(coordArray[1]) * gameManager.blockSize);
        }

        if (IsDirValid(dir))
        {
            switch (dir.ToUpper())
            {
                case "N":
                    playerController.currentRotation = new Vector3(0, 0, 0);
                    break;
                case "NE":
                    playerController.currentRotation = new Vector3(0, 45, 0);
                    break;
                case "E":
                    playerController.currentRotation = new Vector3(0, 90, 0);
                    break;
                case "SE":
                    playerController.currentRotation = new Vector3(0, 135, 0);
                    break;
                case "S":
                    playerController.currentRotation = new Vector3(0, 180, 0);
                    break;
                case "SW":
                    playerController.currentRotation = new Vector3(0, 225, 0);
                    break;
                case "W":
                    playerController.currentRotation = new Vector3(0, 270, 0);
                    break;
                case "NW":
                    playerController.currentRotation = new Vector3(0, 315, 0);
                    break;
            }
        }
    }

    
    // WHEN PLAYER ENTERS AN INTERSECTION
    public void OnIntersectionEnter(Collider other)
    {
        string lastRecorded;

        if (sessionRoute.Count == 0)    //if a route has not been started
        {
            //Debug.Log("Session route = 0");
            if (playerController.playerHasMoved)    // if the player started in a segment add to route and calculate direction
            {
                sessionRoute.Add(other.GetComponent<Intersection>().coordString);
                calculateDirection(playerController.lastIntersection, other.GetComponent<Intersection>().coordinates);
                sessionRouteDir.Add(other.GetComponent<Intersection>().coordString + cardinalDirection);
            }
            else // if the player started at an intersection
            {
                //Debug.Log("Player has not moved yet");
                sessionRoute.Add(other.GetComponent<Intersection>().coordString); //add to route but do not calculate direction
            }

        }
        else if (sessionRoute.Count != 0) //if a route has started
        {
            lastRecorded = sessionRoute.Last();
            if ((lastRecorded != other.GetComponent<Intersection>().coordString)) // if this intersection is different than the last
            {
                sessionRoute.Add(other.GetComponent<Intersection>().coordString);
                calculateDirection(playerController.lastIntersection, other.GetComponent<Intersection>().coordinates);
                if (sessionRouteDir.Count() == 0)   // if the direction route is empty
                {
                    sessionRouteDir.Add(lastRecorded + cardinalDirection);  //add an the direction from the last intersection
                }
                sessionRouteDir.Add(other.GetComponent<Intersection>().coordString + cardinalDirection);
            }
            else
            {
                Debug.Log("Same Intersection");
            }
        }

        //Debug.Log("Route: " + String.Join(";", intersectionManager.sessionRoute));
        //Debug.Log("Route with direction: " + String.Join(";", intersectionManager.sessionRouteDir));

    }

    // WHEN PLAYER LEAVES AN INTERSECTION
    public void OnIntersectionExit(Collider other)
    {
        playerController.lastIntersection = other.GetComponent<Intersection>().coordinates;  // the intersection left becomes the last intersection
    }


    // CALCULATE FROM WHICH CARDINAL DIRECTION THE INTERSECTION IS ENTERED
    public void calculateDirection(float[] lastIntersection, float[] thisIntersection)
    {
        //Debug.Log("Last intersection: " + string.Join(", ", from coord in lastIntersection select coord));
        //Debug.Log("This intersection: " + string.Join(", ", from coord in thisIntersection select coord));

        if (lastIntersection != thisIntersection) //if the the last intersection is not the same as this one
        {
            if ((lastIntersection[0] - thisIntersection[0]) > 0)
            {
                cardinalDirection = "W";
            }
            else if (((lastIntersection[0] - thisIntersection[0]) < 0))
            {
                cardinalDirection = "E";
            }
            else if ((lastIntersection[1] - thisIntersection[1]) > 0)
            {
                cardinalDirection = "S";
            }
            else
            {
                cardinalDirection = "N";
            }
            //Debug.Log(cardinalDirection);
            gameManager.cardinalDirection.text = "(" + string.Join(routeManager.coordSeparator, lastIntersection) + ") to (" + string.Join(routeManager.coordSeparator, thisIntersection) + ") - " + cardinalDirection;
            //gameManager.cardinalDirection.text = "(" + string.Join(".", from coord in lastIntersection select coord) + ") to ("+ string.Join(".", from coord in thisIntersection select coord) + ") - " + cardinalDirection
        }
        else
        {
            Debug.Log("Same intersecion");
        }
    }

}

