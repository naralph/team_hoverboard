using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Phidgets;
using Phidgets.Events;

using Spatial_full;

public class SpatialData
{
	public Spatial device;

	public SpatialData()
	{
		try
		{
			device = new Spatial();
			Open();
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}
	
	// Spatial (accel,gyro,compass phidget) Members
	public double pitchAngleStart = -1000; // store the starting pitch angle for reference
	public double pitchAngle = 0;
	public double rollAngle = 0;
	double[] lastMsCount = { 0, 0, 0 };
	bool[] lastMsCountGood = { false, false, false };
	double[] gyroHeading = { 0, 0, 0 }; //degrees
	List<double[]> compassBearingFilter = new List<double[]>();
	int compassBearingFilterSize = 10;
	double compassBearing = 0;
	double lastBearing=0;
	
	const double ambientMagneticField = 0.57142; //Calgary
	const double ambientGravity = 1; //in G's
	public float	fixedDeltaTime = 0;
	
	public void Open()
	{
		device.open();
        //device.waitForAttachment(1000);
		MonoBehaviour.print ("Spatial Sensors Open");		
		
		device.Attach += new AttachEventHandler(spatial_Attach);
//      device.Error += new Phidgets.Events.ErrorEventHandler(spatial_Error);
        device.SpatialData += new SpatialDataEventHandler(spatial_SpatialData);	
	}
	
	public void Close()
	{
		if (device != null && device.Attached)
		{
			//When the application is being terminated, close the Phidget
	        device.Attach -= new AttachEventHandler(spatial_Attach);
	        //device.Detach -= new DetachEventHandler(spatial_Detach);
	        device.SpatialData -= new SpatialDataEventHandler(spatial_SpatialData);
	
	        //run any events in the message queue - otherwise close will hang if there are any outstanding events
	        //Application.DoEvents();
			MonoBehaviour.print ("Spatial Sensors Close");
	        device.close();
		}
	}
	
	//spatial attach event handler
    void spatial_Attach(object sender, AttachEventArgs e)
    {
		Spatial attached = (Spatial)sender;

        attached.DataRate = 100;

        lastMsCountGood[0] = false;
        lastMsCountGood[1] = false;
        lastMsCountGood[2] = false;
        gyroHeading[0] = 0;
        gyroHeading[1] = 0;
        gyroHeading[2] = 0;
        
		//Debug.Log ("Spatial Serial Number:" + device.SerialNumber);
		
        //enter board specific magnetic field corrections
        //these are commented out because they will be different for every board - get them from the compass calibration program
        //attached.setCompassCorrectionParameters(0.50756, -0.03738, -0.10788, -0.02335, 1.95880, 1.92179, 2.03008, 0.00722, 0.00979, 0.00708, -0.00225, 0.01012, -0.00237);
		
    }
	
	void spatial_SpatialData(object sender, SpatialDataEventArgs e)
	{
		if(device.gyroAxes.Count > 0)
        {
			calculateGyroHeading(e.spatialData, 0); //x axis
	    	calculateGyroHeading(e.spatialData, 1); //y axis
	    	calculateGyroHeading(e.spatialData, 2); //z axis
			//print ("GyroHeading.X");
			//print ("GyroHeading.X"+ gyroHeading[0].ToString("F3")+"°");
			//print (pitchAngle);
			//print ("rollAngle6789687967896");
			//print (rollAngle);
			//print ("BearingAngle678967896");
			//print (bearing);
		}
		
		//Even when there is a compass chip, sometimes there won't be valid data in the event.
        if (device.compassAxes.Count > 0 && e.spatialData[0].MagneticField.Length > 0)
        {
            try
            {
                calculateCompassBearing();
            }
            catch
            {
            }
        }
	}

	//This integrates gyro angular rate into heading over time
	void calculateGyroHeading(SpatialEventData[] data, int index)
	{
	    double gyro = 0;
	    for (int i = 0; i < data.Length; i++)
	    {
	        gyro = data[i].AngularRate[index];
	
	        if (lastMsCountGood[index])
	        {
	            //calculate heading
	            double timechange = data[i].Timestamp.TotalMilliseconds - lastMsCount[index]; // in ms
	            double timeChangeSeconds = (double)timechange / 1000.0;
				//print (timeChangeSeconds + ":" + fixedDeltaTime);
				
				timeChangeSeconds = fixedDeltaTime; // pulled from Unity's fixed delta time
				
	            gyroHeading[index] += timeChangeSeconds * gyro;
	        }
	
	        lastMsCount[index] = data[i].Timestamp.TotalMilliseconds;
	        lastMsCountGood[index] = true;
	    }
	}
	
	void calculateCompassBearing()
    {
        double Xh = 0.0;
        double Yh = 0.0;

        //find the tilt of the board wrt gravity
        SpVector3 gravity = SpVector3.Normalize(
            new SpVector3(
            device.accelerometerAxes[0].Acceleration,
            device.accelerometerAxes[2].Acceleration,
            device.accelerometerAxes[1].Acceleration)
        );

        //double pitchAngle = (double)Mathf.Asin((float)gravity.X);
        //double rollAngle = (double)Mathf.Asin((float)gravity.Z);
		pitchAngle = (double)Mathf.Asin((float)gravity.Z);
        rollAngle = (double)Mathf.Asin((float)gravity.X);

        //The board is up-side down
        if (gravity.Y < 0.0)
        {
            pitchAngle = -pitchAngle;
            rollAngle = -rollAngle;
        }

        //Construct a rotation matrix for rotating vectors measured in the body frame, into the earth frame
        //this is done by using the angles between the board and the gravity vector.
        Matrix3x3 xRotMatrix = new Matrix3x3();
        xRotMatrix.matrix[0, 0] = (double)Mathf.Cos((float)pitchAngle); xRotMatrix.matrix[1, 0] = (double)-Mathf.Sin((float)pitchAngle); xRotMatrix.matrix[2, 0] = 0;
        xRotMatrix.matrix[0, 1] = (double)Mathf.Sin((float)pitchAngle); xRotMatrix.matrix[1, 1] = (double)Mathf.Cos((float)pitchAngle); xRotMatrix.matrix[2, 1] = 0;
        xRotMatrix.matrix[0, 2] = 0; xRotMatrix.matrix[1, 2] = 0; xRotMatrix.matrix[2, 2] = 1;
		//xRotMatrix.matrix[0, 3] = 1; xRotMatrix.matrix[1, 3] = 1; xRotMatrix.matrix[2, 3] = 1; xRotMatrix.matrix[3, 3] = 1;
		//xRotMatrix.matrix[3, 0] = 1; xRotMatrix.matrix[3, 1] = 1; xRotMatrix.matrix[3, 2] = 1; 

        Matrix3x3 zRotMatrix = new Matrix3x3();
        zRotMatrix.matrix[0, 0] = 1; zRotMatrix.matrix[1, 0] = 0; zRotMatrix.matrix[2, 0] = 0;
        zRotMatrix.matrix[0, 1] = 0; zRotMatrix.matrix[1, 1] = (double)Mathf.Cos((float)rollAngle); zRotMatrix.matrix[2, 1] = (double)-Mathf.Sin((float)rollAngle);
        zRotMatrix.matrix[0, 2] = 0; zRotMatrix.matrix[1, 2] = (double)Mathf.Sin((float)rollAngle); zRotMatrix.matrix[2, 2] = (double)Mathf.Cos((float)rollAngle);
		//zRotMatrix.matrix[0, 3] = 1; zRotMatrix.matrix[1, 3] = 1; zRotMatrix.matrix[2, 3] = 1; zRotMatrix.matrix[3, 3] = 1;
		//zRotMatrix.matrix[3, 0] = 1; zRotMatrix.matrix[3, 1] = 1; zRotMatrix.matrix[3, 2] = 1; 

        Matrix3x3 rotMatrix = Matrix3x3.Multiply(xRotMatrix, zRotMatrix); 
		
	
        SpVector3 data = new SpVector3(device.compassAxes[0].MagneticField, device.compassAxes[2].MagneticField, -device.compassAxes[1].MagneticField);
        SpVector3 correctedData = Matrix3x3.Multiply(data, rotMatrix);
		
        //These represent the x and y components of the magnetic field vector in the earth frame
        Xh = -correctedData.X;
        Yh = -correctedData.Z;

        //we use the computed X-Y to find a magnetic North bearing in the earth frame
        try
        {
            double bearing = 0.0;
            double _360inRads = (double)(360.0f * Mathf.PI / 180.0f);
            if (Xh < 0.0)
                bearing = (double)(Mathf.PI - Mathf.Atan((float)(Yh / Xh)));
            else if (Xh > 0.0 && Yh < 0.0)
                bearing = (double)(-Mathf.Atan((float)(Yh / Xh)));
            else if (Xh > 0.0 && Yh > 0.0)
                bearing = (double)(Mathf.PI * 2.0f - Mathf.Atan((float)(Yh / Xh)));
            else if (Xh == 0.0 && Yh < 0.0)
                bearing = (double)(Mathf.PI / 2.0f);
            else if (Xh == 0.0 && Yh > 0.0)
                bearing = (double)(Mathf.PI * 1.5f);
            
            //The board is up-side down
            if (gravity.Y < 0.0)
            {
                bearing = (double)Mathf.Abs((float)(bearing - _360inRads));
            }

            //passing the 0 <-> 360 point, need to make sure the filter never contains both values near 0 and values near 360 at the same time.
            if (Mathf.Abs((float)(bearing - lastBearing)) > 2) //2 radians == ~115 degrees
            {
                if(bearing > lastBearing)
                    foreach (double[] stuff in compassBearingFilter)
                        stuff[0] += _360inRads;
                else
                    foreach (double[] stuff in compassBearingFilter)
                        stuff[0] -= _360inRads;
            }

            compassBearingFilter.Add(new double[] { bearing, pitchAngle, rollAngle });
            if (compassBearingFilter.Count > compassBearingFilterSize)
                compassBearingFilter.RemoveAt(0);

            bearing = pitchAngle = rollAngle = 0.0;
            foreach (double[] stuff in compassBearingFilter)
            {
                bearing += stuff[0];
                pitchAngle += stuff[1];
                rollAngle += stuff[2];
            }
            bearing /= compassBearingFilter.Count;
            pitchAngle /= compassBearingFilter.Count;
            rollAngle /= compassBearingFilter.Count;

            compassBearing = bearing * (180.0f / Mathf.PI);
            lastBearing = bearing;
		
			if(pitchAngleStart == -1000)
				pitchAngleStart = pitchAngle;
			//pitchAngle *= (180.0f / Mathf.PI);
			//rollAngle *= (180.0f / Mathf.PI);
			//print ("PitchAngle:"+pitchAngle.ToString("F3"));
			//print ("RollAngle:"+(rollAngle*(180.0f / Mathf.PI)).ToString("F3"));
			
            //bearingTxt.Text = (bearing * (180.0f / Math.PI)).ToString("F1") + "°";
            //xAngle.Text = (pitchAngle * (180.0f / Math.PI)).ToString("F1") + "°";
            //yAngle.Text = (rollAngle * (180.0f / Math.PI)).ToString("F1") + "°";

        }
        catch 
		{ 
			MonoBehaviour.print ("nogo");
		
		}
    }
}
