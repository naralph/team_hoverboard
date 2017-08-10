using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class RingProcessorWizard : ScriptableWizard
{
    [Header("Bonus Time and Queue Order Settings")]
    [Range(10f, 50f)]
    public float targetVelocity = 20f;
    [Range(-1f, 1f)]
    [Tooltip("Increase or decrease the target bonus time based off of this percentage of the calculated bonus time.")]
    public float timePercentModifier = 0f;
    public int startPositionInOrder = 1;

    [Header("Last Ring Settings")]
    public bool setAsLastInScene = true;
    public int nextSceneIndex = 0;

    [Header("Drag Rings and Rotators In Desired Order Here")]
    public Object[] ringsToProcess;

    Vector3 prevPosition, currPosition;
    int currQueuePosition;
    GameObject previousGameObject, currentGameObject;

    [MenuItem("Cybersurf Tools/Ring Processor Wizard")]
    static void ProcessRings()
    {
        DisplayWizard<RingProcessorWizard>("Ring Processor Wizard", "Update And Close", "Update");
    }

    bool Init()
    {
        if (ringsToProcess == null || ringsToProcess.Length < 2)
            return false;

        currPosition = prevPosition = Vector3.zero;
        currQueuePosition = startPositionInOrder;

        //initialize the prevPosition and previousGameObject
        for (int i = 0; i < ringsToProcess.Length; i++)
        {
            previousGameObject = (GameObject)ringsToProcess[i];

            if (previousGameObject.GetComponent<RingProperties>() != null)
            {
                prevPosition = previousGameObject.GetComponent<RingProperties>().transform.position;
                break;
            }
        }

        if (previousGameObject != null)
            return true;
        else
            return false;
    }

    #region debug code
    //int firstRing = 1, secondRing = 2;
    #endregion
    float CalculateBonusTime()
    {
        float distance = Vector3.Distance(prevPosition, currPosition);

        #region debug code
        //Debug.Log("Distance from ring " + firstRing + " to " + secondRing + ": " + distance);
        //++firstRing;
        //++secondRing;
        #endregion

        return (distance / targetVelocity) + distance * timePercentModifier / targetVelocity;
    }

    void SetProperties()
    {
        RingProperties rp;
        for (int i = 1; i < ringsToProcess.Length; i++)
        {
            currentGameObject = (GameObject)ringsToProcess[i];

            //set the time to reach based off of the prevous ring position to the current ring position
            if (currentGameObject.GetComponent<RingProperties>() != null)
            {
                currPosition = currentGameObject.GetComponent<Transform>().position;

                rp = previousGameObject.GetComponent<RingProperties>();

                rp.bonusTime = CalculateBonusTime();
                rp.positionInOrder = currQueuePosition;

                UnityEditorInternal.ComponentUtility.CopyComponent(rp);
                UnityEditorInternal.ComponentUtility.PasteComponentValues(rp);


            }

            //update our info for the next iteration
            ++currQueuePosition;
            previousGameObject = currentGameObject;
            prevPosition = currPosition;
        }

        //set the last ring properties
        if (currentGameObject.GetComponent<RingProperties>() != null)
        {
            rp = currentGameObject.GetComponent<RingProperties>();

            if (setAsLastInScene)
            {
                rp.lastRingInScene = true;
                rp.nextScene = nextSceneIndex;
            }

            rp.bonusTime = 0f;
            rp.positionInOrder = currQueuePosition;

            UnityEditorInternal.ComponentUtility.CopyComponent(rp);
            UnityEditorInternal.ComponentUtility.PasteComponentValues(rp);
        }
    }

    //our Update And Close button
    private void OnWizardCreate()
    {
        if (Init())
        {
            SetProperties();

            //mark the scene as dirty so we can save our changes
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }

    //our Update button
    private void OnWizardOtherButton()
    {
        if (Init())
        {
            helpString = "Rings Processed!";
            SetProperties();

            //mark the scene as dirty so we can save our changes
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        else
            helpString = "Not enough items in the Rings To Process array!";
    }
}
