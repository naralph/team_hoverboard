using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStandScript : MonoBehaviour
{
    public enum StartDirection { Up, Down };

    float originalHeight;
    float direction = 1f;

    [Header("Board Floating Effect")]
    public Transform boardTransform;
    public StartDirection startDirection = StartDirection.Up;
    public float floatDistance = 0.5f;
    public float floatRate = 0.1f;

    //variables used by BoardStandSelect
    [Header("Board Type")]
    public BoardType boardType = BoardType.Beginner;
    public Material boardMaterial;

    void Start()
    {
        originalHeight = boardTransform.position.y;

        if (startDirection != StartDirection.Up)
            direction = -1f;
    }

    void Update()
    {
        if (direction > 0f)
        {
            if (!(boardTransform.position.y < originalHeight + floatDistance))
                direction *= -1f;
        }
        else
        {
            if (!(boardTransform.position.y > originalHeight - floatDistance))
                direction *= -1f;
        }

        boardTransform.Translate(0f, 0f, Time.deltaTime * floatRate * direction);
    }
}
