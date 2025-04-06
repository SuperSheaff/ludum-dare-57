using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerControlState
{

    private int xInput;
    private bool isGrounded;
    private bool jumpInput;
    private bool JumpInputHold;
    private bool coyoteTime;

    public PlayerInAirState(PlayerController player, GameSettings settings) : base(player, settings)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded      = core.CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        player.Animator.SetBool("jump", true);
    }

    public override void Exit()
    {
        base.Exit();

        player.Animator.SetBool("jump", false);
    }

    public override void Update()
    {
        base.Update();

        CheckCoyoteTime();

        if (player.playerRigidBody.linearVelocity.y < 0) {
            player.playerRigidBody.linearVelocity += Vector2.up * Physics2D.gravity.y * (settings.fallMultiplier - 1) * Time.deltaTime;
        } else if (player.playerRigidBody.linearVelocity.y > 0 && !JumpInputHold) {
            player.playerRigidBody.linearVelocity += Vector2.up * Physics2D.gravity.y * (settings.lowJumpMultiplier - 1) * Time.deltaTime;
        }

        xInput          = player.InputHandler.NormInputX;
        jumpInput       = player.InputHandler.JumpInput;
        JumpInputHold   = player.InputHandler.JumpInputHold;

        if (isGrounded && core.Movement.CurrentVelocity.y < 0.01f) 
        {
            player.StateMachine.ChangeState(player.LandState);
        } 
        else if (jumpInput && player.JumpState.CanJump())
        {
            player.StateMachine.ChangeState(player.JumpState);
        } 
        else {
            core.Movement.CheckIfShouldFlip(xInput);
            core.Movement.SmoothDampVelocityX(settings.movementSpeed * xInput, settings.movementSmoothing);

            player.Animator.SetFloat("yVelocity", core.Movement.CurrentVelocity.y);
        }

        if (player.playerRigidBody.linearVelocity.y < 0) {
            player.playerRigidBody.linearVelocity += Vector2.up * Physics2D.gravity.y * (settings.fallMultiplier - 1) * Time.deltaTime;
        } else if (player.playerRigidBody.linearVelocity.y > 0 && !JumpInputHold) {
            player.playerRigidBody.linearVelocity += Vector2.up * Physics2D.gravity.y * (settings.lowJumpMultiplier - 1) * Time.deltaTime;
        }
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void CheckCoyoteTime() {
        if (coyoteTime && (Time.time > startTime + settings.coyoteTime)) {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

}
