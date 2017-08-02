using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public class PlayerMenuController : MonoBehaviour
{
    [Range(2f, 60f)] public float hoverForce = 15f;
    [Range(0.1f, 10.0f)] public float hoverHeight = 2f;    

    [Header("Controller Specific Variables")]
    [Range(5f, 60f)] public float speed = 20f;
    [Range(5f, 60f)] public float turnSpeed = 30f;
    [Range(0.1f, 5f)] public float cameraSpeed = 1.75f;

    [Header("Gyro Specific Variables")]
    [Range(0.0f, 30.0f)] public float gyroDeadZoneDegree = 7.5f;
    [Range(0.25f, 1.0f)] public float gyroSensativity = 0.65f;
    float pitch;
    float yaw;

    float inverseHoverHeight;
    bool coroutinesStopped;
    bool gamepadEnabled;
    bool inAMenu;

    Rigidbody playerRB;
    Transform playerCameraTransform;
    SpatialData gyro;

    //called by our BoardManager
    public void SetupMenuControllerScript()
    {
        playerRB = GameManager.player.GetComponent<Rigidbody>();
        playerCameraTransform = GameManager.player.GetComponentInChildren<Camera>().transform;
        gyro = GameManager.instance.boardScript.gyro;
        gamepadEnabled = GameManager.instance.boardScript.gamepadEnabled;

        inverseHoverHeight = hoverHeight / 1f;
        coroutinesStopped = false;
    }

    //start our movement coroutines depending on if we are in a menu scene
    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        //if we're in options or main menu
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            coroutinesStopped = false;
            inAMenu = true;
            playerRB.useGravity = true;

            //make sure not to have multiple coroutines going
            StopAllCoroutines();

            if (gamepadEnabled == true)
                StartCoroutine(ControllerCoroutine());
            else
                StartCoroutine(GyroCoroutine());
        }
        else if (!coroutinesStopped)
        {
            //reset our camera position to the player's rotation, if we were using debug camera rotation
            if (!VRDevice.isPresent)
                playerCameraTransform.eulerAngles = playerRB.transform.eulerAngles;

            coroutinesStopped = true;
            inAMenu = false;
            playerRB.useGravity = false;

            StopAllCoroutines();
        }
    }

    //update our script depending on if we are using a xbox gamepad or the gyro
    //  Note: this should normally not be directly called, instead call the BoardManager's UpdateControlsType()
    public void UpdateMenuControlsType(bool gEnabled)
    {
        gamepadEnabled = gEnabled;

        StopAllCoroutines();

        if (gamepadEnabled && inAMenu)
            StartCoroutine(ControllerCoroutine());
        else if (inAMenu)
            StartCoroutine(GyroCoroutine());
    }

    //make sure we don't start rotating up/down or start to roll
    void ClampRotation()
    {
        if (playerRB.rotation.eulerAngles.z != 0f || playerRB.rotation.eulerAngles.x != 0f)
            playerRB.rotation = Quaternion.Euler(new Vector3(0f, playerRB.rotation.eulerAngles.y, 0f));
    }

    //let our right thumbstick control the camera if there is no HMD present
    void DebugCameraRotation()
    {       
        if (!VRDevice.isPresent)
        {
            float cameraPitch = playerCameraTransform.eulerAngles.x + -Input.GetAxis("RVertical") * cameraSpeed;
            float cameraYaw = playerCameraTransform.eulerAngles.y + Input.GetAxis("RHorizontal") * cameraSpeed;

            playerCameraTransform.rotation = (Quaternion.Euler(new Vector3(cameraPitch, cameraYaw, 0f)));
        }
    }

    void ApplyHoverForce()
    {
        Ray ray = new Ray(playerRB.position, -playerRB.transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) * inverseHoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            playerRB.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }
    }

    IEnumerator ControllerCoroutine()
    {
        yield return new WaitForFixedUpdate();

        ClampRotation();
        DebugCameraRotation();
        ApplyHoverForce();       

        playerRB.AddRelativeForce(0f, 0f, Input.GetAxis("LVertical") * speed);
        playerRB.AddRelativeTorque(0f, Input.GetAxis("LHorizontal") * turnSpeed, 0f);

        StartCoroutine(ControllerCoroutine());
    }

    //helper function
    void GyroApplyDeadZone()
    {
        //leaning forward
        if (pitch > 0f)
        {
            if (pitch < gyroDeadZoneDegree)
                pitch = 0f;
        }
        else
        {
            if (pitch > -gyroDeadZoneDegree)
                pitch = 0f;
        }

        //leaning left
        if (yaw > 0f)
        {
            if (yaw < gyroDeadZoneDegree)
                yaw = 0f;
        }
        else
        {
            if (yaw > -gyroDeadZoneDegree)
                yaw = 0f;
        }
    }

    IEnumerator GyroCoroutine()
    {
        yield return new WaitForFixedUpdate();

        ClampRotation();
        DebugCameraRotation();
        ApplyHoverForce();

        pitch = (float)gyro.rollAngle * Mathf.Rad2Deg * gyroSensativity;
        yaw = (float)gyro.pitchAngle * Mathf.Rad2Deg * gyroSensativity * -1f;

        GyroApplyDeadZone();

        playerRB.AddRelativeForce(0f, 0f, pitch);
        playerRB.AddRelativeTorque(0f, yaw, 0f);

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
