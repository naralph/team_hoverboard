using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAimAt : MonoBehaviour
{
    public Transform[] thingsToLookAt;

    private int currentlyLookingAt = 0;

    void Update()
    {
        if (thingsToLookAt[currentlyLookingAt] != null)
        {
            gameObject.transform.LookAt(thingsToLookAt[currentlyLookingAt].transform);
        }
        else
        {
            Debug.Log("The point arrow was supposed to look at doesnt exist");
        }
    }

}
