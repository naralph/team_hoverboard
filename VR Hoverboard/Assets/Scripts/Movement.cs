using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float moveRate = 0.1f;
    private float rotationRate = 10.0f;

    private float minSpeed = 0.1f;
    private float maxSpeed = 10.0f;
    private float idleSpeed = 0.1f;

    private Transform position;

    // Use this for initialization
    void Start()
    {
        position = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //keyboard
        //if (Input.GetKey("w"))
        //    position.Translate(position.forward * Time.deltaTime * moveRate);

        //else if (Input.GetKey("s"))
        //    position.Translate(-position.forward * Time.deltaTime * moveRate);

        //else if (Input.GetKey("d"))
        //    position.Translate(position.right * Time.deltaTime * moveRate);

        //else if (Input.GetKey("a"))
        //    position.Translate(-position.right * Time.deltaTime * moveRate);


        //gamepad left joystick
        //position.Translate(Input.GetAxis("LHorizontal") * position.right * Time.deltaTime * moveRate);



        float lVertVal = Input.GetAxis("LVertical");
        float currSpeed = moveRate * -Input.GetAxis("LVertical");

        print(lVertVal + " JOYSTICK VAL");
        print(currSpeed + " SPEED VAL");

        //getAxis returns a floating value that is in between -1 and 1
        //leaning down on the board, accelerating
        //if (Input.GetAxis("LVertical") < 0.0f && currSpeed > maxSpeed)
        //    currSpeed = maxSpeed;

        ////leaning back on the board, decelerating
        //if (Input.GetAxis("LVertical") > 0.0f && currSpeed < minSpeed)
        //    currSpeed = minSpeed;
        
        position.Translate(position.forward * currSpeed);

        //gamepad right joystick
        position.rotation = Quaternion.AngleAxis(Input.GetAxis("RVertical") * rotationRate, position.right);
    }
}
