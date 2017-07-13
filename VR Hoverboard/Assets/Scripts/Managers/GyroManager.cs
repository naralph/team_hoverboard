using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    public bool debugControls = false;

    public ManagerUtilities.DebugMovementVariables debugMovementVariables = new ManagerUtilities.DebugMovementVariables();
    public ManagerUtilities.GyroMovementVariables gyroMovementVariables = new ManagerUtilities.GyroMovementVariables();

    public void SetupGyroManager(GameObject p)
    {
        //let our movement script know we are using debug controls
        p.GetComponent<Movement>().SetupMovementScript(debugControls, debugMovementVariables, gyroMovementVariables);
    }
}
