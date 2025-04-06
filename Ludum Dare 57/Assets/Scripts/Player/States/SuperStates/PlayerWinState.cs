using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinState : PlayerState
{

    public PlayerWinState(PlayerController player, GameSettings settings) : base(player, settings)
    {
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        
        // Freeze animator
        SoundManager.instance.PlaySound("win", player.transform);
        player.Animator.speed = 0f; 
    }

    public override void Exit()
    {
        base.Exit();
        player.Animator.speed = 1f; 
    }

    public override void Update()
    {
        base.Update();
        core.Movement.SetVelocityZero();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        core.Movement.SetVelocityZero();
    }


}
