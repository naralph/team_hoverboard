using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{

    SpatialData gyro;

    void Awake()
    {
        InterfaceData.instance.Wake();
        gyro = new SpatialData();
    }

    private void Start()
    {
        gyro.device.DataRate = 4;
    }
    void Update()
    {

    }

    private void OnApplicationQuit()
    {
        gyro.Close();
    }

    public SpatialData getGyro()
    {
        return gyro;
    }
}
