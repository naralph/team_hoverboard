﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    bool debugControls = false;
    public float moveRate = 50.0f;

    //objects to get down to getting the gyro object itself for data
    GameObject gameManagerObj;
    GameManager gameManagerScript;
    GyroManager gyroManager;
    SpatialData theGyro;

    [SerializeField] float currSpeed = 0;

    //rotation values for gyro
    [SerializeField] float pitch = 0;
    [SerializeField] float roll = 0;

    //maximum angle for up down
    public float rollMax = 60.0f;

    //speed multiplier for speed against, angle of board
    public float speedDownMultiplier = 0.01f;
    public float speedUpMultiplier = 0.01f;

    public float minSpeed = 0.025f;
    public float maxSpeed = 10.0f;

    private Transform theTransform;

    void usingDebugControls()
    {
        debugControls = true;
    }

    // Use this for initialization
    void Start()
    {
        //TODO:: fix the PhidgetException..... but not this way
        //if (!actualControls)
        //    EventManager.OnNotUsingActualControls();

        theTransform = GetComponent<Transform>();
        gameManagerObj = GameObject.Find("GameManager");
        gameManagerScript = gameManagerObj.GetComponent<GameManager>();
        if (!debugControls)
        {
            gyroManager = gameManagerScript.GetComponent<GyroManager>();
            theGyro = gyroManager.getGyro();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //GetAxis returns a floating value that is in-between -1 and 1
        float lVertVal = Input.GetAxis("LVertical");
        float lHoriVal = Input.GetAxis("LHorizontal");
        currSpeed = moveRate * Time.deltaTime;

        //print(lVertVal + " LEFT VERT JOYSTICK VAL");
        //print(lHoriVal + " LEFT HORIZONTAL JOYSTICK VAL");
        //print(currSpeed + " SPEED VAL");

        if (debugControls)
        {
            //rotates about the x axis
            theTransform.Rotate(Vector3.right * Input.GetAxis("RVertical"));
            //rotates about the y axis
            theTransform.Rotate(Vector3.up * Input.GetAxis("LHorizontal"));
            //rotates about the z axis
            theTransform.Rotate(Vector3.forward * Input.GetAxis("LHorizontal"));
            //rotates about the y axis
            theTransform.Rotate(Vector3.up * Input.GetAxis("RHorizontal"));
            //translates forward
            theTransform.Translate(Vector3.forward * Input.GetAxis("LVertical"));
        }
        else 
        {
            float pitchChange = (float)theGyro.pitchAngle;
            float rollChange = (float)theGyro.rollAngle;

            pitch += pitchChange;
            roll += rollChange;

            //check to make sure not at to steep an angle using our max variable
            if (roll > rollMax || roll < -rollMax)
            {
                roll -= (float)theGyro.rollAngle;
            }

            //preform the actual rotation on the object
            theTransform.rotation =
                Quaternion.Euler(roll, pitch, 0.0f);

            rollChange *= Mathf.Rad2Deg;

            //speed check based on the angle you are at, steeper down equals faster, and steeper up means slower
           
            currSpeed += (speedUpMultiplier * rollChange);
            if (currSpeed > maxSpeed)
            {
                currSpeed = maxSpeed;
            }
            if (currSpeed < minSpeed)
            {
                currSpeed = minSpeed;
            }
            

            //actual movment of the player
            theTransform.Translate(Vector3.forward * currSpeed);
        }
    }

    void OnEnable()
    {
        EventManager.OnDisableActualControls += usingDebugControls;
    }

    void OnDisable()
    {
        EventManager.OnDisableActualControls -= usingDebugControls;
    }
}