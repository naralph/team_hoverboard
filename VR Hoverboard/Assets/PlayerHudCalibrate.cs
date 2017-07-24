using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class PlayerHudCalibrate : MonoBehaviour
{
    public Transform canvasTransform;

    void UpdateHeight(bool selectionLocked)
    {
        //selection locked gets set to false after the player is rotated and transformed in LevelManager
        if (!selectionLocked && VRDevice.isPresent)
        {
            //set the height of the canvas to the height of the headset
            //doing this may not be necissary if we don't set InputTracking.disablePositionalTracking to false in the GameManager....
            canvasTransform.position = new Vector3(canvasTransform.position.x, InputTracking.GetLocalPosition(VRNode.Head).y, canvasTransform.position.z);
        }
    }

    private void OnEnable()
    {
        EventManager.OnSelectionLock += UpdateHeight;
    }

    private void OnDisable()
    {
        EventManager.OnSelectionLock -= UpdateHeight;
    }
}
