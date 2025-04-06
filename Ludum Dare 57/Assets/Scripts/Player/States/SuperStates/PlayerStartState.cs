using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartState : PlayerState
{
    public PlayerStartState(PlayerController player, GameSettings settings) : base(player, settings)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        // Reset position and flip
        player.transform.position = player.RespawnPoint.position;
        core.Movement.CheckIfShouldFlip(1);

        // Clean up teleport state
        player.ResetTeleportMarker(); 

        // Show intro text
        GameController.instance.ShowText(0);
    }

    public override void Exit()
    {
        base.Exit();

        // Hide intro text
        GameController.instance.HideText(0);
    }

    public override void Update()
    {
        base.Update();

        // Freeze movement
        core.Movement.SetVelocityZero();

        // Wait for interact to start
        if (player.InputHandler.InteractInput)
        {
            player.InputHandler.UseInteractInput();
            player.StateMachine.ChangeState(player.IdleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // Keep the player anchored to the respawn point
        player.transform.position = player.RespawnPoint.position;
    }

}
