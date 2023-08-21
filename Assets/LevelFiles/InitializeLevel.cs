using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    public string levelPath;

    public LevelData LevelData;
    // The Entry point of the whole level

    /*
    void Start()
    {
        LevelData.FromFile(levelPath);
        GameplayLogic gameplayLogic = GameObject.Find("GameLogic").GetComponent<GameplayLogic>();
        gameplayLogic.Init();
    }*/
}
