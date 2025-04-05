using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D rb { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }
    public bool CanSetVelocity { get; set; }

    private Vector2 _referenceVelocity;
    private Vector2 _workspace;

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponentInParent<Rigidbody2D>();

        _workspace = Vector2.zero;
        FacingDirection = 1;
        CanSetVelocity = true;
    }

    public void Update()
    {
        CurrentVelocity = rb.linearVelocity;
    }

    #endregion

    #region Velocity Setters

    public void SetVelocityZero()
    {
        _workspace = Vector2.zero;
        SetFinalVelocity();
    }

    public void SetVelocity(float xVelocity, float yVelocity, int direction)
    {
        _workspace.Set(xVelocity * direction, yVelocity);
        SetFinalVelocity();
    }

    public void SetVelocityX(float velocity)
    {
        _workspace.Set(velocity, CurrentVelocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        _workspace.Set(CurrentVelocity.x, velocity);
        SetFinalVelocity();
    }

    public void SmoothDampVelocityX(float velocity, float smoothing)
    {
        _workspace.Set(velocity, CurrentVelocity.y);
        SetFinalSmoothDampVelocity(smoothing);
    }

    public void SmoothDampVelocityY(float velocity, float smoothing)
    {
        _workspace.Set(CurrentVelocity.x, velocity);
        SetFinalSmoothDampVelocity(smoothing);
    }

    private void SetFinalVelocity()
    {
        if (CanSetVelocity)
        {
            rb.linearVelocity = _workspace;
            CurrentVelocity = _workspace;
        }
    }

    private void SetFinalSmoothDampVelocity(float smoothing)
    {
        if (CanSetVelocity)
        {
            rb.linearVelocity = Vector2.SmoothDamp(CurrentVelocity, _workspace, ref _referenceVelocity, smoothing);
            CurrentVelocity = _workspace;
        }
    }

    #endregion

    #region Facing / Flip

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;
        rb.transform.Rotate(0f, 180f, 0f);
    }

    #endregion
}
