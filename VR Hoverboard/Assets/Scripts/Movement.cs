﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    bool controllerEnabled = false;
    bool playerMovementLocked = false;

    float currSpeed = 0.0f;
    float pitch, yaw;
    Rigidbody theRigidbody;
    SpatialData gyro;

    ManagerClasses.GyroMovementVariables gmv;
    ManagerClasses.ControllerMovementVariables cmv;

    void SetPlayerMovementLock(bool locked)
    {
        //make sure we have a different value
        if (locked != playerMovementLocked)
        {
            //if we aren't locked
            if (!locked)
            {
                //don't start our coroutine if we aren't using the gyro
                if (!controllerEnabled)
                {
                    //be sure to not have multiple instances of the gyro coroutine going
                    StopCoroutine(GyroMovementCoroutine());
                    StartCoroutine(GyroMovementCoroutine());
                }
            }
            else
                //if we're locking movement, then set the velocity to zero
                theRigidbody.velocity = Vector3.zero;

            playerMovementLocked = locked;
        }
    }

    public void SetupMovementScript(bool cEnabled, ManagerClasses.GyroMovementVariables g, ManagerClasses.ControllerMovementVariables c)
    {
        controllerEnabled = cEnabled;
        gmv = g;
        cmv = c;

        theRigidbody = GetComponent<Rigidbody>();

        if (controllerEnabled)
            StartCoroutine(ControllerMovementCoroutine());
        else
        {
            gyro = new SpatialData();

            //since the information we are getting from the gyro is in radians, include Mathf.Rad2Deg in our sensitivities
            gmv.pitchSensitivity *= Mathf.Rad2Deg;
            gmv.yawSensitivity *= Mathf.Rad2Deg;

            //initialize our currSpeed as the average of min and max speed
            currSpeed = gmv.startSpeed;

            //adjust our max ascend value for easier use in our GyroMovementCoroutine           
            gmv.maxAscendAngle = 360 - gmv.maxAscendAngle;

            //adjust our pitch and yaw sensitivities
            gmv.pitchSensitivity *= 0.01f;
            gmv.yawSensitivity *= 0.01f * -1f;

            //adjust our decelerate and accelerate rates
            gmv.decelerateRate *= .001f;
            gmv.accelerateRate *= .001f;

            StartCoroutine(GyroMovementCoroutine());
            //enable the debug coroutine so you can move the camera around with the left joystick, even when using the gyro
            StartCoroutine(ControllerMovementCoroutine());
        }
    }

    //Note: debug rotation controls are needed for menu interaction
    IEnumerator ControllerMovementCoroutine()
    {
        yield return new WaitForFixedUpdate();

        pitch = theRigidbody.rotation.eulerAngles.x + Input.GetAxis("LVertical") * cmv.pitchSensitivity;
        yaw = theRigidbody.rotation.eulerAngles.y + Input.GetAxis("LHorizontal") * cmv.yawSensitivity;

        theRigidbody.rotation = Quaternion.Euler(new Vector3(pitch, yaw, 0f));

        if (!playerMovementLocked)
            theRigidbody.velocity = theRigidbody.transform.forward * cmv.startSpeed;

        StartCoroutine(ControllerMovementCoroutine());
    }

    IEnumerator GyroMovementCoroutine()
    {
        while (!playerMovementLocked)
        {
            yield return new WaitForFixedUpdate();

            pitch = theRigidbody.rotation.eulerAngles.x + (float)gyro.rollAngle * gmv.pitchSensitivity;
            yaw = theRigidbody.rotation.eulerAngles.y + (float)gyro.pitchAngle * gmv.yawSensitivity;

            if (pitch < 180.0f)
            {
                if (pitch > gmv.maxDescendAngle)
                    pitch = gmv.maxDescendAngle;

                //calculate deceleration depending on the angle
                //float angleDifference = pitch - gmv.maxAscendAngle;

                currSpeed = Mathf.Lerp(currSpeed, gmv.maxSpeed, gmv.accelerateRate);
            }
            else
            {
                if (pitch < gmv.maxAscendAngle)
                    pitch = gmv.maxAscendAngle;

                currSpeed = Mathf.Lerp(currSpeed, gmv.minSpeed, gmv.decelerateRate);
            }

            theRigidbody.rotation = (Quaternion.Euler(new Vector3(pitch, yaw, 0f)));
            //theRigidbody.AddRelativeTorque()
            theRigidbody.velocity = theRigidbody.transform.forward * currSpeed;
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
        if (!controllerEnabled)
            gyro.Close();
    }
}