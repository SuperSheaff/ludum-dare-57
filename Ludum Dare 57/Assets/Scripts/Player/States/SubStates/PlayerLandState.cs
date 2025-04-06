using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(PlayerController player, GameSettings settings) : base(player, settings)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        SoundManager.instance.PlaySound("landing", player.transform);
    }

    public override void Exit()
    {
        base.Exit();
        // player.playerLandParticles.Stop();
    }


    public override void Update()
    {
        base.Update();

        if (!isExitingState) {
            if (xInput != 0) {
                player.StateMachine.ChangeState(player.MoveState);
            } else {
                player.StateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
