using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CountDownTimer countDownTimer;

    [SerializeField]
    private SpeedBoyController speedBoyController;

    // Singleton
    static GameManager current;
    private void Awake()
    {
        if(current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(this);

        // Register call back
        speedBoyController.OnDied += Play;
        countDownTimer.onFinishCountdown += Play;
    }

    private void OnDestroy()
    {
        // Unregister call back
        speedBoyController.OnDied -= Play;
        countDownTimer.onFinishCountdown -= Play;
    }

    void Start()
    {
        // Start Countdown
        speedBoyController.StopRun();
        countDownTimer.StartCountDown();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            ReturnToMenu();
        }
    }

    private void ReturnToMenu()
    {
        // Load menu scene
        GameLoader.Instance.LoadMenuScene();
        // Clear level
        Destroy(gameObject);
    }

    private void Play()
    {
        speedBoyController.StartRun();
    }
}
