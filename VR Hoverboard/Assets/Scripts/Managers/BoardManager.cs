using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoardType { Beginner, Standard, Expert, Custom }
public class BoardManager : MonoBehaviour
{
    public bool controllerEnabled = false;
    public BoardType defaultBoardSelection = BoardType.Custom;

    [Space]
    public ManagerClasses.PlayerMovementVariables customControllerMovementVariables = new ManagerClasses.PlayerMovementVariables();
    public ManagerClasses.PlayerMovementVariables customGyroMovementVariables = new ManagerClasses.PlayerMovementVariables();

    public void SetupBoardManager(GameObject p)
    {
        //assign our default board selectin to the player
        p.GetComponent<PlayerGameplayController>().SetupMovementScript(controllerEnabled, BoardSelect(defaultBoardSelection));
    }

    public ManagerClasses.PlayerMovementVariables BoardSelect(BoardType bSelect)
    {
        ManagerClasses.PlayerMovementVariables returnMe = new ManagerClasses.PlayerMovementVariables();

        //return the proper variables, depending on if we are using a controller or gyro
        if (controllerEnabled)
            ControllerBoardSelect(bSelect, out returnMe);
        else
            GyroBoardSelect(bSelect, out returnMe);

        return returnMe;
    }

    void ControllerBoardSelect(BoardType bSelect, out ManagerClasses.PlayerMovementVariables pmv)
    {
        switch (bSelect)
        {
            case BoardType.Beginner:
                pmv = new ManagerClasses.PlayerMovementVariables
                    (
                    15f, 10f, 8f,
                    3.45f, 3.45f,
                    30f, 10f, 30f,
                    1f, 1f, 1f, 5f
                    );
                break;
            case BoardType.Standard:
                pmv = new ManagerClasses.PlayerMovementVariables
                    (
                    20f, 15f, 10f,
                    3f, 3f,
                    30f, 10f, 30f,
                    1f, 1f, 1f, 5f
                    );
                break;
            case BoardType.Expert:
                pmv = new ManagerClasses.PlayerMovementVariables
                    (
                    25f, 20f, 18f,
                    2.5f, 2.5f,
                    45f, 5f, 45f,
                    1f, 1f, 1f, 5f
                    );
                break;
            default:
                pmv = customControllerMovementVariables;
                break;
        }
    }

    void GyroBoardSelect(BoardType bSelect, out ManagerClasses.PlayerMovementVariables pmv)
    {
        switch (bSelect)
        {
            case BoardType.Beginner:

            case BoardType.Standard:

            case BoardType.Expert:

            default:
                pmv = customGyroMovementVariables;
                break;
        }
    }
}
