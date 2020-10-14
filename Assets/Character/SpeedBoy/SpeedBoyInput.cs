using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoyInput : MonoBehaviour
{
    public Action OnJumpPressed;
    public Action OnJumpHolding;

    private void Update()   
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpPressed?.Invoke();
        }
        else if(Input.GetKey(KeyCode.Space))
        {
            OnJumpHolding?.Invoke();
        }
    }
}
