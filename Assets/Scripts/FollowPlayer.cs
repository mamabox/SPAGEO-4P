using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 cameraOffset = new Vector3(0, 0.75f, 0); //Camera offset from player
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + cameraOffset; // Sets camera to player movement + offset
        transform.rotation = player.transform.rotation; //Sets camera to player's rotation
    }
}
