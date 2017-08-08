using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCollisionSoundEffects : MonoBehaviour {

    private AudioSource source;

    bool hitSphere = false;
    bool hitCube = false;

    bool hitRing = false;

    [SerializeField]
    float timeToWait = 1.5f;
    [SerializeField]
    float timeIncrement = 0.001f;
    float timeWaited = 0;

    [SerializeField]
    AudioClip wallCollision;
    [SerializeField]
    AudioClip ringCollision;
    [SerializeField]
    AudioClip portalEnter;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}

    private void FixedUpdate()
    {
        if (hitRing)
        {
            timeWaited += timeIncrement;
        }

        if (timeWaited >= timeToWait)
        {
            hitCube = false;
            hitSphere = false;
            hitRing = false;
            timeWaited = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        source.clip = wallCollision;
        
        source.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == typeof(CapsuleCollider) && !hitRing && !hitCube)
        {
            if (other.gameObject.tag == "Ring")
            {
                source.clip = ringCollision;
                source.Play();
                hitRing = true;
                hitSphere = true;
            }
            if (other.gameObject.tag == "Portal")
            {
                source.clip = portalEnter;
                source.Play();
            }
        }
        else if(!hitRing && !hitSphere)
        {
            if (other.gameObject.tag == "Ring")
            {
                source.clip = ringCollision;
                source.Play();
                hitRing = true;
                hitCube = true;
            }
            if(other.gameObject.tag == "Portal")
            {
                source.clip = portalEnter;
                source.Play();
            }
        }
    }
}
