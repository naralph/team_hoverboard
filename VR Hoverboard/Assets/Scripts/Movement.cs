using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    bool debugControls = false;
    bool lockPlayerMovement = false;

    public float moveRate = 50.0f;

    //objects to get down to getting the gyro object itself for data
    GyroManager gyroManager;

    #region assan's code
    //SpatialData theGyro;
    //[SerializeField] float currSpeed = 0;

    ////rotation values for gyro
    //[SerializeField] float pitch = 0;
    //[SerializeField] float roll = 0;

    ////maximum angle for up down
    //public float rollMax = 60.0f;

    ////speed multiplier for speed against, angle of board
    //public float speedDownMultiplier = 0.01f;
    //public float speedUpMultiplier = 0.01f;

    //public float minSpeed = 0.025f;
    //public float maxSpeed = 10.0f;
    #endregion

    float currSpeed = 0.0f;

    Transform theTransform;

    void usingDebugControls()
    {
        debugControls = true;
    }

    // Use this for initialization
    void Start()
    {
        theTransform = GetComponent<Transform>();
    }

    public void AssignManager(GyroManager gMan)
    {
        if (!debugControls)
            gyroManager = gMan;
    }

    // Update is called once per frame
    void Update()
    {
        if (!lockPlayerMovement)
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

            //nate's code
            else
            {
                theTransform.Translate(Vector3.forward * currSpeed);

                //theTransform.Rotate(Vector3.right * (float)theGyro.rollAngle * Mathf.Rad2Deg);
                //theTransform.Rotate(Vector3.forward * (float)theGyro.pitchAngle * Mathf.Rad2Deg);

                Vector3 vec = new Vector3(
                    (float)gyroManager.getGyro().rollAngle * Mathf.Rad2Deg, 
                    theTransform.rotation.y, 
                    (float)gyroManager.getGyro().pitchAngle * Mathf.Rad2Deg);


                theTransform.rotation = Quaternion.Euler(vec);

                //print("X ROTATION: " + vec.x);
                //print("Y ROTATION: " + vec.y);
                //print("Z ROTATION: " + vec.z);
            }

            #region assan's code
            //else if ()
            //{
            //    float pitchChange = (float)theGyro.pitchAngle;
            //    float rollChange = (float)theGyro.rollAngle;

            //    pitch += pitchChange;
            //    roll += rollChange;

            //    //check to make sure not at to steep an angle using our max variable
            //    if (roll > rollMax || roll < -rollMax)
            //    {
            //        roll -= (float)theGyro.rollAngle;
            //    }

            //    //preform the actual rotation on the object
            //    theTransform.rotation =
            //        Quaternion.Euler(roll, pitch, 0.0f);

            //    rollChange *= Mathf.Rad2Deg;

            //    //speed check based on the angle you are at, steeper down equals faster, and steeper up means slower

            //    currSpeed += (speedUpMultiplier * rollChange);
            //    if (currSpeed > maxSpeed)
            //    {
            //        currSpeed = maxSpeed;
            //    }
            //    if (currSpeed < minSpeed)
            //    {
            //        currSpeed = minSpeed;
            //    }


            //    //actual movment of the player
            //    theTransform.Translate(Vector3.forward * currSpeed);
            //}
            #endregion
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