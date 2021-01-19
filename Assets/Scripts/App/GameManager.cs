using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CountDownTimer countDownTimer;
    // Start is called before the first frame update

    void Start()
    {
        // Start Countdown
        countDownTimer.StartCountDown();
    }

}
