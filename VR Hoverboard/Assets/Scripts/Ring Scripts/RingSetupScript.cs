using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSetupScript : MonoBehaviour
{
    Transform[] rings;
    GameObject arrow;
    ArrowAimAt arrowScript;
    
	void Start ()
    {
        arrow = GameObject.Find("Arrow");
        if (arrow != null)
        {
            arrowScript = arrow.GetComponent<ArrowAimAt>();
            //int ringCount = gameObject.transform.childCount;
            rings = GetComponentsInChildren<Transform>();
            //rings = new Transform[ringCount];
            //for (int i = 0; i < ringCount; i++)
            //{
            //    //CAN BE OPTIMIZED
            //    Transform child = gameObject.transform.GetChild(i);
            //    rings[child.gameObject.GetComponent<RingProperties>().positionInOrder - 1] = child;
            //    //Debug.Log("Ring: " + i + " is at " + rings[i].transform.position);
            //}
            arrowScript.thingsToLookAt = rings;
            arrowScript.currentlyLookingAt = 0;
        }        
	}

    private void OnDisable()
    {
        if (arrow != null)
        {
            arrowScript.thingsToLookAt = null;
            arrowScript.currentlyLookingAt = -1;
        }
    }
}
