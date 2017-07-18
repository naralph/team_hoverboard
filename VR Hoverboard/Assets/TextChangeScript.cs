using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextChangeScript : MonoBehaviour {
    GameManager manager;

    
	void Start () {
        manager = GameManager.instance;
	}
	
	void Update ()
    {
        string textToWrite = " " + manager.roundTimer.currRoundTime + " ";
        gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText(textToWrite);
	}
}
