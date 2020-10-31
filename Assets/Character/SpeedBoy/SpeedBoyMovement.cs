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

    [Header("Touching Wall")]
    [SerializeField]
    private float gravityScaleTouchingWall = 0.1f;

    public int IsFacingRightDirection { get; private set; }
    public bool IsMidAir { get; private set; }

    private bool isDrawRayCast = true;
    private bool isOnGround = false;
    private bool isGroundJumping = false;
    private bool isTouchingWall = false;
    private bool isWallJumping = false;

    private void Start()
    {
        InitValue();
    }

    private void InitValue()
    {
        maxDistanceCheckTouchWallRight = maxDistanceCheckTouchWallLeft = myCollider.size.x / 2 + 0.15f;
        IsFacingRightDirection = 1;
    }

    private void Update()
    {
        // Check Condition
        CheckOnGround();
        CheckTouchWall();
       
        // Special Move
        WallSlice();
        GroundJump();
        WallJump();

        // Run
        Run();
    }

    private void WallJump()
    {
        if(myInput.IsJumpPressed && isTouchingWall && !isOnGround && !isWallJumping)
        {
            // Change Run Direction
            IsFacingRightDirection *= -1;
            // Set flag is Jumping to control time holding jump button
            isWallJumping = true;
            // Get Jump Time To Add Jump Force holding later
            jumpTime = Time.time + jumpForceHoldingInterval;
            // Reset Y - Velocity to persist gravity
            myRigid.velocity = new Vector2(0f, 0f);
            // Add Jump force
            myRigid.AddForce(
                Vector2.up * jumpForce * (float)(Math.Sqrt(myRigid.gravityScale)) * 1.5f,
                ForceMode2D.Impulse);
        }

        if (myInput.IsJumpHolding && Time.time <= jumpTime && isWallJumping)
        {
            // Add Jump force holding
            myRigid.AddForce(
                Vector2.up * jumpForceHoldingBonus * (float)(Math.Sqrt(myRigid.gravityScale)),
                ForceMode2D.Impulse);
        }

        if (Time.time > jumpTime)
        {
            isWallJumping = false;
        }
    }
    private void WallSlice()
    {
        if (isTouchingWall) 
        {
            myRigid.gravityScale = gravityScaleTouchingWall;
        }
        else
        {
            myRigid.gravityScale = 1f;
        }
    }
    private void GroundJump()
    {
        if(myInput.IsJumpPressed)
        {
            if(isOnGround && !isGroundJumping)
            {
                // Set flag is Jumping to control time holding jump button
                isGroundJumping = true;
                IsMidAir = true;
                // Get Jump Time To Add Jump Force holding later
                jumpTime = Time.time + jumpForceHoldingInterval;
                // Add Jump force
                myRigid.AddForce(
                    Vector2.up * jumpForce * (float)(Math.Sqrt(myRigid.gravityScale)),
                    ForceMode2D.Impulse);
            }
        }
        if(myInput.IsJumpHolding && Time.time <= jumpTime && isGroundJumping) 
        {
            // Add Jump force holding
            myRigid.AddForce(
                Vector2.up * jumpForceHoldingBonus * (float)(Math.Sqrt(myRigid.gravityScale)), 
                ForceMode2D.Impulse);
        }
        
        if(Time.time > jumpTime)
        {
            isGroundJumping = false;
        }
    }
    private void Run()
    {
        if (isTouchingWall && !isWallJumping)
        {
            StopRun();
        }
        else
        {
            AutoRun();
        }
    }
    private void AutoRun()
    {
        var newVelocity = myRigid.velocity;
        newVelocity.x = runSpeed * IsFacingRightDirection;
        myRigid.velocity = newVelocity;
    }
    private void StopRun()
    {
        var newVelocity = myRigid.velocity;
        newVelocity.x = 0f;
        myRigid.velocity = newVelocity;
    }
    private void CheckTouchWall()
    {
        var rightHit = RayCast(transform.position, Vector2.right, maxDistanceCheckTouchWallRight, groundMask);
        var leftHit = RayCast(transform.position, Vector2.left, maxDistanceCheckTouchWallLeft, groundMask);
       
        isTouchingWall = rightHit || leftHit;
    }
    private void CheckOnGround()
    {
        var originPos = transform.position;
        originPos.y -= myCollider.size.y / 2;
        var groundHit = RayCast(originPos, Vector2.down, maxDistanceCheckOnGround, groundMask);
        isOnGround = groundHit;
        if(isOnGround)
        {
            IsMidAir = false;
        }    
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

