using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryMenuEvents : MonoBehaviour
{
    public void ChangeScene(int index)
    {
        EventManager.OnTriggerSceneChange(index);
    }

}