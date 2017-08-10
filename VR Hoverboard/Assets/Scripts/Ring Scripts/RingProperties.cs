using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RingProperties : MonoBehaviour
{
    //assign through the inspector
    public bool duplicatePosition = false;
    public int positionInOrder = 0;
    public float bonusTime = 0.0f;
    public bool lastRingInScene = false;
    public int nextScene = 0;

    //if we have children, set their values, but only if this is marked as a duplicate
    private void Awake()
    {
        if (duplicatePosition)
        {
            RingProperties[] rps = GetComponentsInChildren<RingProperties>();

            foreach (RingProperties rp in rps)
            {
                rp.duplicatePosition = duplicatePosition;
                rp.positionInOrder = positionInOrder;
                rp.bonusTime = bonusTime;
                rp.lastRingInScene = lastRingInScene;
                rp.nextScene = nextScene;
            }
        }
    }
}
