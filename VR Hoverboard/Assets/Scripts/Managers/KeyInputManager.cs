using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputManager : MonoBehaviour
{
    ManagerClasses.GameState state;

	public void setupKeyInputManager(ManagerClasses.GameState s)
    {
        state = s;
    }
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(state.currentState == States.GamePlay)
            {
                EventManager.OnTriggerTransition(0);
            }
            else
            {
                Application.Quit();
            }
        }
	}
}
