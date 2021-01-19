using System.Collections;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    // Singleton
    private static AppManager current;
    public static AppManager Instance { get => current; }
    // ---------------------------------------------------------

    // Private Fields
    private PlayerData playerData = new PlayerData();


    private void Awake()
    {
        // Singleton Pattern
        if (current != null && current != this)
        {
            Destroy(this);
            return;
        }
        current = this;
        DontDestroyOnLoad(this);

        // Initial game
        InitGame();
    }

    private void InitGame()
    {
        // Start Game App ---------------------------------------------------------
        //
        playerData.LoadPlayerDataFromFile();
        //
    }

    private void Update()
    {
        // This is just for testing porpuse
        if (Input.GetKey(KeyCode.Escape))
        {
            GameLoader.Instance.LoadMapScene();
            AudioManager.Instance.PlayStartGameMusic();
        }
    }
}


