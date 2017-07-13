using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhidgetInterfaceExample : MonoBehaviour
{
    //for gyro
   // SpatialData sd;

    public new Transform transform;
    // Use this for initialization
    void Awake()
    {
       // InterfaceData.instance.Wake();
       // sd = new SpatialData();
    }

    private void Start()
    {
        //sd.device.DataRate = 8;      
    }

    // Update is called once per frame
    void Update()
    {
        //has all the properties
        //InterfaceData.instance.device.inputs //arary of 8 digital inputs

        //InterfaceData.instance.device.sensors[0]. //raw values.. sensativites
        //InterfaceData.instance.device.outputs //arary of 8 digital inputs

        //for fans, deal with digital output 0 or 7
        //InterfaceData.instance.device.outputs[0]

        //SPATIAL DATA
       // print(sd.pitchAngle * Mathf.Rad2Deg);
       // print("DATA RATE : " + sd.device.DataRate);
        //Vector3 vec = new Vector3((float)sd.pitchAngle * Mathf.Rad2Deg, 0.0f, (float)sd.rollAngle * Mathf.Rad2Deg);
        //transform.rotation = Quaternion.Euler(vec);
    }

    //private void OnDestroy()
    //{
    //    sd.Close();
    //}

    private void OnApplicationQuit()
    {
        //sd.Close();
    }
}
