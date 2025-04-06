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

        SoundManager.instance.PlaySound("lose", player.transform);

        // Reset position and flip
        player.transform.position = player.RespawnPoint.position;
        core.Movement.CheckIfShouldFlip(1);

        // Clean up teleport state
        player.ResetTeleportMarker(); 

        player.StateMachine.ChangeState(player.IdleState);
    }

    public override void Exit()
    {
        base.Exit();

        // Hide intro text
        GameController.instance.HideText(0);
        player.ResetTeleportMarker(); 
    }

    public override void Update()
    {
        base.Update();

        // Freeze movement
        core.Movement.SetVelocityZero();

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // Keep the player anchored to the respawn point
        player.transform.position = player.RespawnPoint.position;
    }

}
