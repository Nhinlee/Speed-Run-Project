using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpeedBoyMovement : MonoBehaviour
{
    [Header("Component Ref")]
    [SerializeField]
    private SpeedBoyInput myCustomInput;
    [SerializeField]
    private Rigidbody2D myRigid;
    [SerializeField]
    private BoxCollider2D myCollider;

    [Header("Jump Field")]
    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private float jumpForceHoldingBonus = 0.45f;
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
    [SerializeField]
    private float jumpForceBonusWhenTouchingWall = 1f;

    // Property to control animation
    public int IsFacingRightDirection { get; private set; }
    public bool IsMidAir { get; private set; }

    // Private Flag to control behavior of speedboy
    private bool isDrawRayCast = true;
    private bool isOnGround = false;
    private bool isGroundJumping = false;
    private bool isTouchingWall = false;
    private bool isWallJumping = false;
    private SpeedBoyState speedBoyState;
    private bool isWallSlice = false;

    private void Start()
    {
        InitValue();
    }
    private void Update()
    {
        // Check Condition
        CheckOnGround();
        CheckTouchWall();
        
        // Move----------------
        Run();
        //---------------------
        SpecialMove();
        //----------------------
        SetSpeedBoyState();
    }

    private void SpecialMove()
    {
        if(!isOnGround && speedBoyState.GroundJumping && isTouchingWall)
        {
            var oldYVelocity = myRigid.velocity.y;

            myRigid.velocity = new Vector2(myRigid.velocity.x, 0f);

            if(oldYVelocity > 0)
            {
                myRigid.AddForce(
                Vector2.up * jumpForceBonusWhenTouchingWall * (float)(Math.Sqrt(myRigid.gravityScale)),
                ForceMode2D.Impulse);
            }
        }
    }

    private void SetSpeedBoyState()
    {
        if(isTouchingWall)
        {
            speedBoyState.SetState(State.WALL_SLICE);
        }
        if(isTouchingWall && isWallJumping)
        {
            speedBoyState.SetState(State.WALL_JUMPING);
        }
        if(isGroundJumping)
        {
            speedBoyState.SetState(State.GROUND_JUMPING);
        }
    }

    private void FixedUpdate()
    {
        // Jump Holding //
        Jumping();
    }

    // Callback from input system
    public void OnJump()
    {
        GroundJump();
        WallJump();
        WallSlice();
    }

    // Private Methods
    private void InitValue()
    {
        maxDistanceCheckTouchWallRight = maxDistanceCheckTouchWallLeft = myCollider.size.x / 2 + 0.15f;
        IsFacingRightDirection = 1;
        speedBoyState = SpeedBoyState.GetInstance();
    }
    private void WallJump()
    {
        if(isTouchingWall && !isOnGround && !isWallJumping)
        {
            // Change Run Direction
            IsFacingRightDirection *= -1;
            // Set flag is Jumping to control time holding jump button
            isWallJumping = true;
            IsMidAir = true;
            // Get Jump Time To Add Jump Force holding later
            jumpTime = Time.time + jumpForceHoldingInterval;
            // Reset Y - Velocity to persist gravity
            myRigid.velocity = new Vector2(0f, 0f);
            // Add Jump force
            myRigid.AddForce(
                Vector2.up * jumpForce * (float)(Math.Sqrt(myRigid.gravityScale)) * 1.5f,
                ForceMode2D.Impulse);
        }
    }
    private void WallSlice()
    {
        
        if (isOnGround && !isWallSlice && isTouchingWall)
        {
            // Set flag is Jumping to control time holding jump button
            isWallSlice = true;
            IsMidAir = true;
            // Get Jump Time To Add Jump Force holding later
            jumpTime = Time.time + jumpForceHoldingInterval;
            // Add Jump force
            myRigid.AddForce(
                Vector2.up * jumpForce * (float)(Math.Sqrt(myRigid.gravityScale)),
                ForceMode2D.Impulse);
        }
    }
    private void GroundJump()
    {
        if(isOnGround && !isGroundJumping && !isTouchingWall)
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
    private void Jumping()
    {
        if (myCustomInput.IsJumpHolding && Time.time <= jumpTime && (isGroundJumping || isWallJumping || isWallSlice))
        {
            // Add Jump force holding
            myRigid.AddForce(
                Vector2.up * jumpForceHoldingBonus * (float)(Math.Sqrt(myRigid.gravityScale)),
                ForceMode2D.Impulse);
        }

        if (Time.time > jumpTime)
        {
            isGroundJumping = false;
            isWallJumping = false;
            isWallSlice = false;
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
        myRigid.velocity = new Vector2(runSpeed * IsFacingRightDirection, myRigid.velocity.y);
    }
    private void StopRun()
    {
        myRigid.velocity = new Vector2(0f, myRigid.velocity.y);
    }
    private void CheckTouchWall()
    {
        var rightHit = RayCast(transform.position, Vector2.right, maxDistanceCheckTouchWallRight, groundMask);
        var leftHit = RayCast(transform.position, Vector2.left, maxDistanceCheckTouchWallLeft, groundMask);
       
        isTouchingWall = rightHit || leftHit;

        if (isTouchingWall)
        {
            myRigid.gravityScale = gravityScaleTouchingWall;
        }
        else
        {
            myRigid.gravityScale = 1f;
        }
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

enum State
{   
    GROUND_JUMPING,
    WALL_SLICE,
    WALL_JUMPING,
}
class SpeedBoyState
{
    // Singleton
    protected SpeedBoyState() { }

    private static SpeedBoyState instance = null;
    public static SpeedBoyState GetInstance()
    {
        if (instance == null)
            instance = new SpeedBoyState();
        return instance;
    }
    
    // Properties
    /*public bool Running { get; private set; }*/
    public bool GroundJumping { get; private set; }
    public bool WallSlice { get; private set; }
    public bool WallJumping { get; private set; }

    // Methods
    public void SetState(State state)
    {
        ResetState();
        switch (state)
        {
            /*case State.RUN: Running = true; break;*/
            case State.GROUND_JUMPING: GroundJumping = true; break;
            case State.WALL_SLICE: WallSlice = true; break;
            case State.WALL_JUMPING: WallJumping = true; break;
        }
    }

    private void ResetState()
    {
        /*Running = false;*/
        GroundJumping = false;
        WallSlice = false;
        WallJumping = false;
    }
}


