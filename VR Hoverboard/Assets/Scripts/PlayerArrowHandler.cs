using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowHandler : MonoBehaviour
{
    ArrowAimAt arrowScript;


    void getArrowScipt(bool isOn)
    {
        if (isOn)
        {
            arrowScript = GetComponentInChildren<ArrowAimAt>();
        }
    }

    private void OnEnable()
    {
        EventManager.OnToggleArrow += getArrowScipt;
    }
    private void OnDisable()
    {
        EventManager.OnToggleArrow -= getArrowScipt;
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
