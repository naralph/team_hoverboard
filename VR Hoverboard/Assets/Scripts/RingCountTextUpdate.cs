using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RingCountTextUpdate : MonoBehaviour
{
    GameManager manager;
    TextMeshProUGUI element;

	// Use this for initialization
	void Start () {
        manager = GameManager.instance;
        element = gameObject.GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
        string textToWrite = " " + manager.scoreScript.ringHitCount + " ";
        element.SetText(textToWrite);
	}
}
