using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    public bool debugControls = false;
    SpatialData gyro;

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
        if (!debugControls)
        {
            gyro.device.DataRate = 20;
        }
    }
    void Update()
    {

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
