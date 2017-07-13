using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    public bool debugControls = false;

    public ManagerClasses.GyroMovementVariables gyroMovementVariables = new ManagerClasses.GyroMovementVariables();

    public void SetupGyroManager(GameObject p)
    {
        //let our movement script know we are using debug controls
        p.GetComponent<Movement>().SetupMovementScript(debugControls, gyroMovementVariables);
    }
}
