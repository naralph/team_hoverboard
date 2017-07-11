using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : SelectedObject
{
    [SerializeField] int sceneIndex;
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
        EventManager.OnTriggerSceneChange(sceneIndex);
    }
}
