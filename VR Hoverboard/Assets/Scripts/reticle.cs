using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reticle : MonoBehaviour
{
    public Transform theReticle;
    Vector3 originalScale;
    Quaternion originalRotation;
    bool useNormal = true;

    public void setPosition(RaycastHit hit)
    {
        theReticle.position = hit.point;
        theReticle.localScale = originalScale * hit.distance;

        if (useNormal)
        {
            theReticle.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        }
        else
        {
            theReticle.localRotation = originalRotation;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
