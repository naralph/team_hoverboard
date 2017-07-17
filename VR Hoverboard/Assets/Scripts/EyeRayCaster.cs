using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRayCaster : MonoBehaviour
{
    //turns on or off the ability to do selecting
    public bool canSelect = true;

    //object for selection purposes;
    GameObject preObj = null;
    GameObject curObj = null;

    public Camera myCam;
    reticle reticleScript;
    RaycastHit hit;

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
        //if ray collides with something
        if (Physics.Raycast(myCam.transform.position, fwd, out hit, rayCheckLength))
        {
            if (hit.collider.tag != "Player")
            {
                reticleScript.setPosition(hit, true);
                if (hit.collider.tag == "Selectable" && canSelect)
                {
                    curObj = hit.collider.gameObject;
                    curObj.GetComponent<SelectedObject>().selected(reticleScript);
                }
            }

            preObj = hit.collider.gameObject;
        }
        //if ray doesnt collide with anything
        else
        {
            reticleScript.setPosition(hit, false);
            preObj = null;
        }
        if (((preObj == null || preObj != curObj) && curObj != null))
        {
            curObj.GetComponent<SelectedObject>().deSelected();
        }
    }

    void SetSelectionLock(bool locked)
    {
        canSelect = !locked;
        if (curObj != null)
        {
            curObj.GetComponent<SelectedObject>().deSelected();
        }
    }

    public void OnEnable()
    {
        EventManager.OnSelectionLock += SetSelectionLock;
    }

    public void OnDisable()
    {
        EventManager.OnSelectionLock -= SetSelectionLock;
    }
}
