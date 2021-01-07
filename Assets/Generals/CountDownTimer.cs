using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownTimer : MonoBehaviour
{
    public Action onFinishCountdown;

    [SerializeField]
    [Range(0,5)]
    private int timer;

    [SerializeField]
    private TextMeshProUGUI txtCountDownNumber;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoroutineStartCountdown());
    }

    private IEnumerator CoroutineStartCountdown()
    {
        int countdownTimer = timer;

        while(countdownTimer > 1)
        {
            txtCountDownNumber.text = countdownTimer.ToString();
            yield return new WaitForSeconds(1);
            countdownTimer--;
        }
        txtCountDownNumber.text = "RUN";
        yield return new WaitForSeconds(1);

        txtCountDownNumber.text = "";
        onFinishCountdown?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
