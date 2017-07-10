using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class does not not need to be instantiated
public class EventManager : MonoBehaviour
{
    //delegates can be overwritten (less secure)
    public delegate void ActualControlsDisabled();
    //events can only be subscribed to or unsubscribed to (more secure)
    public static event ActualControlsDisabled OnDisableActualControls;

    static public void OnNotUsingActualControls()
    {
        //if the event is subscribed to
        if (OnDisableActualControls != null)
            OnDisableActualControls();
    }

    //Event for selection success
    public delegate void SelectionSuccess();
    public static event SelectionSuccess onSelectionSuccess;

    static public void onNotSelectionSuccess()
    {
        //if the event is subscribed to
        if (onSelectionSuccess != null)
            onSelectionSuccess();
    }
}
