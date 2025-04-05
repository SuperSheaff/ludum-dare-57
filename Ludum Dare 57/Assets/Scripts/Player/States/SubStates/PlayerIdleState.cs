using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerController player, GameSettings settings) : base(player, settings)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.Animator.SetBool("idle", true);
    }

    public override void Exit()
    {
        base.Exit();

        player.Animator.SetBool("idle", false);
    }

    public override void Update()
    {
        base.Update();
        
        core.Movement.SmoothDampVelocityX(settings.movementSpeed * xInput, settings.movementSmoothing);

        if (!isExitingState) 
        {
            if (xInput != 0) 
            {
                player.StateMachine.ChangeState(player.MoveState);
            } 
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
