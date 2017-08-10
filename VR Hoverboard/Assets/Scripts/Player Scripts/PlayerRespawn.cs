using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRespawn : MonoBehaviour
{
    public Image theFadeObj;
    public float fadeSpeed = 0.8f;
    public float fadeTime = 4.0f;

    [HideInInspector]
    public bool isRespawning = false;

    Rigidbody playerRB;
    Transform respawnPoint;
    float alpha;
    float currTime;

    ManagerClasses.RoundTimer roundTimer;
    float roundTimerStartTime;

    private void Start()
    {
        playerRB = GameManager.player.GetComponent<Rigidbody>();
        roundTimer = GameManager.instance.roundTimer;
    }

    public void RespawnPlayer(Transform rsPoint, float startTime)
    {
        isRespawning = true;
        respawnPoint = rsPoint;
        roundTimerStartTime = startTime;
        currTime = 0f;
        alpha = theFadeObj.color.a;

        StartCoroutine(FadeOut());
    }

    void UpdateAlpha(float direction)
    {
        alpha += direction * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        theFadeObj.color = new Color(0f, 0f, 0f, alpha);
    }

    IEnumerator FadeOut()
    {
        bool lockSet = false;

        //keep going until our fade time as elapsed
        while (currTime < fadeTime)
        {
            //only update our alpha if it isn't 0
            UpdateAlpha(1);

            //once alpha is == 1, lock movement
            if (!lockSet && alpha == 1f)
            {
                EventManager.OnSetMovementLock(true);
                lockSet = true;
            }

            currTime += Time.deltaTime;
            yield return null;
        }

        //once we fade out, move the player
        playerRB.MovePosition(respawnPoint.position);       
        playerRB.MoveRotation(Quaternion.Euler(respawnPoint.eulerAngles.x, respawnPoint.eulerAngles.y, 0f));

        //then start to fade in
        currTime = 0f;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (currTime < fadeTime)
        {
            if (alpha != 0f)
                UpdateAlpha(-1);

            currTime += Time.deltaTime;
            yield return null;
        }

        //TODO:: start the countdown splash screen to alert the player that movement is about to unlock

        //unlock movement
        EventManager.OnSetMovementLock(false);

        //start the timer
        roundTimer.timeLeft = roundTimerStartTime;

        isRespawning = false;
        respawnPoint = null;
    }
}
