using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSetupScript : MonoBehaviour
{
    Transform[] ringTransforms;

    GameObject arrow;
    ArrowAimAt arrowScript;

    void Start()
    {
        arrow = GameObject.Find("Arrow");

        if (arrow != null)
        {
            arrowScript = arrow.GetComponent<ArrowAimAt>();

            RingProperties[] rings;
            rings = GetComponentsInChildren<RingProperties>();

            //insertion sort the rings according to their position in order
            int arrayLength = rings.GetLength(0);
            InsertionSort(rings, arrayLength);
            
            //assign the transforms from the sorted array
            ringTransforms = new Transform[rings.GetLength(0)];

            for (int i = 0; i < arrayLength; i++)
                ringTransforms[i] = rings[i].transform;

            arrowScript.thingsToLookAt = ringTransforms;
            arrowScript.sortedRings = rings;
            arrowScript.currentlyLookingAt = 0;
        }
    }

    void InsertionSort(RingProperties[] rings, int arrayLength)
    {
        int currRing = 1;
        while (currRing < arrayLength)
        {
            RingProperties storedRing = rings[currRing];

            int compareRing = currRing - 1;
            while (compareRing >= 0 && rings[compareRing].positionInOrder > storedRing.positionInOrder)
            {
                rings[compareRing + 1] = rings[compareRing];
                --compareRing;
            }

            rings[compareRing + 1] = storedRing;
            ++currRing;
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
