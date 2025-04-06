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

        SoundManager.instance.StopSound("throw");
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
        player.HasTeleportedInAir = true; 
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
                player.transform.position           = safePosition;
                player.WallGrabPosition             = safePosition;

                if (marker.ShouldGoToWallGrab(out int wallDir))
                {
                    core.Movement.CheckIfShouldFlip(-wallDir);
                    player.StateMachine.ChangeState(player.WallGrabState); 
                    GameObject.Destroy(player.TeleportMarker.gameObject);
                    player.TeleportMarker = null;
                    player.IsTeleportMarkerOut = false;
                    player.TeleportTime = Time.time;
                    return;
                }
            }

            GameObject.Destroy(player.TeleportMarker.gameObject);
            player.TeleportMarker = null;
            player.IsTeleportMarkerOut = false;
            player.TeleportTime = Time.time;

        }

        player.InputHandler.UseInteractInput();
        player.StateMachine.ChangeState(player.InAirState); 
        isAbilityDone = true;
    }


}
