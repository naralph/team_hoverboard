using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowHandler : MonoBehaviour
{
    ArrowAimAt arrowScript;
    ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = GameManager.instance.scoreScript;
    }

    void getArrowScipt(bool isOn)
    {
        if (isOn)
        {
            arrowScript = GetComponentInChildren<ArrowAimAt>();
        }
    }

    void IncreaseScore()
    {
        scoreManager.score += (int)scoreManager.baseScorePerRing;
        //print("Score is now: " + scoreManager.score);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ring")
        {
            RingProperties theRing = other.gameObject.GetComponent<RingProperties>();

            //if we have the arrowScript, and we are going through the correct ring
            if (arrowScript != null && arrowScript.sortedRings[arrowScript.currentlyLookingAt].positionInOrder == theRing.positionInOrder)
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

                        if (originalPosition != comparePosition)
                        {
                            //once we find a different ring, set it and break from the loop
                            arrowScript.currentlyLookingAt = originalLookingAt + offset;
                            break;
                        }
                    }
                }
                //if it isn't a duplicate ring, and it isn't the last ring in the scene
                else if (!theRing.lastRingInScene)
                {
                    arrowScript.currentlyLookingAt++;
                    IncreaseScore();
                }
            }
            if (theRing.lastRingInScene)
            {
                arrowScript.currentlyLookingAt = -1;
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
