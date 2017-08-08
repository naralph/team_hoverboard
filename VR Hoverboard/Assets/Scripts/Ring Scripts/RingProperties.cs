using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingProperties : MonoBehaviour
{
    //assign through the inspector
    public bool duplicatePosition = false;
    public int positionInOrder = 0;
    public float timeToReach = 0.0f;
    public bool lastRingInScene = false;
    public int nextScene = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (lastRingInScene)
            {
                EventManager.OnTriggerTransition(nextScene);
            }
        }
    }
}
