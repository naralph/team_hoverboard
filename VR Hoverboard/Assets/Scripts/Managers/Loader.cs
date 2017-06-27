using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//checks to see if our game manager is instantiated. If it isn't, then Loader instantiates it.
public class Loader : MonoBehaviour
{
    public GameObject gameManager;

    // Use this for initialization
    void Awake()
    {
        if (GameManager.instance == null)
            Instantiate(gameManager); //instantiates our prefab
    }
}
