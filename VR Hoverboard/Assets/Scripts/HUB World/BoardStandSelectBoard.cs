using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStandSelectBoard : SelectedObject
{
    PlayerGameplayController playerGameplayController;
    MeshRenderer playerBoardMesh;

    //our material and board types are stored in the BoardStandScript
    BoardStandProperties selectionVariables;

    private void Start()
    {
        playerGameplayController = GameManager.player.GetComponent<PlayerGameplayController>();
        playerBoardMesh = GameManager.player.GetComponentInChildren<MeshRenderer>();

        selectionVariables = GetComponentInParent<BoardStandProperties>();
    }

    //runs while object is selected
    override public void selectedFuntion()
    {

    }

    //runs when object is deselected
    override public void deSelectedFunction()
    {

    }

    //runs when timer succeeds
    override public void selectSuccessFunction()
    {
        //set the player board to one of our pre-defined boards
        playerGameplayController.UpdatePlayerBoard(selectionVariables.boardType);   
        playerBoardMesh.material = selectionVariables.boardMaterial;
    }

}
