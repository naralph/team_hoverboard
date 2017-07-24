using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool controllerEnabled = false;
    bool playerMovementLocked = true;

    float pitch, yaw;
    Rigidbody playerRigidbody;
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
                    StopAllCoroutines();
                    StartCoroutine(GyroMovementCoroutine());
                }
            }
            else
            {
                print("Player Movement LOCKED!");

                //if we're locking movement, then set the velocity to zero
                playerRigidbody.velocity = Vector3.zero;

                if (!controllerEnabled)
                    StopAllCoroutines();
            }
            playerMovementLocked = locked;
        }
    }

    public void SetupMovementScript(bool cEnabled, ManagerClasses.PlayerMovementVariables variables)
    {
        controllerEnabled = cEnabled;
        movementVariables = variables;

        playerRigidbody = GetComponent<Rigidbody>();

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
            if (pitch < movementVariables.maxAscendAngle)
                pitch = movementVariables.maxAscendAngle;
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
                playerRigidbody.AddRelativeForce(Vector3.forward * movementVariables.restingSpeed, ForceMode.Acceleration);
                //print("In resting threshold!");
            }
            else if (pitch < 180f)
                playerRigidbody.AddRelativeForce(Vector3.forward * movementVariables.maxSpeed, ForceMode.Acceleration);
            else
                playerRigidbody.AddRelativeForce(Vector3.forward * movementVariables.minSpeed, ForceMode.Acceleration);
        }
    }

    //Note: debug rotation controls are needed for menu interaction
    IEnumerator ControllerMovementCoroutine()
    {
        yield return new WaitForFixedUpdate();

        pitch = playerRigidbody.rotation.eulerAngles.x + Input.GetAxis("LVertical") * movementVariables.pitchSensitivity;
        yaw = playerRigidbody.rotation.eulerAngles.y + Input.GetAxis("LHorizontal") * movementVariables.yawSensitivity;

        ClampPitch();
        ApplyForce();

        playerRigidbody.rotation = Quaternion.Euler(new Vector3(pitch, yaw, 0f));

        StartCoroutine(ControllerMovementCoroutine());
    }

    IEnumerator GyroMovementCoroutine()
    {
        yield return new WaitForFixedUpdate();

        //rotate using the gyro
        pitch = playerRigidbody.rotation.eulerAngles.x + (float)gyro.rollAngle * movementVariables.pitchSensitivity;
        yaw = playerRigidbody.rotation.eulerAngles.y + (float)gyro.pitchAngle * movementVariables.yawSensitivity;

        ClampPitch();
        ApplyForce();

        playerRigidbody.rotation = (Quaternion.Euler(new Vector3(pitch, yaw, 0f)));

        StartCoroutine(GyroMovementCoroutine());
    }

    private void OnCollisionEnter(Collision collision)
    {
        //scale our impulse by our bounce amount
        playerRigidbody.AddForce(collision.impulse * movementVariables.bounceModifier, ForceMode.Impulse);
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