using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextElementControllerScript : MonoBehaviour {
    GameObject fpsText;
    GameObject timerText;
    GameObject scoreText;
    GameObject arrow;
    GameObject ringCountText;

    public bool allToggledOff = false;

    public void toggleFPSText()
    {
        fpsText.SetActive(!fpsText.activeSelf);
    }
    public void toggleTimertext()
    {
        timerText.SetActive(!timerText.activeSelf);
    }
    public void toggleScoreText()
    {
        scoreText.SetActive(!scoreText.activeSelf);
    }
    public void toggleArrow()
    {
        arrow.SetActive(!arrow.activeSelf);
    }
    public void toggleRingCountText()
    {
        ringCountText.SetActive(!ringCountText.activeSelf);
    }
    public void toggleAllOff()
    {
        if (fpsText.activeSelf)
        {
            toggleFPSText();
        }
        if(timerText.activeSelf)
        {
            toggleTimertext();
        }
        if(scoreText.activeSelf)
        {
            toggleScoreText();
        }
        if (arrow.activeSelf)
        {
            toggleArrow();
        }
        if(ringCountText.activeSelf)
        {
            toggleRingCountText();
        }
    }
    public void toggleAllOn()
    {
        if (!fpsText.activeSelf)
        {
            toggleFPSText();
        }
        if (!timerText.activeSelf)
        {
            toggleTimertext();
        }
        if (!scoreText.activeSelf)
        {
            toggleScoreText();
        }
        if (!arrow.activeSelf)
        {
            toggleArrow();
        }
        if (!ringCountText.activeSelf)
        {
            toggleRingCountText();
        }
    }

    void setHud(bool isOn)
    {
        if (isOn)
        {
            toggleAllOn();
        }
        else
        {
            toggleAllOff();
        }
    }

    private void OnEnable()
    {
        fpsText = GetComponentInChildren<FPSTextUpdateScript>().gameObject;
        timerText = GetComponentInChildren<TimerTextUpdateScript>().gameObject;
        scoreText = GetComponentInChildren<ScoreTextUpdateScript>().gameObject;
        arrow = GetComponentInChildren<ArrowAimAt>().gameObject;
        ringCountText = GetComponentInChildren<RingCountTextUpdate>().gameObject;
        EventManager.OnToggleHud += setHud;
    }

    private void OnDisable()
    {
        EventManager.OnToggleHud -= setHud;
    }
}
