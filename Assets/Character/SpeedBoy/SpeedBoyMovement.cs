﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float forceJump = 10f;
    [SerializeField]
    private float forceJumpHoldingBonus = 0.45f;
    [SerializeField]
    private float intervalJumpHolding = 0.2f;
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
    private float forceJumpBonusWhenTouchingWall = 1f;

    [Header("Punch And Dash")]
    [SerializeField]
    private float maxDistancePunchAndDash = 2f;
    [SerializeField]
    private float speedDashing = 10f;
    // TODO: This so confused
    [SerializeField]
    private float forceYBonusWhenDashing = 0.6f;
    private Coroutine coroutinePunchAndDash = null;

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
    private bool isPunchingAndDashing = false;
    private int punchingAndDashingCount = 0;
    private float heigtEps = 0.02f; // This height for adjust check bottom hit between player vs wall

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
        ResetDashingCount();
        //Debug.Log($"Running: {speedBoyState.Running}");
        //Debug.Log($"GroundJumping: {speedBoyState.GroundJumping}");
        //Debug.Log($"WallSlice: {speedBoyState.WallSlice}");
        //Debug.Log($"WallJumping: {speedBoyState.WallJumping}");
        //Debug.Log($"Punching: {speedBoyState.Punching}");

        // Set state of SpeedBoyState Instance
        SetSpeedBoyState();

    }

    private void ResetDashingCount()
    {
        if (speedBoyState.Running || speedBoyState.WallSlice)
        {
            punchingAndDashingCount = 0;
        }
    }

    private void SpecialMove()
    {
        // Transition from Ground Jumping to WallSlice 
        //(Đang nhảy mà chạm tường thì sẽ cộng thêm 1 lực nhất định -> character trượt lên một đoạn nhỏ)
        if(!isOnGround && speedBoyState.GroundJumping && isTouchingWall)
        {
            var oldYVelocity = myRigid.velocity.y;

            myRigid.velocity = new Vector2(myRigid.velocity.x, 0f);

            if(oldYVelocity > 0)
            {
                myRigid.AddForce(
                Vector2.up * forceJumpBonusWhenTouchingWall * (float)(Math.Sqrt(myRigid.gravityScale)),
                ForceMode2D.Impulse);
            }
        }
        // Transition from Walljumping to WallSlice (Chạm tường)
        if((speedBoyState.WallJumping || speedBoyState.Punching) && isTouchingWall)
        {
            myRigid.velocity = new Vector2(myRigid.velocity.x, 0f);
        }
        
    }

    private void SetSpeedBoyState()
    {
        if(isTouchingWall)
        {
            speedBoyState.SetState(State.WALL_SLICE);
        }
        if(!isTouchingWall && isWallJumping)
        {
            speedBoyState.SetState(State.WALL_JUMPING);
        }
        if(isGroundJumping)
        {
            speedBoyState.SetState(State.GROUND_JUMPING);
        }
        if (!isTouchingWall && isPunchingAndDashing)
        {
            speedBoyState.SetState(State.PUNCHING);
        }
        if(isOnGround && !isTouchingWall)
        {
            speedBoyState.SetState(State.RUNNING);
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

    public void OnPunch()
    {
        if(!isOnGround 
            && !isTouchingWall 
            && punchingAndDashingCount < 1 
            && !isPunchingAndDashing)
        {
            isPunchingAndDashing = true;
            punchingAndDashingCount++;
            if(coroutinePunchAndDash != null)
                StopCoroutine(coroutinePunchAndDash);
            coroutinePunchAndDash = StartCoroutine(CoroutinePunchAndDash());
        }
    }

    private IEnumerator CoroutinePunchAndDash()
    {
        Vector2 startPosition = transform.position;
        while(
            Mathf.Abs(startPosition.x - transform.position.x) < maxDistancePunchAndDash
            && !isTouchingWall)
        {
            myRigid.velocity = new Vector2(speedDashing * IsFacingRightDirection, forceYBonusWhenDashing);
            yield return null;
        }
        myRigid.velocity = new Vector2(0f, 0f);
        isPunchingAndDashing = false;
    }
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
            CheckAndFlipCharacter();
            // Set flag is Jumping to control time holding jump button
            isWallJumping = true;
            IsMidAir = true;
            // Get Jump Time To Add Jump Force holding later
            jumpTime = Time.time + intervalJumpHolding;
            // Reset Y - Velocity to persist gravity
            myRigid.velocity = new Vector2(0f, 0f);
            // Add Jump force
            myRigid.AddForce(
                Vector2.up * forceJump * (float)(Math.Sqrt(myRigid.gravityScale)) * 1.5f,
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
            jumpTime = Time.time + intervalJumpHolding;
            // Add Jump force
            myRigid.AddForce(
                Vector2.up * forceJump * (float)(Math.Sqrt(myRigid.gravityScale)),
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
            jumpTime = Time.time + intervalJumpHolding;
            // Add Jump force
            myRigid.AddForce(
                Vector2.up * forceJump * (float)(Math.Sqrt(myRigid.gravityScale)),
                ForceMode2D.Impulse);
        }
    }
    private void Jumping()
    {
        if (myCustomInput.IsJumpHolding && Time.time <= jumpTime && (isGroundJumping || isWallJumping || isWallSlice))
        {
            // Add Jump force holding
            myRigid.AddForce(
                Vector2.up * forceJumpHoldingBonus * (float)(Math.Sqrt(myRigid.gravityScale)),
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
        myRigid.velocity = new Vector2(
            Mathf.Max(Mathf.Abs(runSpeed) , Mathf.Abs(myRigid.velocity.x)) * IsFacingRightDirection, 
            myRigid.velocity.y);
    }
    private void CheckAndFlipCharacter()
    {
        Vector2 newScale = transform.localScale;
        if(IsFacingRightDirection * newScale.x < 1)
        {
            newScale.x *= -1;
        }
        transform.localScale = newScale;
    }
    private void StopRun()
    {
        myRigid.velocity = new Vector2(0f, myRigid.velocity.y);
    }
    private void CheckTouchWall()
    {
        // Check Middle
        var rightHit = RayCast(transform.position, Vector2.right, maxDistanceCheckTouchWallRight, groundMask);
        var leftHit = RayCast(transform.position, Vector2.left, maxDistanceCheckTouchWallLeft, groundMask);

        // Check Top
        var topRightHit = RayCast(
            (Vector2)transform.position + new Vector2(0, myCollider.size.y / 2), 
            Vector2.right, 
            maxDistanceCheckTouchWallRight, groundMask);
        var topLeftHit = RayCast(
            (Vector2)transform.position + new Vector2(0, myCollider.size.y / 2),
            Vector2.left,
            maxDistanceCheckTouchWallRight, groundMask);

        // Check Bottom
        var bottomRightHit = RayCast(
            (Vector2)transform.position - new Vector2(0, myCollider.size.y / 2 - heigtEps),
            Vector2.right,
            maxDistanceCheckTouchWallRight, groundMask);
        var bottomLeftHit = RayCast(
            (Vector2)transform.position - new Vector2(0, myCollider.size.y / 2 - heigtEps),
            Vector2.left,
            maxDistanceCheckTouchWallRight, groundMask);

        isTouchingWall = rightHit || leftHit || topLeftHit || topRightHit || bottomLeftHit || bottomRightHit;

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
        originPos.y -= (myCollider.size.y / 2 - heigtEps);
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
    RUNNING,
    GROUND_JUMPING,
    WALL_SLICE,
    WALL_JUMPING,
    PUNCHING
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
    public bool Running { get; private set; }
    public bool GroundJumping { get; private set; }
    public bool WallSlice { get; private set; }
    public bool WallJumping { get; private set; }
    public bool Punching { get; private set; }

    // Methods
    public void SetState(State state)
    {
        ResetState();
        switch (state)
        {
            case State.RUNNING: Running = true; break;
            case State.GROUND_JUMPING: GroundJumping = true; break;
            case State.WALL_SLICE: WallSlice = true; break;
            case State.WALL_JUMPING: WallJumping = true; break;
            case State.PUNCHING: Punching = true; break;
        }
    }

    private void ResetState()
    {
        Running = false;
        GroundJumping = false;
        WallSlice = false;
        WallJumping = false;
        Punching = false;
    }
}

