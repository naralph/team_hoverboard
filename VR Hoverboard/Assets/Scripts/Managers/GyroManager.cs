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
            //interface data is for the 8/8/8 device
            //InterfaceData.instance.Wake();
            gyro = new SpatialData();
        }
    }

    public void SetupGyroManager(GameObject p)
    {
        //let our movement script know we are using debug controls
        if (debugControls)
            EventManager.OnNotUsingActualControls();
        else
        {
            gyro.device.DataRate = 8;
            //assign our gyro to the player movement script
            p.GetComponent<Movement>().AssignManager(this);
        }     
    }

    public IEnumerator ShutdownCoroutine()
    {
        gyro.Close();

        yield return new WaitForSeconds(1);
    }

    private void OnApplicationQuit()
    {
        if (!debugControls)
            StartCoroutine(ShutdownCoroutine());
    }

    public SpatialData getGyro()
    {
        return gyro;
    }
}
