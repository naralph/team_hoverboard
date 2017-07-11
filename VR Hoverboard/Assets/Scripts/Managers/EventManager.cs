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
    //delegate for selectionSuccess
    public delegate void SelectionSuccess();
    //holds the funtions to call on a selection success
    //use to store functions to call
    public static event SelectionSuccess selectionEvents;

    //use this to call the functions stored in the event
    static public void selectionCall()
    {
        //if the event is subscribed to
        if (selectionEvents != null)
            selectionEvents();
    }
}
