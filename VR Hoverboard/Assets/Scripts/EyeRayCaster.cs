using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRayCaster : MonoBehaviour {

    public Camera myCam;
    public GameObject cameraObj;
    //how far ahead the ray checks for a collision
    public float rayLength = 5.0f;
    reticle reticleScript;
    RaycastHit hit;
    bool lookingAtSomething = false;

    // Use this for initialization
    void Start ()
    {
        reticleScript = cameraObj.GetComponent<reticle>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 fwd = myCam.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(myCam.transform.position, fwd * rayLength, Color.blue);

        //Ray in front of the camera
        Ray ray = new Ray(myCam.transform.position, myCam.transform.forward);

        if(Physics.Raycast(ray, out hit, rayLength))
        {
            lookingAtSomething = true;
            if (hit.collider.tag != "Player")
            {
                reticleScript.setPosition(hit, lookingAtSomething);
            }
        }
        else
        {
            lookingAtSomething = false;
            reticleScript.setPosition(hit, lookingAtSomething);
        }
	}
}
