using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRayCaster : MonoBehaviour {

    public Camera myCam;
    RaycastHit hit;
    bool lookingAtSomething = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 fwd = myCam.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(myCam.transform.position, fwd * 50, Color.blue);
        if(Physics.Raycast(myCam.transform.position, fwd, out hit))
        {
            lookingAtSomething = true;
        }
        else
        {
            lookingAtSomething = false;
        }
	}

    bool areWeLookingAtSomething()
    {
        return lookingAtSomething;
    }
    RaycastHit getLookingAt()
    {
        return hit;
    }
}
