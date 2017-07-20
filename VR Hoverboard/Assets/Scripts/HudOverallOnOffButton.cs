﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudOverallOnOffButton : SelectedObject
{

    GameObject textElementController;
    bool safeCheck = false;
	void Start () {
        textElementController = GameObject.Find("TextElementController");
        if(textElementController != null)
        {
            safeCheck = true;
        }
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
        if (safeCheck)
        {
            textElementController.SetActive(!textElementController.activeSelf);
        }
        else
        {
            Debug.Log("The button couldn't find the players text element controller to turn off");
        }
    }
}