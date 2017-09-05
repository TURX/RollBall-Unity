using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset;
    //private Vector3 MouseDeltaPosition;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
        //MouseDeltaPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        //if (Input.GetMouseButtonDown(0))
        //{
        //Vector3 MoveCam = new Vector3((Input.mousePosition.x - MouseDeltaPosition.x) * 0.3f, transform.position.y, (Input.mousePosition.y - MouseDeltaPosition.y) * 0.3f);
        //transform.position = MoveCam;
        //}
    }

    

}
