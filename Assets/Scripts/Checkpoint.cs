﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameManager gameManager;
    private RouteManager routeManager;

    public string coordString;  // Intersection's coordinate in "x.y" format
    //public float[] coordinates;  // Intersection's coordinate in [x,y] format
    public int ID;
    public bool isCollected;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        routeManager = FindObjectOfType<GameManager>().GetComponent<RouteManager>();

        isCollected = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}