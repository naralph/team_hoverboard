using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//we'll use our LevelManger to initialize any objects that carry from one scene to the next
public class LevelManager : MonoBehaviour
{
    GameObject player;

    //stores each player spawn point at each different level
    public Transform[] spawnPoints;

    public void SetupLevelManager(GameObject p)
    {
        player = p;
    }

    //called by our GameManager once the scene changes
    public void InitializeLevel(int scene)
    {
        SceneManager.LoadScene(scene);
        //player position = spawnPoints[scene]
    }

}
