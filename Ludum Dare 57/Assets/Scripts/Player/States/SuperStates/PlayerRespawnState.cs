using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnState : PlayerState
{
    public PlayerRespawnState(PlayerController player, GameSettings settings) : base(player, settings)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.transform.position = player.RespawnPoint.position;
        core.Movement.CheckIfShouldFlip(1);

        player.StateMachine.ChangeState(player.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        core.Movement.SetVelocityZero();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
