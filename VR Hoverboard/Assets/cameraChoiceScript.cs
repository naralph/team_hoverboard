using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class cameraChoiceScript : MonoBehaviour {

    private string headsetUsed;

    public GameObject OpenVRCamera;
    public GameObject OcculusCameraSet;

	// Use this for initialization
	void Start () {
        headsetUsed = VRSettings.loadedDeviceName;
        Debug.Log(headsetUsed);

        if (headsetUsed == "OpenVR")
        {
            OcculusCameraSet.SetActive(false);
        }
        else
        {
            OpenVRCamera.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
