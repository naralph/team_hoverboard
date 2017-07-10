using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Phidgets;
using Phidgets.Events;

public class InterfaceData : MonoBehaviour
{
	private static InterfaceData _instance;
	private InterfaceKit interfaceKit;

	// getter to reference the actual InterfaceKit instance
	public InterfaceKit device
	{
		get { return interfaceKit; }
	}

	// Singleton
	public static InterfaceData instance
	{
		get
		{
			if (_instance == null)
			{
				// create the singleton instance          
				Debug.Log("InterfaceData Singleton Instantiated");
				GameObject go = new GameObject("InterfaceData");
				DontDestroyOnLoad(go);
				_instance = go.AddComponent<InterfaceData>();
				_instance.Init();
			}

			return _instance;
		}
	}

	public InterfaceData Wake()
	{
		// "wake" the singleton if needed
		return _instance;
	}

	void Init()
	{
		//interfaceKitInput = _instance.gameObject.AddComponent<InterfaceKitInput>();
		try
		{
			interfaceKit = new InterfaceKit();
			Open();
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	public void Open()
	{
		try
		{
			interfaceKit.open();
			interfaceKit.Attach += new AttachEventHandler(OnAttach);
			Debug.Log("Interface Kit Attempt Open");
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	public void Close()
	{
		if (interfaceKit.Attached)
		{
			//When the application is being terminated, close the Phidget
			interfaceKit.Attach -= new AttachEventHandler(OnAttach);
			Debug.Log("Interface Kit Close");
			interfaceKit.close();
		}
	}

	void OnAttach(object sender, AttachEventArgs e)
	{
		InterfaceKit attached = (InterfaceKit)sender;
		//attached.DataRate = 100;       
		Debug.Log("Interface Kit Attached");
	}

}
