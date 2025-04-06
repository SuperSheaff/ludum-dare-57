using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlState : PlayerState
{
    protected float nextThrowTime;
    protected float TeleportReadyTime;

    public PlayerControlState(PlayerController player, GameSettings settings) : base(player, settings)
    {
    }

    public override void Update()
    {
        base.Update();

        if (CanTeleport())
        {
            player.StateMachine.ChangeState(player.TeleportState);
        }
        else if (CanThrowTeleportMarker())
        {
            ThrowTeleportMarker();
        }
    }

    protected void ThrowTeleportMarker()
    {
        player.IsTeleportMarkerOut = true;
        player.TeleportTime = Time.time;

        // Set cooldown and delay timers
        nextThrowTime = Time.time + settings.TeleportThrowCooldown;
        TeleportReadyTime = Time.time + settings.TeleportDelay;

        // Create throw direction (45° angle upward in the facing direction)
        Vector2 direction = new Vector2(core.Movement.FacingDirection, 1).normalized;

        // Instantiate and launch marker
        GameObject markerObj = GameObject.Instantiate(settings.TeleportMarkerPrefab, player.transform.position, Quaternion.identity);
        Rigidbody2D rb = markerObj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * settings.TeleportMarkerSpeed;

        // Track the active marker
        player.TeleportMarker = markerObj.transform;

        // Consume interact input
        player.InputHandler.UseInteractInput();
    }

    protected bool CanThrowTeleportMarker()
    {
        return player.HasUnlockedTeleport &&
            player.InputHandler.InteractInput &&
            !player.IsTeleportMarkerOut &&
            Time.time >= nextThrowTime &&
            !player.HasTeleportedInAir;
    }

    protected bool CanTeleport()
    {
        return player.HasUnlockedTeleport &&
            player.InputHandler.InteractInput &&
            player.IsTeleportMarkerOut &&
            Time.time >= TeleportReadyTime;
    }

}
