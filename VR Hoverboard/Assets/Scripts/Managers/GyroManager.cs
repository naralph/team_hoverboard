using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    public bool debugControls = false;

    public void SetupGyroManager(GameObject p)
    {
        //let our movement script know we are using debug controls
        p.GetComponent<Movement>().SetupMovement(debugControls);
    }
}
