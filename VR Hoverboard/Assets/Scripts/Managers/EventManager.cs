using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ActualControlsEnabled();
    public static event ActualControlsEnabled OnEnableControls;

    void OnGUI()
    {
        if (OnEnableControls != null)
            OnEnableControls();
    }	
}
