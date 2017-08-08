using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardRollEffect : MonoBehaviour
{
    Transform playerTransform;
    Transform boardTransform;
    float zRotation;
    float prevYRotation;

    public float rollIncreaseRate = 1.2f;
    public float rollDecreaseRate = 0.1f;
    public float maxRollDegree = 25f;

    void LevelSelectionUnlocked(bool locked)
    {
        if (locked == false)
        {
            StopAllCoroutines();

            if (playerTransform == null)
                playerTransform = GetComponentInParent<Transform>();

            if (boardTransform == null)
                boardTransform = GetComponent<Transform>();

            //reset our zRotation and prevYRotation once level manager has updated the player position
            zRotation = 0f;
            prevYRotation = playerTransform.eulerAngles.y;

            StartCoroutine(BoardRollCoroutine());
        }
    }

    //since degrees are measured from 0 to 360, we have to account for an edge case
    //  where one of our recorded values is below 360, while the other is above 0
    float RotationDifference()
    {
        float difference = 0f;
        float currPlayerY = playerTransform.eulerAngles.y;
        float prevPlayerY = prevYRotation;

        if (prevYRotation < 10f && playerTransform.eulerAngles.y > 350f)
            currPlayerY = 360f - playerTransform.eulerAngles.y;

        else if (prevYRotation > 350f && playerTransform.eulerAngles.y < 10f)
            prevPlayerY = 360f - prevPlayerY;

        difference = prevPlayerY - currPlayerY;
        //print("Difference: " + difference);

        return difference;
    }

    IEnumerator BoardRollCoroutine()
    {
        yield return new WaitForFixedUpdate();

        //no reason to do any calculations if there was no difference between rotations
        if (prevYRotation != playerTransform.eulerAngles.y)
        {
            zRotation += RotationDifference() * rollIncreaseRate;
           print("Z ROT: " + zRotation);

            //clamp our rotation to our maxRollDegree
            if (zRotation > maxRollDegree)
                zRotation = maxRollDegree;
            else if (zRotation < -maxRollDegree)
                zRotation = -maxRollDegree;

            //don't forget to update our prevYRotation
            prevYRotation = playerTransform.eulerAngles.y;
        }

        //only update our zRotation if there is a rotation to update
        if (zRotation != 0f)
        {
            //lerp to 0 based off of our rollDecreaseRate
            zRotation = Mathf.Lerp(zRotation, 0f, rollDecreaseRate);

            //set our rotation to 0 if we're within tolerance
            if (zRotation < 0.1f && zRotation > -0.1f)
                zRotation = 0f;

            boardTransform.rotation = Quaternion.Euler(boardTransform.eulerAngles.x, boardTransform.eulerAngles.y, zRotation);
        }

        StartCoroutine(BoardRollCoroutine());
    }

    private void OnEnable()
    {
        EventManager.OnSelectionLock += LevelSelectionUnlocked;
    }

    private void OnDisable()
    {
        EventManager.OnSelectionLock -= LevelSelectionUnlocked;
    }
}
