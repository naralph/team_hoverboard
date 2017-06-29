using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public bool debugControls = false;
    public bool actualControls = true;
    public float moveRate = 50.0f;

    //objects to get down to getting the gyro object itself for data
    GameObject gameManagerObj;
    GameManager gameManagerScript;
    GyroManager gyroManager;
    SpatialData theGyro;

    //rotation values for gyro
    float pitch = 0;
    float rollOld = 0;

    //roll stabalization value
    public float rStabalizer = 0.2f;

    //speed multiplier for speed against, angle of board
    public float speedMultiplier = 10;

    //static float currRotation;
    private float rotationRate = 10.0f;

    private float minSpeed = 0.025f;
    private float maxSpeed = 10.0f;

    private Transform theTransform;

    // Use this for initialization
    void Start()
    {
        theTransform = GetComponent<Transform>();
        gameManagerObj = GameObject.Find("GameManager(Clone)");
        gameManagerScript = gameManagerObj.GetComponent<GameManager>();
        gyroManager = gameManagerScript.GetComponent<GyroManager>();
        theGyro = gyroManager.getGyro();
    }

    // Update is called once per frame
    void Update()
    {
        //GetAxis returns a floating value that is in-between -1 and 1
        float lVertVal = Input.GetAxis("LVertical");
        float lHoriVal = Input.GetAxis("LHorizontal");
        float currSpeed = moveRate * Time.deltaTime;

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
            theTransform.Translate(Vector3.forward * Input.GetAxis("LVertical") * currSpeed);
        }
        else if(actualControls)
        {
            pitch += (float)theGyro.pitchAngle;
            float roll = (float)theGyro.rollAngle * Mathf.Rad2Deg;

            float rollChange = ((rollOld - roll) * 0.5f);
            float rollUse = rollChange + roll;
            if (rollChange < rStabalizer)
            {
                rollUse = rollOld;
            }

            Vector3 rotation = new Vector3(rollUse, pitch, 0.0f);
            
            theTransform.rotation =
                Quaternion.Euler(roll, pitch, 0.0f);
            
            theTransform.Translate(Vector3.forward * currSpeed);
            rollOld = rollUse;
         }
        else
        {
            currSpeed += minSpeed;
            theTransform.Translate(Vector3.forward * minSpeed);

            //leaning down on the board, accelerating
            if (Input.GetAxis("LVertical") < 0.0f && currSpeed > maxSpeed)
                currSpeed = maxSpeed;

            //leaning back on the board, decelerating
            if (Input.GetAxis("LVertical") > 0.0f && currSpeed < minSpeed)
                currSpeed = minSpeed;
        }
    }
}