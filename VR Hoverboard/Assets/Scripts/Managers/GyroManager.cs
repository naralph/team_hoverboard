using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    public enum BoardType { Beginner, Standard, Expert, Custom }

    public bool controllerEnabled = false;
    public BoardType boardSelection = BoardType.Custom;

    [Space]
    public ManagerClasses.PlayerMovementVariables customControllerMovementVariables = new ManagerClasses.PlayerMovementVariables();
    public ManagerClasses.PlayerMovementVariables customGyroMovementVariables = new ManagerClasses.PlayerMovementVariables();

    ManagerClasses.PlayerMovementVariables selectedMovementVariables;

    public void SetupGyroManager(GameObject p) //OnAwake
    {
        //let our movement script know we are using debug controls
        if (controllerEnabled)
            ControllerBoardSelect();
        else
            GyroBoardSelect();

        p.GetComponent<PlayerController>().SetupMovementScript(controllerEnabled, selectedMovementVariables);
    }

    public void ControllerBoardSelect()
    {
        switch (boardSelection)
        {
            case BoardType.Beginner:
                selectedMovementVariables = new ManagerClasses.PlayerMovementVariables
                    (
                    15f, 10f, 8f,
                    3.45f, 3.45f,
                    30f, 10f, 30f,
                    1f, 1f, 1f, 5f
                    );
                break;
            case BoardType.Standard:
                selectedMovementVariables = new ManagerClasses.PlayerMovementVariables
                    (
                    20f, 15f, 10f,
                    3f, 3f,
                    30f, 10f, 30f,
                    1f, 1f, 1f, 5f
                    );
                break;
            case BoardType.Expert:
                selectedMovementVariables = new ManagerClasses.PlayerMovementVariables
                    (
                    25f, 20f, 18f,
                    2.5f, 2.5f,
                    45f, 5f, 45f,
                    1f, 1f, 1f, 5f
                    );
                break;
            default:
                selectedMovementVariables = customControllerMovementVariables;
                break;
        }
    }

    public void GyroBoardSelect()
    {
        switch (boardSelection)
        {
            case BoardType.Beginner:

            case BoardType.Standard:

            case BoardType.Expert:

            default:
                selectedMovementVariables = customGyroMovementVariables;
                break;
        }
    }
}
