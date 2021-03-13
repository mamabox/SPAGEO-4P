using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckpointManager : MonoBehaviour
{
    private GameManager gameManager;
    private RouteManager routeManager;
    public GameObject checkpointPrefab;
    private List<string> allCheckpoints;        // List of all checkpoints coordinates
    private int nextCheckpoint;                 // ID of the checkpoint that can be validated next
    public List<string> checkpointsText;        // Text import of text to display at each checkpoint

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        routeManager = FindObjectOfType<GameManager>().GetComponent<RouteManager>();

        nextCheckpoint = 1; //
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //GENERATE CHECKPOINTS PREFABS
    public void GenerateCheckpoints(List<string> checkpoints)
    {
        string[] coordArray;
        for (int i = 0; i < checkpoints.Count(); i++)
        {
            allCheckpoints = new List<string>(checkpoints);
            coordArray = checkpoints[i].Split(char.Parse(routeManager.coordSeparator));      //stores the coordinates x and y in an array
            //Debug.Log("Generating checkpoint at (" + coordArray[0] + coordSeparator + coordArray[1]);
            var newCheckpoint = Instantiate(checkpointPrefab, new Vector3(float.Parse(coordArray[0]) * gameManager.blockSize, 0.02f, float.Parse(coordArray[1]) * gameManager.blockSize), checkpointPrefab.transform.rotation);    //instantiate the checkpoint right above the ground
            newCheckpoint.GetComponent<Checkpoint>().coordString = checkpoints[i];  //store the coordinates as a string in the instance
            newCheckpoint.GetComponent<Checkpoint>().ID = i + 1;    //stores the checkpoitn number in the instance
        }
    }

    public void OnCheckpointEnter(Collider other)
    {
        Checkpoint checkpoint = other.GetComponent<Checkpoint>();
        Debug.Log(checkpointsText.ElementAt(checkpoint.ID-1));
        //CHECK CONDITIONS
        if (!checkpoint.isCollected)   // this checkpoint has not been collected
        {

            if (checkpoint.ID == nextCheckpoint) // this is the next valid checkpoint
            {
                Debug.Log("Checkpoint valid");
                checkpoint.isCollected = true;
                nextCheckpoint ++;
            }
            else if (nextCheckpoint == 1) // not the next valid checkpoint and not checkpoint has been collected
            {
                Debug.Log("Return to start");
                // return to start
            } else
            {
                Debug.Log("Return to previous checkpoint");
                // go to the previous checkpoint
            }
        }

        
    }

    void OnCheckpointExit(Collider other)
    {

    }
}
