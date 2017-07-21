using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudArrowOnOffScript : SelectedObject
{
    TextElementControllerScript textElementController;
    bool safeCheck = false;

    TextMeshPro onOffText;
    bool isOn = true;
    void Start()
    {
        textElementController = GameObject.Find("TextElementController").GetComponent<TextElementControllerScript>();
        if (textElementController != null)
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
            isOn = !isOn;
            textElementController.setArrow(isOn);
            if (isOn)
            {
                onOffText.SetText("On");
            }
            else
            {
                onOffText.SetText("Off");
            }
        }
        else
        {
            Debug.Log("The button couldn't find the players text element to toggle");
        }
    }
}
