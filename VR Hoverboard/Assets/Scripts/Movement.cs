using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public bool debugControls = true;
    public float moveRate = 50.0f;

    //static float currRotation;
    private float rotationRate = 10.0f;

    private float minSpeed = 0.025f;
    private float maxSpeed = 10.0f;

    private Transform position;

    // Use this for initialization
    void Start()
    {
        position = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //GetAxis returns a floating value that is in-between -1 and 1
        float lVertVal = Input.GetAxis("LVertical");
        float lHoriVal = Input.GetAxis("LHorizontal");
        float currSpeed = moveRate * Time.deltaTime;

        print(lVertVal + " LEFT VERT JOYSTICK VAL");
        print(lHoriVal + " LEFT HORIZONTAL JOYSTICK VAL");
        print(currSpeed + " SPEED VAL");

        if (debugControls)
        {
            //rotates about the x axis
            position.Rotate(Vector3.right * Input.GetAxis("RVertical"));
            //rotates about the y axis
            position.Rotate(Vector3.up * Input.GetAxis("LHorizontal"));
            //rotates about the z axis
            position.Rotate(Vector3.forward * Input.GetAxis("LHorizontal"));
            //rotates about the y axis
            position.Rotate(Vector3.up * Input.GetAxis("RHorizontal"));
            //translates forward
            position.Translate(Vector3.forward * Input.GetAxis("LVertical") * currSpeed);
        }
        else
        {
            currSpeed += minSpeed;
            position.Translate(Vector3.forward * minSpeed);

            //leaning down on the board, accelerating
            if (Input.GetAxis("LVertical") < 0.0f && currSpeed > maxSpeed)
                currSpeed = maxSpeed;

            //leaning back on the board, decelerating
            if (Input.GetAxis("LVertical") > 0.0f && currSpeed < minSpeed)
                currSpeed = minSpeed;
        }
    }
}