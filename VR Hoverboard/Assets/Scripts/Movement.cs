using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float moveRate = 10.0f;

    private Transform position;

    // Use this for initialization
    void Start()
    {
        position = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
            position.Translate(position.forward * Time.deltaTime * moveRate);

        else if (Input.GetKey("s"))
            position.Translate(-position.forward * Time.deltaTime * moveRate);

        else if (Input.GetKey("d"))
            position.Translate(position.right * Time.deltaTime * moveRate);

        else if (Input.GetKey("a"))
            position.Translate(-position.right * Time.deltaTime * moveRate);
    }
}
