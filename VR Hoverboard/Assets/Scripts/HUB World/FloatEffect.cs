using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public enum StartDirection { Up, Down };
    public Transform objectTransform;

    [Space]

    public bool randomizeStartHeight = true;

    float originalHeight;
    float direction = 1f;

    public StartDirection startDirection = StartDirection.Up;
    public float floatDistance = 0.5f;
    public float floatRate = 0.1f;

    // Use this for initialization
    void Start ()
    {
        originalHeight = objectTransform.position.y;

        if (randomizeStartHeight)
        {
            float randomStartHeight = Random.Range(-floatDistance, floatDistance);
            objectTransform.Translate(0f, randomStartHeight, 0f);
        }

        if (startDirection != StartDirection.Up)
            direction = -1f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (direction > 0f)
        {
            if (!(objectTransform.position.y < originalHeight + floatDistance))
                direction *= -1f;
        }
        else
        {
            if (!(objectTransform.position.y > originalHeight - floatDistance))
                direction *= -1f;
        }

        objectTransform.Translate(0f, Time.deltaTime * floatRate * direction, 0f);
    }
}
