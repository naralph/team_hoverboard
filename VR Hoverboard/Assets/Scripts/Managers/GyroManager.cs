using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    public bool debugControls = false;
    SpatialData gyro;

    //TODO:: fix the PhidgetException..... but not this way
    ////when this manager is enabled, it subscribes to our event
    //private void OnEnable()
    //{
    //    EventManager.OnDisableActualControls += DisableGyro;
    //}

    ////when this manager is disabled, it unsubscribes to the event
    ////not doing this can cause memory leaks and other problems
    //private void OnDisable()
    //{
    //    EventManager.OnDisableActualControls -= DisableGyro;
    //}

    ////function for our event
    //void DisableGyro()
    //{
    //    debugControls = true;
    //}

    void Awake()
    {
        if (!debugControls)
        {
            InterfaceData.instance.Wake();
            gyro = new SpatialData();
        }
    }

    private void Start()
    {
        if (debugControls)
        {
            EventManager.OnNotUsingActualControls();
        }
        else
        { 
            gyro.device.DataRate = 20;
        }
    }

    private void OnApplicationQuit()
    {
        if (!debugControls)
        {
            gyro.Close();
        }
    }

    public SpatialData getGyro()
    {
        return gyro;
    }
}
