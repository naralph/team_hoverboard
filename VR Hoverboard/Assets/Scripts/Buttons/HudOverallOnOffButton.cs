using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudOverallOnOffButton : SelectedObject
{
    TextElementControllerScript textElementController;
    bool safeCheck = false;

    TextMeshPro onOffText;
    bool isOn = true;
    void Start () {
        textElementController = GameObject.Find("TextElementController").GetComponent<TextElementControllerScript>();
        if(textElementController != null)
        {
            safeCheck = true;
        }
        onOffText = gameObject.GetComponentsInChildren<TextMeshPro>()[0];
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
            if (textElementController.allToggledOff)
            {
                textElementController.toggleAllOn();
                textElementController.allToggledOff = false;
                onOffText.SetText("On");
            }
            else
            {
                textElementController.toggleAllOff();
                textElementController.allToggledOff = true;
                onOffText.SetText("Off");
            }
        }
        else
        {
            Debug.Log("The button couldn't find the players text element controller to toggle");
        }
    }
}
