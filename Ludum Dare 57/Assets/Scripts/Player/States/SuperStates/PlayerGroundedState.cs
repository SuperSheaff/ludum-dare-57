using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerControlState
{

    protected Vector2 input;
    protected int xInput;
    protected int yInput;

    private bool jumpInput;
    private bool chargeInput;
    private bool isGrounded;
    
    public PlayerGroundedState(PlayerController player, GameSettings settings) : base(player, settings)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetAmountOfJumpsLeft();
        player.HasTeleportedInAir = false; 
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        xInput          = player.InputHandler.NormInputX;
        yInput          = player.InputHandler.NormInputY;
        jumpInput       = player.InputHandler.JumpInput;

        core.Movement.CheckIfShouldFlip(xInput);

        if (jumpInput && player.JumpState.CanJump()) {
            player.StateMachine.ChangeState(player.JumpState);
        } else if (!isGrounded) {
            player.InAirState.StartCoyoteTime();
            player.StateMachine.ChangeState(player.InAirState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
