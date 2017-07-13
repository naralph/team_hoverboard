using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    bool debugControls = false;
    bool playerMovementLocked = false;

    float currSpeed = 0.0f;
    float Rad2DegPitchSensativity, Rad2DegYawSensativity;
    float pitch, yaw, roll;
    Transform theTransform;
    SpatialData gyro;

    ManagerClasses.GyroMovementVariables gmv;

    void SetPlayerMovementLock(bool locked)
    {
        //make sure we have a different value
        if (locked != playerMovementLocked)
        {
            if (!locked && !debugControls)
                StartCoroutine(GyroMovementCoroutine());

            playerMovementLocked = locked;
        }           
    }

    public void SetupMovementScript(bool debugCon, ManagerClasses.GyroMovementVariables g)
    {
        debugControls = debugCon;
        gmv = g;

        theTransform = GetComponent<Transform>();      

        if (debugControls)
            StartCoroutine(DebugMovementCoroutine());
        else
        {
            gyro = new SpatialData();

            //adjust our sensitivities
            //  since the information we are getting from the gyro is in radians, include Mathf.Rad2Deg
            Rad2DegPitchSensativity = Mathf.Rad2Deg *  gmv.pitchSensitivity;
            Rad2DegYawSensativity = Mathf.Rad2Deg * gmv.yawSensitivity;

            //initialize our currSpeed
            currSpeed = gmv.startSpeed * Time.deltaTime;

            //adjust our max ascend value for easier use in our GyroMovementCoroutine           
            gmv.maxAscendAngle = 360 - gmv.maxAscendAngle;

            StartCoroutine(GyroMovementCoroutine());
        }
    }

    //Note: debug rotation controls are needed for menu interaction
    IEnumerator DebugMovementCoroutine()
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
        if (!playerMovementLocked)
            theTransform.Translate(Vector3.forward * Input.GetAxis("LVertical"));

        yield return null;
        StartCoroutine(DebugMovementCoroutine());
    }

    IEnumerator GyroMovementCoroutine()
    {
        while (!playerMovementLocked)
        {
            theTransform.Translate(Vector3.forward * currSpeed);

            pitch = theTransform.eulerAngles.x + (float)gyro.rollAngle * Rad2DegPitchSensativity;
            yaw = theTransform.eulerAngles.y + (float)gyro.pitchAngle * Rad2DegYawSensativity;
            roll = 0.0f;

            //for our gyro, 0 is resting position
            //when angled up, degrees go down from 360 to 0, depending on the degree of the angle
            //when angled down, derees go up from 0 to 360, depending on the degree of the angle
            //we can think of the degrees of 0 to 180 as pointing down, and 180 to 360 as pointing up

            //pointing up
            if (pitch > 180.0f)
            {
                if (pitch < gmv.maxAscendAngle)
                    pitch = gmv.maxAscendAngle;

                //calculate deceleration depending on the angle
                //float angleDifference = pitch - gmv.maxAscendAngle;
                //currSpeed *= angleDifference * gmv.decelerateRate;

                currSpeed = Mathf.Lerp((gmv.decelerateRate * Time.deltaTime), currSpeed, gmv.decelerateRate);
            }
            //pointing down
            else
            {
                if (pitch > gmv.maxDescendAngle)
                    pitch = gmv.maxDescendAngle;

                float angleDifference = gmv.maxDescendAngle - pitch;


                //print("AngleDifference: " + angleDifference);
                //print("Pitch: " + pitch);
                //print("Speed: " + currSpeed);
                //print("Decelerated speed: " + (angleDifference * gmv.decelerateRate));
            }

            print("Speed: " + currSpeed);

            theTransform.rotation = Quaternion.Euler(new Vector3(pitch, yaw, roll));
            yield return null;
        }
    }

    void OnEnable()
    {
        EventManager.OnToggleMovement += SetPlayerMovementLock;
    }

    void OnDisable()
    {
        EventManager.OnToggleMovement -= SetPlayerMovementLock;
    }

    //cleanup our gyroscope so we don't crash
    private void OnApplicationQuit()
    {
        if (!debugControls)
            gyro.Close();
    }
}