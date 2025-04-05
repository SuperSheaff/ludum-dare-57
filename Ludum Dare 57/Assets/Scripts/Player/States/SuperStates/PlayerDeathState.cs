using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{

    private bool deathTime;

    public PlayerDeathState(PlayerController player, GameSettings settings) : base(player, settings)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (!deathTime) {
            StartDeathTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
        player.CameraShake();
        core.Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        CheckDeathTime();

        if (!deathTime) {
            player.StateMachine.ChangeState(player.RespawnState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void CheckDeathTime() {
        if (deathTime && (Time.time > startTime + settings.deathTime)) {
            deathTime = false;
        }
    }

    public void StartDeathTime() => deathTime = true;

}
