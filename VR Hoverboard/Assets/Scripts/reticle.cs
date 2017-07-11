using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reticle : MonoBehaviour
{
    //image object for the selection radial(required to function for radial bar)
    [SerializeField] Image selectionRadial;
    //default distance away from the camera the reticle sits at
    public float defaultDistance;
    //the actual reticle
    [SerializeField] public Transform theReticle;
    //whether or not we use a normal of the object we are hitting to rotate the reticle to match against it;
    [SerializeField] bool useNormal = true;
    //the camera transform
    new Transform camera;

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

            theReticle.localScale = originalScale * defaultDistance * scaleMultiplier;

            theReticle.localRotation = originalRotation;
        }
    }

	// Use this for initialization
	void Start () {
        camera = gameObject.GetComponent<Camera>().transform;
        originalScale = theReticle.localScale;
        originalRotation = theReticle.localRotation;
        camera = gameObject.transform;
	}

    //update the reticle based on time
    public void updateReticle(float ratioOfTimePassed)
    {
        selectionRadial.fillAmount = ratioOfTimePassed;
    }

    public void resetReticle()
    {
        selectionRadial.fillAmount = 0;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
