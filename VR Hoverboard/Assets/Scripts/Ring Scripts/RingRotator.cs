﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//parent this gameobject to any rings that you want to rotate
//don't forget to set the individual ring properties and positions
public class RingRotator : MonoBehaviour
{
    Transform anchor;
    public float rotateRate = 5f;

	// Use this for initialization
	void Start ()
    {
        anchor = GetComponent<Transform>();
        GetComponent<MeshRenderer>().enabled = false;      
	}
	
	// Update is called once per frame
	void Update ()
    {
        anchor.Rotate(Vector3.forward, Time.deltaTime * rotateRate);
	}
}
