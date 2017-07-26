using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudRingCountOnOffScript : SelectedObject
{
    bool safeCheck = false;
    Light temp;

    TextMeshPro onOffText;
    bool isOn = true;
    void Start()
    {
        isOnUpdate();
    }
    public void isOnUpdate()
    {
        if (isOn)
        {
            onOffText.SetText("On");
        }
        else
        {
            onOffText.SetText("Off");
        }
    }

    private void OnEnable()
    {
        onOffText = gameObject.GetComponentsInChildren<TextMeshPro>()[0];
        EventManager.OnUpdateButtons += isOnUpdate;
    }

    private void OnDisable()
    {
        EventManager.OnUpdateButtons -= isOnUpdate;
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
