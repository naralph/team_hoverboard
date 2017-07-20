using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowHandler : MonoBehaviour
{
    ArrowAimAt arrowScript;


    void Start()
    {
        arrowScript = GetComponentInChildren<ArrowAimAt>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ring")
        {
            RingProperties theRing = other.gameObject.GetComponent<RingProperties>();
            if (arrowScript != null)
            {
                if (arrowScript.currentlyLookingAt == theRing.positionInOrder - 1)
                {
                    if (!theRing.lastRingInScene)
                    {
                        arrowScript.currentlyLookingAt++;
                    }
                    else
                    {
                        arrowScript.currentlyLookingAt = -1;
                    }
                }
            }
        }
    }
}
