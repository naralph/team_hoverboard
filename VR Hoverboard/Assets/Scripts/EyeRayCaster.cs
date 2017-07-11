using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRayCaster : MonoBehaviour
{
    //object for selection purposes;
    GameObject curObj;

    public Camera myCam;
    reticle reticleScript;
    RaycastHit hit;
    bool lookingAtSomething = false;

    [SerializeField] float debugRayDrawLength = 50.0f;
    [SerializeField] float rayCheckLength = 50.0f;

    // Use this for initialization
    void Start ()
    {
        reticleScript = GetComponent<reticle>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 fwd = myCam.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(myCam.transform.position, fwd * debugRayDrawLength, Color.blue);
        if(Physics.Raycast(myCam.transform.position, fwd, out hit, rayCheckLength))
        {
            lookingAtSomething = true;
            if (hit.collider.tag != "Player")
            {
                reticleScript.setPosition(hit, lookingAtSomething);
                if (hit.collider.tag == "Selectable")
                {
                    curObj = hit.collider.gameObject;
                    curObj.GetComponent<SelectedObject>().selected();
                }
                else
                {
                    if (curObj)
                    {
                        curObj.GetComponent<SelectedObject>().deSelected();
                        curObj = null;
                    }
                }
            }
        }
        else
        {
            lookingAtSomething = false;
            reticleScript.setPosition(hit, lookingAtSomething);
        }
	}
}
