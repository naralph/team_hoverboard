using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundaryDistanceFade : MonoBehaviour {

    Transform player;

    float backWall;
    float frontWall;
    float rightWall;
    float leftWall;
    float ceiling;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player(Clone)").GetComponent<Transform>();

        float width = gameObject.transform.localScale.x;
        float height = gameObject.transform.localScale.y;
        float length = gameObject.transform.localScale.z;

        frontWall = (gameObject.transform.position + transform.forward * length/2).z;
        backWall = (gameObject.transform.position - transform.forward * length / 2).z;

        leftWall = (gameObject.transform.position - transform.right * width / 2).x;
        rightWall = (gameObject.transform.position + transform.right * width / 2).x;

        ceiling = (gameObject.transform.position + transform.up * height / 2).y;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
}
