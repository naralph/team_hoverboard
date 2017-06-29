using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//we'll use our SceneManger to initialize any objects that carry from one scene to the next
public class SceneManager : MonoBehaviour
{
    //stores each player spawn point at each different scene
    public Transform[] spawnPoints;

    public void SetupSceneManager()
    {

    }

    //called by our GameManager once the scene changes
    public void InitializeScene(int scene)
    {

    }

}
