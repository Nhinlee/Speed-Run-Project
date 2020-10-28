using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoyMovement : MonoBehaviour
{
    [Header("Component Ref")]
    [SerializeField]
    private SpeedBoyInput myInput;
    [SerializeField]
    private Rigidbody2D myRigid;
    [SerializeField]
    private BoxCollider2D myCollider;

    [Header("Jump Field")]
    [SerializeField]
    private float jumpForce = 6.5f;
    [SerializeField]
    private float jumpForceHoldingBonus = 0.5f;
    [SerializeField]
    private float jumpForceHoldingInterval = 0.2f;
    float jumpTime;

    [Header("Horizontal Field")]
    [SerializeField]
    private float runSpeed;

    [Header("Check On Ground")]
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float maxDistanceCheckOnGround;
    [SerializeField]
    private float maxDistanceCheckTouchWallRight;
    [SerializeField]
    private float maxDistanceCheckTouchWallLeft;
    [SerializeField]
    private float isFacingRightDirection = 1;

    private bool isStartJumpHolding;
    private bool isOnGround = false;
    private bool isJumping = false;
    private static bool isDrawRayCast = true;

    private void Start()
    {
        InitValue();
    }

    private void InitValue()
    {
        maxDistanceCheckTouchWallRight = maxDistanceCheckTouchWallLeft = myCollider.size.x / 2 + 0.1f;
    }

    private void Update()
    {
        CheckOnGround();
        CheckTouchWall();
        AutoRun();
        MidAirMove();
    }

    private void CheckTouchWall()
    {
        var rightHit = RayCast(transform.position, Vector2.right, maxDistanceCheckTouchWallRight, groundMask);
        var leftHit = RayCast(transform.position, Vector2.left, maxDistanceCheckTouchWallLeft, groundMask);
        if(rightHit)
        {
            isFacingRightDirection = -1;
        }
        if (leftHit)
        {
            isFacingRightDirection = 1;
        }
    }

    private void MidAirMove()
    {
        if (myInput.IsJumpPressed && isOnGround && !isJumping)
        {
            isJumping = true;

            // Get Jump Time To Add Jump Force holding later
            jumpTime = Time.time + jumpForceHoldingInterval;
            // Add Jump force
            myRigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        
        if(myInput.IsJumpHolding && Time.time <= jumpTime && !isOnGround) 
        {
            // Add Jump force holding
            myRigid.AddForce(Vector2.up * jumpForceHoldingBonus, ForceMode2D.Impulse);
        }
        
        if(Time.time > jumpTime)
        {
            isJumping = false;
        }
    }

    private void AutoRun()
    {
        var newVelocity = myRigid.velocity;
        newVelocity.x = runSpeed * isFacingRightDirection;
        myRigid.velocity = newVelocity;
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
