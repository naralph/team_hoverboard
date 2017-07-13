using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class does not not need to be instantiated
public class EventManager : MonoBehaviour
{
    //delegates can be overwritten (less secure)
    public delegate void ToggleMovementLock(bool locked);
    //events can only be subscribed to or unsubscribed to (more secure)
    public static event ToggleMovementLock OnToggleMovement;

    static public void OnSetMovementLock(bool locked)
    {
        //if the event is subscribed to
        if (OnToggleMovement != null)
            OnToggleMovement(locked);
    }

    public delegate void SwitchScene(int scInd);
    public static event SwitchScene OnChangeScenes;

    static public void OnTriggerSceneChange(int sceneIndex)
    {
        if (OnChangeScenes != null)
            OnChangeScenes(sceneIndex);
    }

    public delegate void Transition(int sceneIndex);
    public static event Transition OnTransition;

    static public void OnTriggerTransition(int sceneIndex)
    {
        if (OnTransition != null)
            OnTransition(sceneIndex);
    }

    public delegate void fade();
    public static event fade OnFade;

    static public void OnTriggerFade()
    {
        if (OnFade != null)
            OnFade();
    }

    public delegate void ToggleSelectionLock(bool locked);
    public static event ToggleSelectionLock OnSelectionLock;
    static public void OnTriggerSelectionLock(bool locked)
    {
        if (OnSelectionLock != null)
            OnSelectionLock(locked);
    }
}
