using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class CameraCounterRotate : MonoBehaviour
{
    public Transform mainCameraTransform, cameraContainerTransform, boardTransform;

    private void Start()
    {
        if (VRDevice.isPresent) ;
            StartCoroutine(CounterRotate());
    }

    IEnumerator CounterRotate()
    {
        if (cameraContainerTransform.eulerAngles.x != 0f)
            cameraContainerTransform.Rotate(Vector3.right, -cameraContainerTransform.eulerAngles.x);

        print("MAIN CAMERA EULER ANGLES: " + mainCameraTransform.eulerAngles);
        print("BOARD EULER ANGLES: " + boardTransform.eulerAngles);

        yield return null;

        StartCoroutine(CounterRotate());
    }
}
