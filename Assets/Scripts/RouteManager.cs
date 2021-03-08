using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RouteManager : MonoBehaviour
{
    private IntersectionManager intersectionManager;
    private GameManager gameManager;

    public string coordSeparator = "_";

    //VALID ROUTES
    private string[] route1 = { "2_4E", "2_4", "3_4", "4_4", "4_5", "3_5" }; // Start point + direction, route coodirnates
    private string[] route2 = { "2_4E", "2_4", "3_4" };
    private string[] route3 = { "1.5_4E", "2_4", "3_4" };

    public string[] currentRoute;
    public string[] routeStart;
    public ValidationInfo validationInfo = new ValidationInfo();


    // Start is called before the first frame update
    void Start()
    {
        currentRoute = route1;  //for testing
        gameManager = GetComponent<GameManager>();
        intersectionManager = GetComponent<IntersectionManager>();
        routeStart = getRouteStart(currentRoute);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public ValidationInfo validatePath(List<string> myRoute)
    {
        gameManager.validationCheck = true;
        List<string> correctRoute = new List<string>();
        correctRoute = currentRoute.ToList();
        //foreach (string coord in currentRoute)
        //{
        //    correctRoute.Add(coord);
        //}
        correctRoute.RemoveAt(0);   //Remove the start point 

        Debug.Log("Checking route: " + string.Join(coordSeparator, correctRoute));

        
        bool isValid = false;
        bool hasError = false;
        //1. Check if route is correct
        if (correctRoute.Count() == myRoute.Count())     // are the routes the same lenght
        {
            //Debug.Log("Routes have SAME LENgHT");
            for (int i = 0; i < myRoute.Count(); i++)   //3. if route not correct, check where the error was
            {
                if (myRoute.ElementAt(i) != correctRoute.ElementAt(i))
                {
                    hasError = true;
                }    

            }

        }
        //else
        //{
        //    hasError = true;
        //}

        if (!hasError)
        {
            validationInfo.isValid = true;
            validationInfo.errorAt = 0;
            validationInfo.endReached = true;
        }
        else
        {
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

    private string[] getRouteStart(string[] route)
    {
        char _startDir = route[0][route[0].Length - 1];    //direcion is the last character
        string _startCoord = route[0].Remove(route[0].Length - 1);  //coord is route[0] minus last character

        //Debug.Log("Start Coord from currentRoute =" + _startCoord);
        //Debug.Log("Start Dir from currentRoute =" + _startDir);

        return new string[] { _startCoord, _startDir.ToString() };
    }
}
