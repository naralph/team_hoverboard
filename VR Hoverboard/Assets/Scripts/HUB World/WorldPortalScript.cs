using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPortalScript : MonoBehaviour
{
    int sceneIndex = 0;
    System.Type boxCollider;
    Rigidbody playerRB;

    private void Start()
    {
        boxCollider = typeof(UnityEngine.BoxCollider);
        playerRB = GameManager.player.GetComponent<Rigidbody>();
        sceneIndex = GetComponentInParent<WorldPortalProperties>().sceneIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == boxCollider && other.gameObject.tag == "Player")        
            EventManager.OnTriggerTransition(sceneIndex);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetType() == boxCollider)
        {
            if (playerRB.velocity != Vector3.zero)
            playerRB.velocity = Vector3.zero;

            if (playerRB.angularVelocity != Vector3.zero)
                playerRB.angularVelocity = Vector3.zero;
        }
    }
}
