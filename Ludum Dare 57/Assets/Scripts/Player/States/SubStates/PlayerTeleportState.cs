using UnityEngine;

public class PlayerTeleportState : PlayerAbilityState
{
    public PlayerTeleportState(PlayerController player, GameSettings settings)
        : base(player, settings)
    {
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero(); 

        SoundManager.instance.PlaySound("preteleport", player.transform);
        player.Animator.SetBool("teleport", true);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        core.Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();

        player.Animator.SetBool("teleport", false);
    }


    // Triggers an animation event
    public override void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "teleport":
                teleportToMarker();
                SoundManager.instance.PlaySound("teleport", player.transform);
                break;
        }
    }

    private void teleportToMarker()
    {
        if (player.TeleportMarker != null)
        {
            TeleportMarker marker = player.TeleportMarker.GetComponent<TeleportMarker>();

            if (marker != null)
            {
                Vector2 safePosition = marker.CalculateSafeTeleportPosition();
                player.transform.position = safePosition;

                // If mid-air, block jump/throw
                if (!core.CollisionSenses.Ground)
                {
                    player.HasTeleportedInAir = true;
                }

                Debug.Log("attempt tele");
            }

            GameObject.Destroy(player.TeleportMarker.gameObject);
            player.TeleportMarker = null;
            player.IsTeleportMarkerOut = false;
            player.TeleportTime = Time.time;
        }

        player.InputHandler.UseInteractInput();
        isAbilityDone = true;
    }


}
