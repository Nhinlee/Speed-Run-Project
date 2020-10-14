using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoyMovement : MonoBehaviour
{
    // Component Ref
    [SerializeField]
    private SpeedBoyInput mySpeedBoyInput;
    [SerializeField]
    private Rigidbody2D myRigid;
    [SerializeField]
    private BoxCollider2D myCollider;

    // Private Field
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float jumpForceHoldingBonus = 0.2f;

    [SerializeField]
    private LayerMask groundMask;
    private bool isOnGround = true;
    [SerializeField]
    private float maxDistanceCheckOnGround;



    private static bool isDrawRayCast = true;

    // Start is called before the first frame update
    void Start()
    {
        registerInputEvent();
    }
    private void OnDestroy()
    {
        unRegisterInputEvent();
    }
    private void Update()
    {
        CheckOnGround();
    }

    private void registerInputEvent()
    {
        mySpeedBoyInput.OnJumpPressed += OnJumpPressed;
        mySpeedBoyInput.OnJumpHolding += OnJumpHolding;
    }

    private void unRegisterInputEvent()
    {
        mySpeedBoyInput.OnJumpPressed -= OnJumpPressed;
        mySpeedBoyInput.OnJumpHolding -= OnJumpHolding;
    }

    private void OnJumpHolding()
    {
        myRigid.AddForce(Vector2.up * jumpForceHoldingBonus, ForceMode2D.Impulse);
    }

    private void OnJumpPressed()
    {
        if(isOnGround)
        {
            myRigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void CheckOnGround()
    {
        var originPos = transform.position;
        originPos.y -= myCollider.size.y / 2;
        var groundHit = RayCast(originPos, Vector2.down, maxDistanceCheckOnGround, groundMask);
        isOnGround = groundHit;
    }

    private RaycastHit2D RayCast(Vector2 pos, Vector2 direction, float length, LayerMask mask)
    {
        // Raycast
        RaycastHit2D hit = Physics2D.Raycast(pos, direction, length, mask);
        // Draw ray to debug
        if (isDrawRayCast)
        {
            //...determine the color based on if the raycast hit...
            Color color = hit ? Color.red : Color.green;
            //draw ray in screen...
            Debug.DrawRay(pos, length * direction, color, 0f, false);
        }

        return hit;
    }

}
