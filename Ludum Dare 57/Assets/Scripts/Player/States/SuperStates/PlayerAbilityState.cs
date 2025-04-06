using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerControlState
{
    protected int yInput;
    
    protected bool isAbilityDone;
    private bool isGrounded;

    public PlayerAbilityState(PlayerController player, GameSettings settings) : base(player, settings)
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

        isAbilityDone = false;
    }


    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        yInput = player.InputHandler.NormInputY;

        if (isAbilityDone) {
            if (isGrounded && core.Movement.CurrentVelocity.y < 0.01f) {
                player.StateMachine.ChangeState(player.IdleState);
            } else {
                player.StateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
