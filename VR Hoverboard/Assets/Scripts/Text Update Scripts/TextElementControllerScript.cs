using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextElementControllerScript : MonoBehaviour {
    GameObject fpsText;
    GameObject timerText;
    GameObject scoreText;
    GameObject arrow;
    GameObject ringCountText;

    public struct hudElementsBools
    {
        public bool timerBool;
        public bool scoreBool;
        public bool fpsBool;
        public bool arrowBool;
        public bool ringCountBool;
        public bool overAllBool;
    }
    public hudElementsBools hudElementsControl;

    public void setTimer(bool isOn) { hudElementsControl.timerBool = isOn; }
    public void setScore(bool isOn) { hudElementsControl.scoreBool = isOn; }
    public void setFPS(bool isOn) { hudElementsControl.fpsBool = isOn; }
    public void setArrow(bool isOn){ hudElementsControl.arrowBool = isOn; }
    public void setRingCount(bool isOn) { hudElementsControl.ringCountBool = isOn; }
    public void setAll(bool isOn)
    {
        setTimer(isOn);
        setScore(isOn);
        setFPS(isOn);
        setArrow(isOn);
        setRingCount(isOn);
        hudElementsControl.overAllBool = isOn;
    }

    //For level use
    public void gameStart()
    {
        timerText.SetActive(hudElementsControl.timerBool);
        scoreText.SetActive(hudElementsControl.scoreBool);
        fpsText.SetActive(hudElementsControl.fpsBool);
        arrow.SetActive(hudElementsControl.arrowBool);
        ringCountText.SetActive(hudElementsControl.ringCountBool);
    }
 
    //For menu's use
    public void menuStart()
    {
        if (fpsText.activeSelf)
        {
            fpsText.SetActive(false);
        }
        if(timerText.activeSelf)
        {
            timerText.SetActive(false);
        }
        if(scoreText.activeSelf)
        {
            scoreText.SetActive(false);
        }
        if (arrow.activeSelf)
        {
            arrow.SetActive(false);
        }
        if(ringCountText.activeSelf)
        {
            ringCountText.SetActive(false);
        }
    }
   

    void setHud(bool isOn)
    {
        if (isOn)
        {
            gameStart();
        }
        else
        {
            menuStart();
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
