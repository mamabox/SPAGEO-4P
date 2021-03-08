using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RouteManager : MonoBehaviour
{
    private IntersectionManager intersectionManager;
    //private GameManager gameManager;

    public string coordSeparator = "_";

    //VALID ROUTES
    private string[] route1 = { "2_4E", "2_4", "3_4", "4_4", "4_5", "3_5" }; // Start point + direction, route coodirnates
    private string[] route2 = { "2_4E", "2_4", "3_4" };
    private string[] route3 = { "1.5_4E", "2_4", "3_4" };

    public string[] currentRoute;
    public string[] routeStart;


    // Start is called before the first frame update
    void Start()
    {
        currentRoute = route1;  //for testing
        intersectionManager = GetComponent<IntersectionManager>();
        routeStart = getRouteStart(currentRoute);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void validatePath(List<string> myRoute)
    {

        List<string> correctRoute = new List<string>();
        correctRoute = currentRoute.ToList();
        //foreach (string coord in currentRoute)
        //{
        //    correctRoute.Add(coord);
        //}
        correctRoute.RemoveAt(0);   //Remove the start point 

        Debug.Log("correct Route: " + string.Join(coordSeparator, correctRoute));

        validationInfo validationInfo = new validationInfo();
        bool isValid = false;
        //1. Check if route is correct
        if (correctRoute.Count() == myRoute.Count())     // are the routes the same lenght
        {
            Debug.Log("Routes have SAME LENgHT");
            for (int i = 0; i < myRoute.Count(); i++)   //3. if route not correct, check where the error was
            {
                if (myRoute.ElementAt(i) == correctRoute.ElementAt(i))
                    isValid = true;
                else
                {
                    isValid = false;
                }
            }

        }

        if (correctRoute.Last() == myRoute.Last())  //2. check if reached destination
        {
            Debug.Log("Routes ends in the same place");
        }
        else
        {
            Debug.Log("Routes DO NOT in the same place");
        }

        if (!isValid)
        {

            for (int i = 0; i < myRoute.Count(); i++)   //3. if route not correct, check where the error was
            {
                if (myRoute.ElementAt(i) != correctRoute.ElementAt(i))
                {
                    Debug.Log("Error at intersectin #: " + (i+1));
                    //break;
                }
            }
        }

        Debug.Log("Routes are the same= " + isValid);



    }

    private struct validationInfo
    {
        public bool isCorrect;
        public bool endReached;
        public int errorLocation;
        public int routeLength;
    }

    private string[] getRouteStart(string[] route)
    {
        char _startDir = route[0][route[0].Length - 1];    //direcion is the last character
        string _startCoord = route[0].Remove(route[0].Length - 1);  //coord is route[0] minus last character

        //Debug.Log("Start Coord from currentRoute =" + _startCoord);
        //Debug.Log("Start Dir from currentRoute =" + _startDir);

        return new string[] { _startCoord, _startDir.ToString() };
    }
}
