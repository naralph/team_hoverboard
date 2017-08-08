using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInsideBoundary : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Boundary")
        {
            EventManager.OnTriggerTransition(0);
        }
    }

}
