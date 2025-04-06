using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput     { get; private set; }
    public int NormInputX               { get; private set; }
    public int NormInputY               { get; private set; }
    public bool JumpInput               { get; private set; }
    public bool JumpInputHold           { get; private set; }
    public bool InteractInput           { get; private set; }

    [SerializeField]
    private float inputHoldtime = 0.2f;

    private float jumpInputStartTime;

    private void Awake()
    {
        // Setup input system bindings if needed here
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputHold = true;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputHold = false;
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractInput = true;
        }

        if (context.canceled)
        {
            InteractInput = false;
        }
    }

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldtime)
        {
            JumpInput = false;
        }
    }

    public void UseJumpInput() => JumpInput = false;
    public void UseInteractInput() => InteractInput = false;
}
