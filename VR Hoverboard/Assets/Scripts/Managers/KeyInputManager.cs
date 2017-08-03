using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class KeyInputManager : MonoBehaviour
{
    ManagerClasses.GameState state;

    //variables for returning back to menu
    public float flippedTimer = 3f;
    public bool menuOnFlippedHMD = false;
    bool countingDown = false;
    float timeUpsideDown = 0f;

    public void setupKeyInputManager(ManagerClasses.GameState s)
    {
        state = s;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state.currentState != States.MainMenu)
            {
                EventManager.OnTriggerTransition(0);
            }
            else
            {
                Application.Quit();
            }
        }

        if (state.currentState != States.MainMenu && menuOnFlippedHMD && VRDevice.isPresent)
        {
            Quaternion q = InputTracking.GetLocalRotation(VRNode.Head);

            //if we're upside down, start the countdown and reset our timer
            if (q.eulerAngles.z > 150f && q.eulerAngles.z < 210f && !countingDown)
            {
                countingDown = true;
                timeUpsideDown = 0f;
            }
            else if (countingDown)
            {
                //if we're still upside down
                if (q.eulerAngles.z > 150f && q.eulerAngles.z < 210f)
                    timeUpsideDown += Time.deltaTime;
                else
                    countingDown = false;

                //go back to main menu once we've been upside down long enough
                if (timeUpsideDown > flippedTimer)
                {
                    countingDown = false;
                    EventManager.OnTriggerTransition(0);
                }
            }
        }
    }

}
