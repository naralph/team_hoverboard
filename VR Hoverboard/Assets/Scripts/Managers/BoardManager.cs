using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoardType { Beginner, Standard, Expert, Custom }

public class BoardManager : MonoBehaviour
{
    [HideInInspector()] public SpatialData gyro;
    PlayerGameplayController pgc;
    PlayerMenuController pmc;

    public bool gamepadEnabled = false;

    //store our current board selection
    public BoardType currentBoardSelection = BoardType.Custom;

    [Space]
    public ManagerClasses.PlayerMovementVariables customGamepadMovementVariables = new ManagerClasses.PlayerMovementVariables();
    public ManagerClasses.PlayerMovementVariables customGyroMovementVariables = new ManagerClasses.PlayerMovementVariables();

    //use this instead of Awake() so that we can control the execution order through the GameManager
    public void SetupBoardManager(GameObject p)
    {
        if (!gamepadEnabled)
            gyro = new SpatialData();

        pgc = p.GetComponent<PlayerGameplayController>();
        pmc = p.GetComponent<PlayerMenuController>();

        //setup our controller scripts
        pgc.SetupGameplayControllerScript();
        pmc.SetupMenuControllerScript();
    }

    //updates our player controller scripts depending on what type of controls we are using
    //  if gEnabled == true, then our controllers will assume that we are using a xbox controller
    //  if false, then the gyro controls will be used
    public void UpdateControlsType(bool gEnabled)
    {
        gamepadEnabled = gEnabled;

        if (!gamepadEnabled && gyro == null)
            gyro = new SpatialData();

        else if (gamepadEnabled && gyro != null)
        {
            gyro.Close();
            gyro = null;
        }

        pgc.UpdateGameplayControlsType(gEnabled, gyro);
        pmc.UpdateMenuControlsType(gEnabled, gyro);
    }

    //returns our controller specific movement variables and updates our currentBoardSelection
    public ManagerClasses.PlayerMovementVariables BoardSelect(BoardType bSelect)
    {
        ManagerClasses.PlayerMovementVariables newBoardVariables = new ManagerClasses.PlayerMovementVariables();

        //update our current board selection
        currentBoardSelection = bSelect;

        //return the proper variables, depending on if we are using a gamepad or gyro
        if (gamepadEnabled)
            ControllerBoardSelect(bSelect, out newBoardVariables);
        else
            GyroBoardSelect(bSelect, out newBoardVariables);

        return newBoardVariables;
    }

    //helper function
    void ControllerBoardSelect(BoardType bSelect, out ManagerClasses.PlayerMovementVariables pmv)
    {
        switch (bSelect)
        {
            case BoardType.Beginner:
                pmv = new ManagerClasses.PlayerMovementVariables
                    (
                    17f, 15f, 12f,
                    3.45f, 3.45f,
                    30f, 10f, 30f,
                    1f, 1f, 1f, 5f
                    );
                break;
            case BoardType.Standard:
                pmv = new ManagerClasses.PlayerMovementVariables
                    (
                    20f, 16f, 12f,
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
                pmv = customGamepadMovementVariables;
                break;
        }
    }

    //helper function
    void GyroBoardSelect(BoardType bSelect, out ManagerClasses.PlayerMovementVariables pmv)
    {
        switch (bSelect)
        {
            case BoardType.Beginner:
                pmv = new ManagerClasses.PlayerMovementVariables
                    (
                    17f, 15f, 12f,
                    2.5f, 2.5f,
                    30f, 10f, 30f,
                    1f, 1f, 1f, 5f
                    );
                break;
            case BoardType.Standard:
                pmv = new ManagerClasses.PlayerMovementVariables
                    (
                    20f, 16f, 12f,
                    2f, 2f,
                    30f, 10f, 30f,
                    1f, 1f, 1f, 5f
                    );
                break;
            case BoardType.Expert:
                pmv = new ManagerClasses.PlayerMovementVariables
                    (
                    25f, 20f, 18f,
                    1.5f, 1.5f,
                    45f, 5f, 45f,
                    1f, 1f, 1f, 5f
                    );
                break;
            default:
                pmv = customGyroMovementVariables;
                break;
        }
    }

    private void OnApplicationQuit()
    {
        if (gyro != null)
            gyro.Close();
    }
}