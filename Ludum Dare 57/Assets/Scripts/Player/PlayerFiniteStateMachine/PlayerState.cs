using UnityEngine;

public class PlayerState : State<PlayerController>
{
    protected Core core;
    protected PlayerController player;
    protected GameSettings settings;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    public PlayerState(PlayerController player, GameSettings settings) : base(player)
    {
        this.player     = player;
        this.settings   = settings;
        this.core       = player.Core;
    }

    #region State Flow

    public override void Enter()
    {
        DoChecks();
        startTime               = Time.time;
        isAnimationFinished     = false;
        isExitingState          = false;
    }

    public override void Exit()
    {
        isExitingState = true;
    }

    public override void Update() { }

    public override void FixedUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }

    #endregion

    #region Animation Events

    public override void OnAnimationEvent(string eventName) { }

    #endregion
}
