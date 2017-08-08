using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowHandler : MonoBehaviour
{
    ArrowAimAt arrowScript;
    ScoreManager scoreManager;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ring")
        {
            RingProperties theRing = other.gameObject.GetComponent<RingProperties>();

            //if we have the arrowScript, and we are going through the correct ring
            if (arrowScript != null && prevPositionInOrder < theRing.positionInOrder)
            {
                int ringArrLength = arrowScript.sortedRings.GetLength(0);

                //if there is more than one ring at this position in the ring order
                if (theRing.duplicatePosition)
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
                //if it isn't a duplicate ring, and it isn't the last ring in the scene
                RingProperties rp;
                while (true)
                {
                    if (arrowScript.currentlyLookingAt != -1)
                    {
                        rp = arrowScript.sortedRings[arrowScript.currentlyLookingAt];

                        if (!rp.lastRingInScene)
                        {
                            if (rp.positionInOrder <= theRing.positionInOrder)
                                ++arrowScript.currentlyLookingAt;
                            else
                                break;
                        }
                        else
                            break;
                    }
                    else
                        break;
                }

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
