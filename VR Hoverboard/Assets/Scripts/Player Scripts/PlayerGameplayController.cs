using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayController : MonoBehaviour
{
    bool gamepadEnabled = false;
    bool playerMovementLocked = true;

    float pitch, yaw;
    Rigidbody playerRigidbody;
    SpatialData gyro;

    BoardManager bMan;
    ManagerClasses.PlayerMovementVariables movementVariables;

    //called by our BoardManager
    public void SetupGameplayControllerScript()
    {
        bMan = GameManager.instance.boardScript;

        gamepadEnabled = bMan.gamepadEnabled;
        playerRigidbody = GetComponent<Rigidbody>();
        gyro = bMan.gyro;

        UpdateMovementVariables(bMan.BoardSelect(bMan.currentBoardSelection));
    }

    //function to subscribe to the OnToggleMovement event
    void SetPlayerMovementLock(bool locked)
    {
        //if we aren't locked
        if (!locked)
        {
            print("Player Gameplay Controller UNLOCKED!");

            //be sure to not have multiple instances of our coroutines going
            StopAllCoroutines();

            if (gamepadEnabled)
                StartCoroutine(ControllerMovementCoroutine());
            else
                StartCoroutine(GyroMovementCoroutine());
        }
        else
        {
            print("Player Gameplay Controller LOCKED!");
            StopAllCoroutines();

            //if we're locking movement, then set the velocity to zero
            playerRigidbody.velocity = Vector3.zero;
        }

        playerMovementLocked = locked;
    }

    //update our script depending on if we are using a xbox gamepad or the gyro
    //  Note: this should normally not be directly called, instead call the BoardManager's UpdateControlsType()
    public void UpdateGameplayControlsType(bool gEnabled)
    {
        gamepadEnabled = gEnabled;

        //if our movement isn't locked, update what coroutine we are using
        if (!playerMovementLocked)
        {
            StopAllCoroutines();

            if (gamepadEnabled)
                StartCoroutine(ControllerMovementCoroutine());
            else
                StartCoroutine(GyroMovementCoroutine());
        }

        UpdateMovementVariables(bMan.BoardSelect(bMan.currentBoardSelection));
    }

    //updates our movement variables
    //  Note: UpdateGameplayControlsType() already calls this function
    public void UpdateMovementVariables(ManagerClasses.PlayerMovementVariables variables)
    {
        movementVariables = variables;

        playerRigidbody.mass = movementVariables.mass;
        playerRigidbody.drag = movementVariables.drag;
        playerRigidbody.angularDrag = movementVariables.angularDrag;

        //adjust our max ascend value for easier use in ClampPitch()
        movementVariables.maxAscendAngle = 360 - movementVariables.maxAscendAngle;

        if (!gamepadEnabled)
        {
            //since the information we are getting from the gyro is in radians, include Mathf.Rad2Deg in our sensitivities
            movementVariables.pitchSensitivity *= Mathf.Rad2Deg;
            movementVariables.yawSensitivity *= Mathf.Rad2Deg;

            //adjust our pitch and yaw sensitivities
            movementVariables.pitchSensitivity *= 0.01f;
            movementVariables.yawSensitivity *= 0.01f * -1f;
        }
    } 

    //helper function
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

    //helper function
    void ApplyForce()
    {
        if (!playerMovementLocked)
        {
            //if restingThreshold were set to 10
            //         pitch > 350 or pitch < 10
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

    IEnumerator ControllerMovementCoroutine()
    {
        yield return new WaitForFixedUpdate();

        pitch = playerRigidbody.rotation.eulerAngles.x + Input.GetAxis("LVertical") * movementVariables.pitchSensitivity;
        yaw = playerRigidbody.rotation.eulerAngles.y + Input.GetAxis("LHorizontal") * movementVariables.yawSensitivity;

        ClampPitch();
        ApplyForce();

        //since we don't want to make our player sick, make sure we never roll the camera
        playerRigidbody.rotation = Quaternion.Euler(new Vector3(pitch, yaw, 0f));

        StartCoroutine(ControllerMovementCoroutine());
    }

    IEnumerator GyroMovementCoroutine()
    {
        yield return new WaitForFixedUpdate();

        pitch = playerRigidbody.rotation.eulerAngles.x + (float)gyro.rollAngle * movementVariables.pitchSensitivity;
        yaw = playerRigidbody.rotation.eulerAngles.y + (float)gyro.pitchAngle * movementVariables.yawSensitivity;

        ClampPitch();
        ApplyForce();

        //since we don't want to make our player sick, make sure we never roll the camera
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

}