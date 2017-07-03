using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reticle : MonoBehaviour
{
    //default distance away from the camera the reticle sits at
    public float defaultDistance;
    //the actual reticle
    public Transform theReticle;
    //whether or not we use a normal of the object we are hitting to rotate the reticle to match against it;
    bool useNormal = true;
    //the camera transform
    public Transform camera;

    //Scale value for reticle size(to make sure it isnt to huge in the scene)
    public float scaleMultiplier = 0.01f;

    //need to save the originals of the scale and rotation for the reticle so that we can reset them as need be
    Vector3 originalScale;
    Quaternion originalRotation;

    public void setPosition(RaycastHit hit, bool didHit)
    {
        if (didHit)
        {
            theReticle.position = hit.point;
            theReticle.localScale = originalScale * hit.distance * scaleMultiplier;

            if (useNormal)
            {
                theReticle.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            }
            else
            {
                theReticle.localRotation = originalRotation;
            }
        }
        else
        {
            theReticle.position = camera.position + camera.forward * defaultDistance;

            theReticle.localScale = originalScale;

            theReticle.localRotation = originalRotation;
        }
        //Debug.Log("Collision for reticle returned: " + didHit);
    }

	// Use this for initialization
	void Start () {
        originalScale = theReticle.localScale;
        originalRotation = theReticle.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
