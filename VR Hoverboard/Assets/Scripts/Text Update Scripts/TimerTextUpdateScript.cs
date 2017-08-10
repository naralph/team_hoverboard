using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerTextUpdateScript : MonoBehaviour {
    GameManager manager;
    TextMeshProUGUI element;
    
	void Start () {
        manager = GameManager.instance;
        element = gameObject.GetComponent<TextMeshProUGUI>();

    }
	
	void Update ()
    {
        string textToWrite = " " + manager.roundTimer.timeLeft.ToString("n2") + " ";
        element.SetText(textToWrite);
	}
}
