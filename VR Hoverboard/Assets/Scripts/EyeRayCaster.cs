using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRayCaster : MonoBehaviour {

    public Camera myCam;
    public GameObject reticleObj;
    reticle reticleScript;
    RaycastHit hit;
    bool lookingAtSomething = false;

    // Use this for initialization
    void Start ()
    {
        reticleScript = reticleObj.GetComponent<reticle>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 fwd = myCam.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(myCam.transform.position, fwd * 50, Color.blue);
        if(Physics.Raycast(myCam.transform.position, fwd, out hit))
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
