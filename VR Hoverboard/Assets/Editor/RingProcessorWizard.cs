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
    [Range(-1f, 1f)] [Tooltip("Increase or decrease the target bonus time based off of this percentage of the calculated bonus time.")]
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
    bool prevIsRotator, currIsRotator;

    [MenuItem("Cybersurf Tools/Ring Processor Wizard")]
    static void ProcessRings()
    {
        DisplayWizard<RingProcessorWizard>("Ring Processor Wizard", "Update And Close", "Update");
    }

    void Init()
    {
        currPosition = prevPosition = Vector3.zero;
        currQueuePosition = startPositionInOrder;

        previousGameObject = (GameObject)ringsToProcess[0];
        currentGameObject = null;
        prevIsRotator = false;
        currIsRotator = false;

        //initialize the prevPosition and our flag
        if (previousGameObject.GetComponent<RingProperties>() != null)
        {
            prevIsRotator = false;
            prevPosition = previousGameObject.GetComponent<RingProperties>().transform.position;
        }
        else if (previousGameObject.GetComponent<RingRotator>() != null)
        {
            prevIsRotator = true;
            prevPosition = previousGameObject.GetComponent<RingRotator>().transform.position;
        }
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
        RingRotator rr;
        RingProperties rp;
        for (int i = 1; i < ringsToProcess.Length; i++)
        {
            currentGameObject = (GameObject)ringsToProcess[i];

            //set our flags and our currPosition based off of what type of GameObject we have in the array
            if (currentGameObject.GetComponent<RingProperties>() != null)
            {
                currIsRotator = false;
                currPosition = currentGameObject.GetComponent<RingProperties>().transform.position;
            }
            else if (currentGameObject.GetComponent<RingRotator>() != null)
            {
                currIsRotator = true;
                currPosition = currentGameObject.GetComponent<RingRotator>().transform.position;
            }

            //set the time to reach based off of the prevous ring position to the current ring position
            if (prevIsRotator)
            {
                rr = previousGameObject.GetComponent<RingRotator>();

                rr.bonusTime = CalculateBonusTime();
                rr.positionInOrder = currQueuePosition;

                //make sure the unity editor saves the changes to the component
                UnityEditorInternal.ComponentUtility.CopyComponent(rr);
                UnityEditorInternal.ComponentUtility.PasteComponentValues(rr);
            }
            else
            {
                rp = previousGameObject.GetComponent<RingProperties>();

                rp.bonusTime = CalculateBonusTime();
                rp.positionInOrder = currQueuePosition;

                UnityEditorInternal.ComponentUtility.CopyComponent(rp);
                UnityEditorInternal.ComponentUtility.PasteComponentValues(rp);
            }

            //update our info for the next iteration
            ++currQueuePosition;
            prevIsRotator = currIsRotator;
            previousGameObject = currentGameObject;
            prevPosition = currPosition;
        }

        //set the last ring properties
        if (currIsRotator)
        {
            rr = currentGameObject.GetComponent<RingRotator>();

            if (setAsLastInScene)
            {
                rr.lastRingInScene = true;
                rr.nextScene = nextSceneIndex;
            }

            //since we don't have another ring in our array to base our time calculation off of, just set our bonus time to 0
            rr.bonusTime = 0f;
            rr.positionInOrder = currQueuePosition;

            UnityEditorInternal.ComponentUtility.CopyComponent(rr);
            UnityEditorInternal.ComponentUtility.PasteComponentValues(rr);
        }
        else
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

    private void OnWizardCreate()
    {
        Init();
        SetProperties();

        //mark the scene as dirty so we can save our changes
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private void OnWizardOtherButton()
    {
        Init();
        SetProperties();

        //mark the scene as dirty so we can save our changes
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
}
