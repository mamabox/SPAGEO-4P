using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //used for from - in - select syntax

public class IntersectionManager : MonoBehaviour
{
    public GameObject intersectionPrefab;
    private GameManager gameManager;
    public string enteredFrom; // Cardinal direction from which the last intersection was entered

    //public List<string> validCoord = new List<string>();

    // Start is called before the first frame update

    [ContextMenu("Intersection Manager")] //To debug without needing to play
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        GenerateIntersections();

    }

    // Update is called once per frame
    void Update()
    {

    }

   
    void GenerateIntersections()
    {
        // Y = -4 to -1
        for (int i = 4; i <= 11; i++)
        {
            for (int j = -4; j <= -1; j++)
            {
                //Debug.Log(i + "." + j);
                //gameManager.validCoord.Add(i + "." + j);
                addPrefab(i, j);
            }
        }

        // Y = 0 to 3
        for (int i = 0; i <= 11; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                //Debug.Log(i + "." + j);
                //gameManager.validCoord.Add(i + "." + j);
                addPrefab(i, j);
            }
        }
        // Y = 4 to 7
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 4; j <= 7; j++)
            {
                //Debug.Log(i + "." + j);
                //gameManager.validCoord.Add(i + "." + j);
                addPrefab(i, j);
            }
        }

        // OPTION 2 - Debug array in one line
        Debug.Log(string.Join(", ", from coord in gameManager.validCoord select coord)); //could use coord.attribtre

        ////OPTION 1 - Debug with one item per list
        //foreach (string coord in gameManager.validCoord)
        //{
        //    Debug.Log(coord);
        //}
    }

    public void addPrefab(int x, int z)
    {
        Instantiate(intersectionPrefab, new Vector3(x * gameManager.blockSize, 1, z * gameManager.blockSize), intersectionPrefab.transform.rotation);
        gameManager.validCoord.Add(x + "." + z);    //add the prefab's coordinate to the list of valid coordinates
    }

}

