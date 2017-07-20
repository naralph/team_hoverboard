using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    bool controllerEnabled = false;
    bool playerMovementLocked = true;

    float pitch, yaw;
    Rigidbody theRigidbody;
    SpatialData gyro;

    ManagerClasses.PlayerMovementVariables movementVariables;

    void SetPlayerMovementLock(bool locked)
    {
        //make sure we have a different value
        if (locked != playerMovementLocked)
        {
            //if we aren't locked
            if (!locked)
            {
                print("Player Movement UNLOCKED!");

                //don't start our coroutine if we aren't using the gyro
                if (!controllerEnabled)
                {
                    //be sure to not have multiple instances of the gyro coroutine going
                    StopCoroutine(GyroMovementCoroutine());
                    StartCoroutine(GyroMovementCoroutine());
                }
            }
            else
            {
                //if we're locking movement, then set the velocity to zero
                theRigidbody.velocity = Vector3.zero;
                StopCoroutine(GyroMovementCoroutine());

                print("Player Movement LOCKED!");
            }

            playerMovementLocked = locked;

        }
    }

    public void SetupMovementScript(bool cEnabled, ManagerClasses.PlayerMovementVariables variables)
    {
        controllerEnabled = cEnabled;

        theRigidbody = GetComponent<Rigidbody>();
        movementVariables = variables;

        if (controllerEnabled)
            StartCoroutine(ControllerMovementCoroutine());
        else
        {
            gyro = new SpatialData();

            //since the information we are getting from the gyro is in radians, include Mathf.Rad2Deg in our sensitivities
            movementVariables.pitchSensitivity *= Mathf.Rad2Deg;
            movementVariables.yawSensitivity *= Mathf.Rad2Deg;

            //adjust our max ascend value for easier use in our GyroMovementCoroutine           
            movementVariables.maxAscendAngle = 360 - movementVariables.maxAscendAngle;

            //adjust our pitch and yaw sensitivities
            movementVariables.pitchSensitivity *= 0.01f;
            movementVariables.yawSensitivity *= 0.01f * -1f;

            //StartCoroutine(GyroMovementCoroutine());
        }
    }

    void ClampPitch()
    {
        //pitch rests at 0 degrees
        //when decending pitch travels in a positive direction (from 0 to 360)
        //when ascending pitch travels in a negative direction (from 360 to 0)

        //descending
        if (pitch < 180f)
        {
            if (pitch > movementVariables.maxDescendAngle)
                pitch = movementVariables.maxDescendAngle;
        }
        //ascending
        else
        {
            if (pitch < 360f - movementVariables.maxAscendAngle)
                pitch = 360f - movementVariables.maxAscendAngle;
        }

        //print("Pitch: " + pitch);
        //print("Yaw:   " + yaw);
    }

    void ApplyForce()
    {
        //if restingThreshold were set to 10
        //         pitch > 350 or pitch < 10
        if (!playerMovementLocked)
        {
            if (pitch > 360f - movementVariables.restingThreshold || pitch < movementVariables.restingThreshold)
            {
                theRigidbody.AddRelativeForce(Vector3.forward * movementVariables.restingSpeed, ForceMode.Acceleration);
                //print("In resting threshold!");
            }
            else if (pitch < 180f)
                theRigidbody.AddRelativeForce(Vector3.forward * movementVariables.maxSpeed, ForceMode.Acceleration);
            else
                theRigidbody.AddRelativeForce(Vector3.forward * movementVariables.minSpeed, ForceMode.Acceleration);
        }



    }

    //Note: debug rotation controls are needed for menu interaction
    IEnumerator ControllerMovementCoroutine()
    {
        yield return new WaitForFixedUpdate();

        pitch = theRigidbody.rotation.eulerAngles.x + Input.GetAxis("LVertical") * movementVariables.pitchSensitivity;
        yaw = theRigidbody.rotation.eulerAngles.y + Input.GetAxis("LHorizontal") * movementVariables.yawSensitivity;

        ClampPitch();
        ApplyForce();

        theRigidbody.rotation = Quaternion.Euler(new Vector3(pitch, yaw, 0f));

        StartCoroutine(ControllerMovementCoroutine());
    }

    IEnumerator GyroMovementCoroutine()
    {
        print("started gyro movement");

        yield return new WaitForFixedUpdate();

        //rotate using the gyro
        pitch = theRigidbody.rotation.eulerAngles.x + (float)gyro.rollAngle * movementVariables.pitchSensitivity;
        yaw = theRigidbody.rotation.eulerAngles.y + (float)gyro.pitchAngle * movementVariables.yawSensitivity;

        ClampPitch();
        ApplyForce();

        theRigidbody.rotation = (Quaternion.Euler(new Vector3(pitch, yaw, 0f)));

        StartCoroutine(GyroMovementCoroutine());
    }

    private void OnCollisionEnter(Collision collision)
    {
        //scale our impulse by our bounce amount
        theRigidbody.AddForce(collision.impulse * movementVariables.bounceModifier, ForceMode.Impulse);
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