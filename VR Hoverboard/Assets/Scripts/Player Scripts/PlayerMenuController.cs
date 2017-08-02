using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public class PlayerMenuController : MonoBehaviour
{
    public float speed = 20f;
    public float turnSpeed = 30f;
    public float hoverForce = 15f;
    public float hoverHeight = 2f;
    public float cameraSpeed = 2f;

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

    IEnumerator GyroCoroutine()
    {
        yield return new WaitForFixedUpdate();

        ClampRotation();
        DebugCameraRotation();
        ApplyHoverForce();

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
