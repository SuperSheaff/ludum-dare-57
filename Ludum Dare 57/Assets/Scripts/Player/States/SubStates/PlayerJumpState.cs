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

        player.InputHandler.UseJumpInput();
        core.Movement.SetVelocityY(settings.jumpForce);
        isAbilityDone = true;
        amountOfJumpsLeft--;
    }

    public bool CanJump() {
        if (amountOfJumpsLeft > 0) 
        {
            return true;
        } else {
            return false;
        }
    }

    public void ResetAmountOfJumpsLeft() 
    {
        amountOfJumpsLeft = 1;
    }

    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}
