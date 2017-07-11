using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemTest : SelectedObject
{
    //runs while object is selected
    override public void selectedFuntion()
    {

    }

    //runs when object is selected
    override public void deSelectedFunction()
    {

    }

    //runs when timer succeeds
    override public void selectSuccessFunction()
    {
        gameObject.transform.localScale *= 0.5f;
    }
}
