using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public float speed = 10f;
    private float z = -10f;

    private void Update()
    {
        // new Vector3(Mathf.Lerp(transform.position.x, player.transform.position.x, Time.deltaTime * speed), 
        // Mathf.Lerp(transform.position.y, player.transform.position.y, Time.deltaTime * speed), 
        //    z);
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, z);
    }
}
