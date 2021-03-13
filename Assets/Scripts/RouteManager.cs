using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class RouteManager : MonoBehaviour
{
    public GameObject routeIndicatorPrefab;
    
    private IntersectionManager intersectionManager;
    private GameManager gameManager;
    private CheckpointManager checkpointManager;

    public string coordSeparator = "_"; //Should be CHAR

    //VALID ROUTES
    //private string[] route1 = { "2_4E", "2_4", "3_4", "4_4", "4_5", "3_5" }; // Start point + direction, route coodirnates
    private List<string> route1 = new List<string> { "2_4E", "2_4", "3_4", "4_4", "4_5", "3_5" }; // Start point + direction, route coodirnates
    private List<string> route2 = new List<string> { "2_4E", "2_4", "3_4" };
    public List<string> importedRoutes;
    public List<string> routesS0;
    

    public List<string> selectedRoute;
    public List<string> routeStart;
    public ValidationInfo validationInfo = new ValidationInfo();
    private string importPath;

    [ContextMenu("Route Manager")]
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        intersectionManager = GetComponent<IntersectionManager>();
        checkpointManager = GetComponent<CheckpointManager>();
    
        importPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Media/Text/");

        ImportAllText();

        selectedRoute = importedRoutes.ElementAt(0).Split(',').ToList();  //for testing, manual route test
        

        routeStart = getRouteStart(selectedRoute);

        if (gameManager.selectedSequence == 0)
        {
            Sequence0();
        }

        if (gameManager.selectedSequence == 2)
        {
            Sequence2();
        }


        //SpawnLine(selectedRoute);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ImportAllText()
    {
        importedRoutes = ImportText("routes.txt");
        routesS0 = ImportText("S0.txt");
        checkpointManager.checkpointsText = ImportText("checkpointstext.txt");
    }

    private void Sequence0()
    {
        SpawnLine(routesS0.ElementAt(2).Split(',').ToList());       //Draw a line with the coordinates from line 2 in text file
        checkpointManager.GenerateCheckpoints(routesS0.ElementAt(0).Split(',').ToList()); //Generate Checkpoints from coordinates from line 1
        intersectionManager.GotoCoord("3_5", "E");
    }

    private void Sequence2()
    {
        List<string> lineToDraw = new List<string>(selectedRoute);
        lineToDraw.RemoveAt(0); //Remove the start coordiante
        SpawnLine(lineToDraw);
        
    }

    public ValidationInfo validatePath(List<string> myRoute)
    {
        gameManager.validationCheck = true;
        List<string> correctRoute = new List<string>(selectedRoute);

        correctRoute.RemoveAt(0);   //Remove the start point 

        Debug.Log("Checking Correctroute: " + string.Join(coordSeparator, correctRoute));

        bool hasError = false;
        //1. Check if route is correct
        if (correctRoute.Count() != myRoute.Count())     
        {
            hasError = true;
        }
        else //  routes the same lenght
        {
            //Debug.Log("Routes have the same length");
            for (int i = 0; i < myRoute.Count(); i++)   //3. if route not correct, check where the error was
            {
                if (myRoute.ElementAt(i) != correctRoute.ElementAt(i))
                {
                    //Debug.Log("comparaison error found");
                    hasError = true;
                }
            }
        }
   

        if (!hasError)
        {
            //Debug.Log("there are no errors, the route is valid");
            validationInfo.isValid = true;
            validationInfo.errorAt = 0;
            validationInfo.endReached = true;
        }
        else if (myRoute.Count!=0)
        {
            //Debug.Log("there are errors");
            validationInfo.isValid = false;
            if (correctRoute.Last() == myRoute.Last())  //2. check if reached destination
            {
                validationInfo.endReached = true;
                //Debug.Log("Routes ends in the same place");
            }
            else
            {
                validationInfo.endReached = false;
                //Debug.Log("Routes DO NOT in the same place");
            }

            for (int i = 0; i < myRoute.Count(); i++)   //3. if route not correct, check where the error was
            {
                if (correctRoute.Count() >= myRoute.Count()) // myRoute is not longer than the correct route
                {
                    if (myRoute.ElementAt(i) != correctRoute.ElementAt(i))
                    {
                        validationInfo.errorAt = i + 1;
                        //Debug.Log("Error at intersectin #: " + (i+1));
                        break;
                    }
                    else
                    {
                        validationInfo.errorAt = 0;   
                    }
                }
                else
                {
                    validationInfo.errorAt = correctRoute.Count()+1;
                }
            }
        }

        //Debug.Log("Routes are the same= " + isValid);
        validationInfo.routeLength = myRoute.Count();
        Debug.Log("Valid= " + validationInfo.isValid + "- length: " + validationInfo.routeLength + "- error at #: " + validationInfo.errorAt + "- endReached= " + validationInfo.endReached);
        return validationInfo;


    }

    public struct ValidationInfo
    {
        public bool isValid;
        public bool endReached;
        public int errorAt;
        public int routeLength;
    }

    private List<string> getRouteStart(List<string> route) // returns the first 
    {

        //Debug.Log("Route count (getRouteStart)= " + selectedRoute.Count());
        //char _startDir = route[0][route[0].Length - 1];    //direcion is the last character of the last coordinate
        char _startDir = route.ElementAt(0).Last();     //direcion is the last character of the last coordinate
        string _startCoord = route[0].Remove(route[0].Length - 1);  //coord is route[0] minus last character
        //string _startCoord = route.ElementAt(0).Remove(route.ElementAt(0).Length - 1); //WHY DOES NOT WORK?

        //Debug.Log("Start Coord from currentRoute =" + _startCoord);
        //Debug.Log("Start Dir from currentRoute =" + _startDir);

        return new List<string> { _startCoord, _startDir.ToString() };
    }

    //IMPORTS TEXT FROM A .TXT FILE AND RETURNS EACH LINE AS A STRING
    public List<string> ImportText(string fileName)    
    {
        List<string> txtImport = new List<string>(System.IO.File.ReadAllLines(importPath + fileName));

        return txtImport;
    }

    private void SpawnLine(List<string> route)
    {
        GameObject newLineGen = Instantiate(routeIndicatorPrefab);
        LineRenderer lRend = newLineGen.GetComponent<LineRenderer>();

        List<string> lineToDraw = new List<string> (route); 
        //lineToDraw.RemoveAt(0); //remove the starting position
        lRend.positionCount = lineToDraw.Count();    //set length of line renderer to the number of coordinates on the path 

        for (int i = 0; i < lineToDraw.Count(); i++)
        {
            string[] _coord = lineToDraw[i].Split(char.Parse(coordSeparator));
            //Debug.Log("Draw at (" + string.Join(", ", _coord) + ")");
            lRend.SetPosition(i, new Vector3(float.Parse(_coord[0]) * gameManager.blockSize, 0.01f, float.Parse(_coord[1]) * gameManager.blockSize));
        }

    }

    //private void GenerateCheckpoints(List<string> checkpoints)
    //{
    //    string[] coordArray;
    //    for (int i = 0; i < checkpoints.Count(); i++)
    //    {
    //        coordArray = checkpoints[i].Split(char.Parse(coordSeparator));      //stores the coordinates x and y in an array
    //        //Debug.Log("Generating checkpoint at (" + coordArray[0] + coordSeparator + coordArray[1]);
    //        var newCheckpoint = Instantiate(checkpointPrefab , new Vector3(float.Parse(coordArray[0]) * gameManager.blockSize, 0, float.Parse(coordArray[1]) * gameManager.blockSize), checkpointPrefab.transform.rotation);    //instantiate the checkpoint
    //        newCheckpoint.GetComponent<Checkpoint>().coordString = checkpoints[i];  //store the coordinates as a string in the instance
    //        newCheckpoint.GetComponent<Checkpoint>().checkpointID = i+1;    //stores the checkpoitn number in the instance
    //    }
    //}

    //private string[] getRouteStartOld (string[] route)
    //{
    //    char _startDir = route[0][route[0].Length - 1];    //direcion is the last character
    //    string _startCoord = route[0].Remove(route[0].Length - 1);  //coord is route[0] minus last character

    //    //Debug.Log("Start Coord from currentRoute =" + _startCoord);
    //    //Debug.Log("Start Dir from currentRoute =" + _startDir);

    //    return new string[] { _startCoord, _startDir.ToString() };
    //}
}
