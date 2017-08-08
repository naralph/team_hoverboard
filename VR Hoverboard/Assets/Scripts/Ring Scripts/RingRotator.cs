using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//parent this gameobject to any rings that you want to rotate
//don't forget to set the individual ring properties and positions
public class RingRotator : MonoBehaviour
{
    Transform anchor;
    public float rotateRate = 5f;

    //values for setting our children to
    public bool duplicatePosition = true;
    public int positionInOrder = 0;
    public float timeToReach = 0f;
    public bool lastRingInScene = false;
    public int nextScene = 0;

	// Use this for initialization
	void Start ()
    {
        anchor = GetComponent<Transform>();
        GetComponent<MeshRenderer>().enabled = false;

        //only set the individual values of each child ring if they are duplicate rings
        if (duplicatePosition == true)
        {
            RingProperties[] rps = GetComponentsInChildren<RingProperties>();

            foreach (RingProperties rp in rps)
            {
                rp.duplicatePosition = duplicatePosition;
                rp.positionInOrder = positionInOrder;
                rp.timeToReach = timeToReach;
                rp.lastRingInScene = lastRingInScene;
                rp.nextScene = nextScene;
            }
        }       
	}
	
	// Update is called once per frame
	void Update ()
    {
        anchor.Rotate(Vector3.forward, Time.deltaTime * rotateRate);
	}
}
