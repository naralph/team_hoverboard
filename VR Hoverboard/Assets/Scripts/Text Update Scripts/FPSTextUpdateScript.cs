using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSTextUpdateScript : MonoBehaviour {
    
    TextMeshProUGUI element;

    float frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;

    void Start()
    {
        element = gameObject.GetComponent<TextMeshProUGUI>();
    }
	
	void Update ()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0f/updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }

        string textToWrite = " " + fps.ToString("n2") + " ";
        element.SetText(textToWrite);
	}
}
