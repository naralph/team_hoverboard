using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public class PlayerMenuController : MonoBehaviour
{
    public float speed = 90f;
    public float turnSpeed = 5f;
    public float hoverForce = 65f;
    public float hoverHeight = 3.5f;
    public float cameraSpeed = 2f;

    float inverseHoverHeight;
    float powerInput;
    float turnInput;
    bool coroutinesStopped;

    Rigidbody playerRB;
    Transform playerTransform;
    Transform playerCameraTransform;

    private void Start()
    {
        playerRB = GameManager.player.GetComponent<Rigidbody>();
        playerTransform = GameManager.player.GetComponent<Transform>();
        playerCameraTransform = GameManager.player.GetComponentInChildren<Camera>().transform;

        inverseHoverHeight = hoverHeight / 1f;
        coroutinesStopped = false;
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            coroutinesStopped = false;

            if (GameManager.instance.gyroScript.controllerEnabled == true)
                StartCoroutine(ControllerCoroutine());
            else
                StartCoroutine(GyroCoroutine());
        }
        else if (!coroutinesStopped)
        {
            //reset our camera position to 0, if we were using the right joystick for rotating it
            if (!VRDevice.isPresent)
                playerCameraTransform.eulerAngles = Vector3.zero;

            coroutinesStopped = true;
            StopAllCoroutines();
        }
    }

    IEnumerator ControllerCoroutine()
    {
        yield return new WaitForFixedUpdate();

        //let our right thumbstick control the camera if there is no HMD present
        if (!VRDevice.isPresent)
        {
            float pitch = playerCameraTransform.eulerAngles.x + -Input.GetAxis("RVertical") * cameraSpeed;
            float yaw = playerCameraTransform.eulerAngles.y + Input.GetAxis("RHorizontal") * cameraSpeed;

            playerCameraTransform.rotation = (Quaternion.Euler(new Vector3(pitch, yaw, 0f)));
        }

        Ray ray = new Ray(playerTransform.position, playerTransform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) * inverseHoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            playerRB.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }

        powerInput = Input.GetAxis("LVertical");
        turnInput = Input.GetAxis("LHorizontal");

        playerRB.AddRelativeForce(0f, 0f, powerInput * speed);
        playerRB.AddRelativeTorque(0f, turnInput * turnSpeed, 0f);

        StartCoroutine(ControllerCoroutine());
    }

    IEnumerator GyroCoroutine()
    {
        yield return new WaitForFixedUpdate();


        StartCoroutine(GyroCoroutine());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }
}
