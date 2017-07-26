using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFade : MonoBehaviour
{

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;
    public float fadeTime = 10.0f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;
    
    private bool isFading = false;
    
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
        fadeTime = BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
        isFading = false;
    }

    // Fades from 0.0 to 1.0, use at end of scene
    public IEnumerator FadeOut()
    {
        fadeTime = BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        isFading = false;
        GameManager.instance.levelScript.fadeing = false;
    }

    private void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade (int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }
}
