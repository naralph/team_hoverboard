using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundaryDistanceFade : MonoBehaviour
{
    [SerializeField]
    float minimumDistance = 20;
    [SerializeField]
    float ratio = 0;

    [SerializeField]
    float cullDistance = 100;
    [SerializeField]
    int layer = 8;

    MeshRenderer myRenderer;

    Transform player;

    Vector3 backWall;
    Vector3 frontWall;
    Vector3 rightWall;
    Vector3 leftWall;
    Vector3 ceiling;

    //vectors from center of object to its walls
    Vector3[] distancesToCenter = new Vector3[5];

    // Use this for initialization
    void Start()
    {
        myRenderer = gameObject.GetComponent<MeshRenderer>();

        myRenderer.material.color = new Color(
            myRenderer.material.color.r,
            myRenderer.material.color.g,
            myRenderer.material.color.g, 0);

        player = GameObject.Find("Player(Clone)").GetComponent<Transform>();

        float width = gameObject.transform.localScale.x;
        float height = gameObject.transform.localScale.y;
        float length = gameObject.transform.localScale.z;

        frontWall = (gameObject.transform.position + transform.forward * length / 2);
        backWall = (gameObject.transform.position - transform.forward * length / 2);

        rightWall = (gameObject.transform.position + transform.right * width / 2);
        leftWall = (gameObject.transform.position - transform.right * width / 2);

        ceiling = (gameObject.transform.position - transform.up * height / 2);

        //front
        distancesToCenter[0] = frontWall - gameObject.transform.position;
        //back
        distancesToCenter[1] = backWall - gameObject.transform.position;
        //right
        distancesToCenter[2] = rightWall - gameObject.transform.position;
        //left
        distancesToCenter[3] = leftWall - gameObject.transform.position;
        //ceiling
        distancesToCenter[4] = ceiling - gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //vector from player to walls
        Vector3[] distancesToPlayer = new Vector3[5];
        //front
        distancesToPlayer[0] = (frontWall - player.transform.position);
        //back
        distancesToPlayer[1] = (backWall - player.transform.position);
        //right
        distancesToPlayer[2] = (rightWall - player.transform.position);
        //left
        distancesToPlayer[3] = (leftWall - player.transform.position);
        //ceiling
        distancesToPlayer[4] = (ceiling - player.transform.position);

        //index of wall player is closest to
        int index = 0;
        //temporary variable to help find the wall the player is closest to by
        //seting itself to lowest distance
        float temp = distancesToPlayer[index].magnitude;
        for (int i = 0; i < 5; i++)
        {
            if (temp > distancesToPlayer[i].magnitude)
            {
                index = i;
                temp = distancesToPlayer[index].magnitude;
            }
        }

        float difference = distancesToCenter[index].magnitude - distancesToPlayer[index].magnitude;

        if (difference <= minimumDistance)
        {
            int thing = 34;
        }

        ratio = 1;

        //gameObject.GetComponent<LODGroup>().

        myRenderer.material.color = new Color(
            myRenderer.material.color.r,
            myRenderer.material.color.g,
            myRenderer.material.color.b, ratio);
    }
}
