using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{

    private int amountOfJumpsLeft;

    public PlayerJumpState(PlayerController player, GameSettings settings) : base(player, settings)
    {
        amountOfJumpsLeft = 1;
    }

    public override void Enter()
    {
        base.Enter();

        SoundManager.instance.PlaySound("jump", player.transform);
        player.InputHandler.UseJumpInput();
        core.Movement.SetVelocityY(settings.jumpForce);
        isAbilityDone = true;
        amountOfJumpsLeft--;
    }

    public bool CanJump() 
    {
        if (player.HasTeleportedInAir)
        {
            return false;
        }
        else 
        {
            return amountOfJumpsLeft > 0;
        }
    }

    public void ResetAmountOfJumpsLeft() 
    {
        amountOfJumpsLeft = 1;
    }

    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}
