using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    public bool controllerEnabled = false;

    public ManagerClasses.ControllerMovementVariables gamepadMovementVariables = new ManagerClasses.ControllerMovementVariables();
    public ManagerClasses.GyroMovementVariables gyroMovementVariables = new ManagerClasses.GyroMovementVariables();

    public void SetupGyroManager(GameObject p) //OnAwake
    {
        //let our movement script know we are using debug controls
        p.GetComponent<Movement>().SetupMovementScript(controllerEnabled, gyroMovementVariables, gamepadMovementVariables);
    }
}
