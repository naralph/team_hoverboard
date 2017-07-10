using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//we'll use our LevelManger to initialize any objects that carry from one scene to the next
public class LevelManager : MonoBehaviour
{
    GameState state;
    GameObject player;

    //stores each player spawn point at each different level
    public Transform[] spawnPoints;

    public void SetupLevelManager(GameState s, GameObject p)
    {
        player = p;
        state = s;

        InitializeLevel(SceneManager.GetActiveScene().buildIndex);
    }

    //called by our GameManager once the scene changes
    public void InitializeLevel(int sceneIndex)
    {
        //switch (state)
        //{
        //    case GameState.MainMenu:
        //        //do things like lock player movement here....
        //        break;
        //    case GameState.GamePlay:
        //        //do things like unlock player movement here....
        //        break;
        //    case GameState.GameOver:
                
        //        break;
        //    default:
        //        break;
        //}

        //player.transform.rotation = spawnPoints[sceneIndex].rotation;
        //player.transform.position = spawnPoints[sceneIndex].position;

        SceneManager.LoadScene(sceneIndex);
    }

}
