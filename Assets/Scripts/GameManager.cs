using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CountDownTimer countDownTimer;
    // Start is called before the first frame update

    private void Awake()
    {
        var numGameObject = FindObjectsOfType<GameManager>().Length;
        if (numGameObject > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        countDownTimer.StartCountDown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
