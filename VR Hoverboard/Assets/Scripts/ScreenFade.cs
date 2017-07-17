using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFade : MonoBehaviour
{
    public float fadeTime = 200.0f;

    //The initial screen color. SHould have opposite alphas(0.0 and 1.0)
    public Color fadeOutColor = new Color(0.01f, 0.01f, 0.01f, 0.0f);
    public Color fadeInColor = new Color(0.01f, 0.01f, 0.01f, 1.0f);

    public Material fadeMaterial = null;
    private bool isFading = false;
    private YieldInstruction fadeInstruction = new WaitForEndOfFrame();

    // Initialize.
    void Awake()
    {
        // create the fade material
        fadeMaterial = new Material(Shader.Find("Oculus/Unlit Transparent Color"));
    }

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

    // Cleans up the fade material
    void OnDestroy()
    {
        if (fadeMaterial != null)
        {
           //Destroy(fadeMaterial);
        }
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
        if (isFading)
        {
            fadeMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Color(fadeMaterial.color);
            GL.Begin(GL.QUADS);
            GL.Vertex3(0f, 0f, -12f);
            GL.Vertex3(0f, 1f, -12f);
            GL.Vertex3(1f, 1f, -12f);
            GL.Vertex3(1f, 0f, -12f);
            GL.End();
            GL.PopMatrix();
        }
    }
}
