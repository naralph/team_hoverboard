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

    //current values of rotation for checks on max
    float rotYCur = 0;
    float rotZCur = 0;

    //max angle values for the rotations
    public float rotYMax; //up down
    public float rotZMax; //Roll

    //speed multiplier for speed against, angle of board
    public float speedMultiplier = 10;

    //static float currRotation;
    private float rotationRate = 10.0f;

    private float minSpeed = 0.025f;
    private float maxSpeed = 10.0f;

    private Transform position;

    // Use this for initialization
    void Start()
    {
        position = GetComponent<Transform>();
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
            position.Rotate(Vector3.right * Input.GetAxis("RVertical"));
            //rotates about the y axis
            position.Rotate(Vector3.up * Input.GetAxis("LHorizontal"));
            //rotates about the z axis
            position.Rotate(Vector3.forward * Input.GetAxis("LHorizontal"));
            //rotates about the y axis
            position.Rotate(Vector3.up * Input.GetAxis("RHorizontal"));
            //translates forward
            position.Translate(Vector3.forward * Input.GetAxis("LVertical") * currSpeed);
        }
        else if(actualControls)
        {
            float pitch = (float)theGyro.pitchAngle;
            float roll = (float)theGyro.rollAngle;


            position.Rotate(Vector3.up * pitch); //left right
            

            position.Rotate(Vector3.right * roll); //up down
            rotYCur += roll;

            //position.Rotate(Vector3.back * pitch); //roll
            //rotZCur += pitch; //roll

           
            
            if (rotYCur > rotYMax || rotYCur < -rotYMax)
            {
                position.Rotate(Vector3.right * -roll);
                rotYCur -= roll;
            }
            
            //if (rotZCur > rotZMax || rotZCur < - rotZMax)
            //{
            //    position.Rotate(Vector3.back * -pitch);
            //    rotZCur -= pitch;
            //}


            position.Translate(Vector3.forward * currSpeed);
        }
        else
        {
            currSpeed += minSpeed;
            position.Translate(Vector3.forward * minSpeed);

            //leaning down on the board, accelerating
            if (Input.GetAxis("LVertical") < 0.0f && currSpeed > maxSpeed)
                currSpeed = maxSpeed;

            //leaning back on the board, decelerating
            if (Input.GetAxis("LVertical") > 0.0f && currSpeed < minSpeed)
                currSpeed = minSpeed;
        }
    }
}