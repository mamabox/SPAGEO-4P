using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 cameraTransformOffset = new Vector3(0, 0.75f, 0); //Camera offset from player
    private int cameraRotationOffset = -5; //Camera offset from player
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + cameraTransformOffset; // Sets camera to player movement + offset
        //transform.rotation = player.transform.rotation; //Sets camera to player's rotation

        if (player.GetComponent<PlayerController>().cameraTilt)
        {
            transform.rotation = Quaternion.Euler(cameraRotationOffset, player.transform.eulerAngles.y, 0);
        }
        else
        {
            transform.rotation = player.transform.rotation;
        }
        
    }
}
