using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    private GameManager gameManager;

    public string coordString;  // Intersection's coordinate in "x.y" format
    public float[] coordinates;  // Intersection's coordinate in [x,y] format
    public int instanceID;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        coordinates[0] = (transform.position.x / gameManager.blockSize);
        coordinates[1] = (transform.position.z / gameManager.blockSize);
        coordString = coordinates[0] + "," + coordinates[1];
    }

    // Update is called once per frame
    void Update()
    {

    }
}