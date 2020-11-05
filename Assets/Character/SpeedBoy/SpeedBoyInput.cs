using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class SpeedBoyInput : MonoBehaviour
{
    public bool IsJumpHolding { get; private set; }

    private bool isReadyToClear = false;

    private void FixedUpdate()
    {
        // Clear out of all input before and go to Update() and use current input
        isReadyToClear = true;
    }

    private void Update()   
    {
        if (isReadyToClear)
        {
            ResetInput();
        }
        IsJumpHolding = IsJumpHolding || Input.GetButton("Jump");
    }

    private void ResetInput()
    {
        IsJumpHolding = false;

        isReadyToClear = false;
    }
}
