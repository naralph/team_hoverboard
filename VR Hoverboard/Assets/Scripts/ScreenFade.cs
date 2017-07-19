using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFade : MonoBehaviour
{
    
    private bool isFading = false;
    private YieldInstruction fadeInstruction = new WaitForEndOfFrame();
    
    // Starts the fade in
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinished;
        EventManager.OnFade += startFadeOutCoroutine;
        StartCoroutine(FadeIn());
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinished;
        EventManager.OnFade -= startFadeOutCoroutine;
    }

    // Starts a fade in when a new level is loaded
    void OnLevelFinished(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    void startFadeOutCoroutine()
    {
        StartCoroutine(FadeOut());
    }
    // Fades alpha from 1.0 to 0.0, use at beginning of scene
    IEnumerator FadeIn()
    {
        yield return fadeInstruction;
        isFading = false;
    }

    // Fades from 0.0 to 1.0, use at end of scene
    public IEnumerator FadeOut()
    {
        yield return fadeInstruction;
        isFading = false;
        GameManager.instance.levelScript.fadeing = false;
    }

    // Renders the fade overlay when attached to a camera object
    void OnPostRender()
    {

    }
}
