using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    bool debugControls = false;
    bool playerMovementLocked = false;

    float currSpeed = 0.0f;
    float pitch, yaw, roll;
    Rigidbody theRigidbody;
    SpatialData gyro;

    ManagerClasses.GyroMovementVariables gmv;
    ManagerClasses.DebugMovementVariables dmv;

    void SetPlayerMovementLock(bool locked)
    {
        //make sure we have a different value
        if (locked != playerMovementLocked)
        {
            if (!locked && !debugControls)
            {
                StopCoroutine(GyroMovementCoroutine());
                StartCoroutine(GyroMovementCoroutine());
            }

            playerMovementLocked = locked;
        }           
    }

    public void SetupMovementScript(bool debugCon, ManagerClasses.GyroMovementVariables g, ManagerClasses.DebugMovementVariables d)
    {
        debugControls = debugCon;
        gmv = g;
        dmv = d;

        theRigidbody = GetComponent<Rigidbody>();

        if (debugControls)
            StartCoroutine(DebugMovementCoroutine());
        else
        {
            gyro = new SpatialData();

            //since the information we are getting from the gyro is in radians, include Mathf.Rad2Deg in our sensitivities
            gmv.pitchSensitivity *= Mathf.Rad2Deg;
            gmv.yawSensitivity *= Mathf.Rad2Deg;

            //initialize our currSpeed as the average of min and max speed
            currSpeed = (gmv.minSpeed + gmv.maxSpeed) * 0.5f;

            //adjust our max ascend value for easier use in our GyroMovementCoroutine           
            gmv.maxAscendAngle = 360 - gmv.maxAscendAngle;

            //adjust our pitch and yaw sensitivities
            gmv.pitchSensitivity *= 0.01f;
            gmv.yawSensitivity *= 0.01f * -1f;

            //adjust our decelerate and accelerate rates
            gmv.decelerateRate *= .001f;
            gmv.accelerateRate *= .001f;

            StartCoroutine(GyroMovementCoroutine());
        }
    }

    //Note: debug rotation controls are needed for menu interaction
    IEnumerator DebugMovementCoroutine()
    {
        yield return new WaitForFixedUpdate();

        pitch = theRigidbody.rotation.eulerAngles.x + Input.GetAxis("LVertical") * dmv.pitchSensitivity;
        yaw = theRigidbody.rotation.eulerAngles.y + Input.GetAxis("LHorizontal") * dmv.yawSensitivity; 
        roll = 0.0f;

        theRigidbody.rotation = Quaternion.Euler(new Vector3(pitch, yaw, roll));

        if (!playerMovementLocked)
            theRigidbody.velocity = theRigidbody.transform.forward * dmv.startSpeed;

        StartCoroutine(DebugMovementCoroutine());
    }

    IEnumerator GyroMovementCoroutine()
    {
        while (!playerMovementLocked)
        {
            yield return new WaitForFixedUpdate();

            pitch = theRigidbody.rotation.eulerAngles.x + (float)gyro.rollAngle * gmv.pitchSensitivity;
            yaw = theRigidbody.rotation.eulerAngles.y + (float)gyro.pitchAngle * gmv.yawSensitivity;
            roll = 0.0f;

            //for our gyro, 0 is resting position
            //when angled up, degrees go down from 360 to 0, depending on the degree of the angle
            //when angled down, derees go up from 0 to 360, depending on the degree of the angle
            //we can think of the degrees of 0 to 180 as pointing down, and 180 to 360 as pointing up

            //pointing down
            //if (pitch < 180.0f)
            //{
            //    if (pitch > gmv.maxAscendAngle)
            //        pitch = gmv.maxAscendAngle;

            //    //calculate deceleration depending on the angle
            //    //float angleDifference = pitch - gmv.maxAscendAngle;

            //    currSpeed = Mathf.Lerp(currSpeed, gmv.minSpeed, gmv.decelerateRate);
            //}
            ////pointing up
            //else
            //{
            //    if (pitch < gmv.maxDescendAngle)
            //        pitch = gmv.maxDescendAngle;

            //    //float angleDifference = gmv.maxDescendAngle - pitch;
            //    currSpeed = Mathf.Lerp(currSpeed, gmv.maxSpeed, gmv.accelerateRate);

            //    //print("AngleDifference: " + angleDifference);
            //    //print("Pitch: " + pitch);
            //    //print("Speed: " + currSpeed);
            //    //print("Decelerated speed: " + (angleDifference * gmv.decelerateRate));
            //}

            Vector3 vec = new Vector3(pitch, yaw, 0f);
            theRigidbody.rotation = (Quaternion.Euler(vec));
            //theRigidbody.velocity = theRigidbody.transform.forward * currSpeed;
            //print("Curr Speed: " + currSpeed);
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