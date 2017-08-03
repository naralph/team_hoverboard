using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControllerSwitchScript : SelectedObject
{
    TextMeshPro controllerOnOffText;

    GameManager theManager;
    bool isOn = true;

    private void Start()
    {
        theManager = GameManager.instance;
        controllerIsOnUpdate();
    }

    public void controllerIsOnUpdate()
    {
        isOn = theManager.boardScript.gamepadEnabled;
        if (isOn)
        {
            controllerOnOffText.SetText("On");
        }
        else
        {
            controllerOnOffText.SetText("Off");
        }
    }

    private void OnEnable()
    {
        controllerOnOffText = gameObject.GetComponentInChildren<TextMeshPro>();
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
        isOn = !isOn;
        theManager.boardScript.UpdateControlsType(isOn);
        if (isOn)
        {
            controllerOnOffText.SetText("On");
        }
        else
        {
            controllerOnOffText.SetText("Off");
        }
    }
}
