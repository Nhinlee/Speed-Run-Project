using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader
{
    // Singleton pattern
    private static GameLoader _instance = new GameLoader();

    public static GameLoader Instance { get => _instance; }

    private GameLoader() { }
    
    //-----------------------------------------------------------

    public void LoadCurrentMapScene()
    {
        // TODO: not implement save mode yet!
    }

    public void LoadMapScene()
    {
        SceneManager.LoadScene(SceneName.MapScene);
    }

    public void LoadLevelScene(int level)
    {
        SceneManager.LoadScene(SceneName.Level(level));
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(SceneName.MenuScene);
    }
}
