using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowHandler : MonoBehaviour
{
    ArrowAimAt arrowScript;
    ScoreManager scoreManager;
    RingProperties theRing;

    int prevPositionInOrder;

    private void Start()
    {
        scoreManager = GameManager.instance.scoreScript;
        prevPositionInOrder = 0;
    }

    void getArrowScipt(bool isOn)
    {
        if (isOn)
            arrowScript = GetComponentInChildren<ArrowAimAt>();
    }

    void HandleDuplicateRing(int ringArrLength)
    {
        //find the next ring without the same position
        int originalPosition = theRing.positionInOrder;
        int originalLookingAt = arrowScript.currentlyLookingAt;
        int comparePosition = 0;

        //set currentlyLookingAt to -1 in case we don't find a ring after the duplicates
        arrowScript.currentlyLookingAt = -1;
        for (int offset = 1; arrowScript.currentlyLookingAt + offset < ringArrLength; ++offset)
        {
            //store our comparePosition using our offset
            comparePosition = arrowScript.sortedRings[originalLookingAt + offset].positionInOrder;

            if (originalPosition < comparePosition)
            {
                //once we find a different ring, set it and break from the loop
                arrowScript.currentlyLookingAt = originalLookingAt + offset;
                break;
            }
        }
    }

    void HandleRegularRing(int ringArrLength)
    {
        RingProperties rp;

        for (int i = 0; i < ringArrLength; i++)
        {
            if (arrowScript.currentlyLookingAt != -1)
            {
                rp = arrowScript.sortedRings[arrowScript.currentlyLookingAt];

                //so long as we aren't looking at the ring after the one we just went through, and we haven't reached the lastRingInScene
                if (!rp.lastRingInScene && rp.positionInOrder <= theRing.positionInOrder)
                    //keep advancing our looking at target
                    ++arrowScript.currentlyLookingAt;

                else
                    //exit the loop once we've found what we're looking for
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ring")
        {
            theRing = other.gameObject.GetComponent<RingProperties>();

            //if we have the arrowScript, and we are going through the correct ring
            if (arrowScript != null && prevPositionInOrder < theRing.positionInOrder)
            {
                int ringArrLength = arrowScript.sortedRings.GetLength(0);

                if (theRing.duplicatePosition)
                    HandleDuplicateRing(ringArrLength);
                else
                    HandleRegularRing(ringArrLength);

                //update our prevPositionInOrder
                prevPositionInOrder = theRing.positionInOrder;
            }

            //always check to see if it was the last ring in the scene
            if (theRing.lastRingInScene)
            {
                arrowScript.currentlyLookingAt = -1;
                prevPositionInOrder = 0;
            }
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
}
