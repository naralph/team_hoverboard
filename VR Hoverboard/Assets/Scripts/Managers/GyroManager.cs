using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    public bool controllerEnabled = false;
    
    public ManagerClasses.PlayerMovementVariables controllerMovementVariables = new ManagerClasses.PlayerMovementVariables();
    public ManagerClasses.PlayerMovementVariables gyroMovementVariables = new ManagerClasses.PlayerMovementVariables();

    public void SetupGyroManager(GameObject p) //OnAwake
    {
        //let our movement script know we are using debug controls
        if (controllerEnabled)
        p.GetComponent<Movement>().SetupMovementScript(controllerEnabled, controllerMovementVariables);
        else
            p.GetComponent<Movement>().SetupMovementScript(controllerEnabled, gyroMovementVariables);
    }
}
