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

    private void Awake()
    {
        RingRotator rr = GetComponentInParent<RingRotator>();

        if (rr != null && rr.duplicatePosition == true)
        {
            duplicatePosition = rr.duplicatePosition;
            positionInOrder = rr.positionInOrder;
            bonusTime = rr.bonusTime;
            lastRingInScene = rr.lastRingInScene;
            nextScene = rr.nextScene;
        }
    }  
}
