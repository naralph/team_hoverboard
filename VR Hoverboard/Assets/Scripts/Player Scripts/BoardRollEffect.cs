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

    [SerializeField] float rollIncreaseRate = 1.2f;
    [SerializeField] float rollDecreaseRate = 0.1f;
    [SerializeField] float maxRollDegree = 25f;

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

    IEnumerator BoardRollCoroutine()
    {
        yield return new WaitForFixedUpdate();

        //no reason to do any calculations if there was no difference between rotations
        if (prevYRotation != playerTransform.eulerAngles.y)
        {
            zRotation += Mathf.DeltaAngle(playerTransform.eulerAngles.y, prevYRotation) * rollIncreaseRate;
           //print("Z ROT: " + zRotation);


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
