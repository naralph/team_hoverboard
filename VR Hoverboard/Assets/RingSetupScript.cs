using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSetupScript : MonoBehaviour
{

    Transform[] rings;
    
	void Start ()
    {
        int ringCount = gameObject.transform.childCount;
        rings = new Transform[ringCount];
        for (int i = 0; i < ringCount; i++)
        {
            rings[i] = gameObject.GetComponentsInChildren<Transform>()[i];
            Debug.Log("Ring " + i + " Ring postition: " + rings[i].position);
        }
		
	}
}
