using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class SpeedBoyInput : MonoBehaviour
{
    public bool IsJumpPressed { get; private set; }
    public bool IsJumpHolding { get; private set; }
        
    private void Update()   
    {
        ResetInput();
        IsJumpPressed = IsJumpPressed || Input.GetKeyDown(KeyCode.Space);
        IsJumpHolding = IsJumpHolding || Input.GetKey(KeyCode.Space);
    }

    private void ResetInput()
    {
        IsJumpHolding = false;
        IsJumpPressed = false;
    }
}
