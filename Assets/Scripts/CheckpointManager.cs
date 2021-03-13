using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckpointManager : MonoBehaviour
{
    private GameManager gameManager;
    private RouteManager routeManager;
    public GameObject checkpointPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        routeManager = FindObjectOfType<GameManager>().GetComponent<RouteManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateCheckpoints(List<string> checkpoints)
    {
        string[] coordArray;
        for (int i = 0; i < checkpoints.Count(); i++)
        {
            coordArray = checkpoints[i].Split(char.Parse(routeManager.coordSeparator));      //stores the coordinates x and y in an array
            //Debug.Log("Generating checkpoint at (" + coordArray[0] + coordSeparator + coordArray[1]);
            var newCheckpoint = Instantiate(checkpointPrefab, new Vector3(float.Parse(coordArray[0]) * gameManager.blockSize, 0.02f, float.Parse(coordArray[1]) * gameManager.blockSize), checkpointPrefab.transform.rotation);    //instantiate the checkpoint right above the ground
            newCheckpoint.GetComponent<Checkpoint>().coordString = checkpoints[i];  //store the coordinates as a string in the instance
            newCheckpoint.GetComponent<Checkpoint>().checkpointID = i + 1;    //stores the checkpoitn number in the instance
        }
    }

    public void OnCheckpointEnter(Collider other)
    {

    }

    void OnCheckpointExit(Collider other)
    {

    }
}
