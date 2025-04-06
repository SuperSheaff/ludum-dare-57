using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private Vector3             referenceVelocity;

    public PlayerMoveState(PlayerController player, GameSettings settings) : base(player, settings)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        referenceVelocity = Vector3.zero;

        player.Animator.SetBool("move", true);
    }

    public override void Exit()
    {
        base.Exit();

        player.Animator.SetBool("move", false);
    }

    public override void Update()
    {
        base.Update();

        core.Movement.SmoothDampVelocityX(settings.movementSpeed * xInput, settings.movementSmoothing);

        if (!isExitingState) {
            if (xInput == 0) {
                player.StateMachine.ChangeState(player.IdleState);
            } 
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    
    // Triggers an animation event
    public override void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "step":
                SoundManager.instance.PlaySound("step", player.transform);
                break;
        }
    }

}
