using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameManager gameManager;
    private RouteManager routeManager;

    public string coordString;  // Intersection's coordinate in "x.y" format
    //public float[] coordinates;  // Intersection's coordinate in [x,y] format
    public int checkpointID;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        routeManager = FindObjectOfType<GameManager>().GetComponent<RouteManager>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}