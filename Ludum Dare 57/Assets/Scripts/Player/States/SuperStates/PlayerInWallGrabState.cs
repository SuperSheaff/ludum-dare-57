using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerState
{

    private int xInput;
    private int yInput;
    private bool jumpInput;

    public PlayerWallGrabState(PlayerController player, GameSettings settings) : base(player, settings)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        player.Animator.SetBool("wallGrab", true);
    }

    public override void Exit()
    {
        base.Exit();

        player.Animator.SetBool("wallGrab", false);
    }

    public override void Update()
    {
        base.Update();

        player.transform.position = player.WallGrabPosition;

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;

        // Case 1: Wall Jump
        if (jumpInput)
        {
            player.StateMachine.ChangeState(player.JumpState);
            player.ResetTeleportMarker();
            return;
        }

        // Case 2: Drop from wall (down)
        if (yInput == -1)
        {
            player.StateMachine.ChangeState(player.InAirState);
            player.ResetTeleportMarker(); 
            return;
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        core.Movement.SetVelocityZero();
    }

}
