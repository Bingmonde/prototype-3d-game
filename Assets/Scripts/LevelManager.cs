using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    public GameLevels currentLevel = GameLevels.Difficle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

        }
    }


    void Start()
    {
        Debug.Log("current level:" + currentLevel);   
    }

    public void SetCurrentGameLevel(GameLevels level) {
        currentLevel = level;
    }

}

public enum GameLevels { 
    Normal, Difficle
}
