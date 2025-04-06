using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGameSettings", menuName = "Data/Game Settings/Base Settings")]

public class GameSettings : ScriptableObject
{
    [Header("Debug Mode")]
    public bool debugMode = true;

    [Header("Move State")]
    public float movementSpeed = 150f;
    public float movementSmoothing = 0.01f;

    [Header("Jump State")]
    public float jumpForce = 305f;
    public float fallMultiplier = 5f;
    public float lowJumpMultiplier = 10f;

    [Header("In Air State")]
    public float coyoteTime = 0.1f;

    [Header("Death State")]
    public float deathTime      = 2f;
    public int startingHealth   = 3;

    [Header("Teleport Settings")]
    public GameObject TeleportMarkerPrefab;
    public float TeleportMarkerSpeed    = 250f;
    public float TeleportThrowCooldown  = 1f;
    public float TeleportDelay          = 0.2f;

}
