using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region State Variables

    public StateMachine<PlayerController> StateMachine;

    public PlayerRespawnState       RespawnState        { get; private set; }
    public PlayerDeathState         DeathState          { get; private set; }
    public PlayerIdleState          IdleState           { get; private set; }
    public PlayerMoveState          MoveState           { get; private set; }
    public PlayerJumpState          JumpState           { get; private set; }
    public PlayerInAirState         InAirState          { get; private set; }
    public PlayerLandState          LandState           { get; private set; }

    [SerializeField]
    public GameSettings Settings;

    // [SerializeField]
    // public GameController GameController;

    // [SerializeField]
    // public CameraController CameraController;

    #endregion

    #region Components

    [SerializeField]

    public Core                 Core                { get; private set; }
    public Animator             Animator      { get; private set; }
    public PlayerInputHandler   InputHandler        { get; private set; }
    public Rigidbody2D          playerRigidBody     { get; private set; }
    public BoxCollider2D        playerBoxCollider   { get; private set; }
    
    // public PlayerAudioManager   playerAudioManager  { get; private set; }

    // public ParticleSystem       playerRunParticles;
    // public ParticleSystem       playerLandParticles;
    // public ParticleSystem       playerWallLandParticles;
    // public ParticleSystem       playerJumpParticles;
    // public ParticleSystem       playerGroundSlideParticles;

    // public float                HealthPoints;
    // public GameObject           HealthBar;

    #endregion

    #region Spawning Transforms

    [SerializeField]
    public Transform StartingSpawn;

    [SerializeField]
    public Transform RespawnPoint;

    #endregion

    #region Other Variables

    private Vector2             workspace;
    private Vector2             referenceVelocity;

    // private bool                canPowerUp;
    // private bool                isPoweredUp;
    // private bool                isSafe;
    // private bool                isSizzling;

    // public bool                IsPlayable;

    #endregion

    #region Unity Callback Functions

        private void Awake() {

            Core = GetComponentInChildren<Core>();

            StateMachine        = new StateMachine<PlayerController>(Settings.debugMode);

            DeathState          = new PlayerDeathState(this, Settings);
            RespawnState        = new PlayerRespawnState(this, Settings);
            IdleState           = new PlayerIdleState(this, Settings);
            MoveState           = new PlayerMoveState(this, Settings);
            JumpState           = new PlayerJumpState(this, Settings);
            InAirState          = new PlayerInAirState(this, Settings);
            LandState           = new PlayerLandState(this, Settings);

        }

        private void Start() {
            Animator      = GetComponent<Animator>();
            InputHandler        = GetComponent<PlayerInputHandler>();
            playerRigidBody     = GetComponent<Rigidbody2D>();
            playerBoxCollider   = GetComponent<BoxCollider2D>();

            referenceVelocity       = Vector2.zero;
            RespawnPoint.position   = StartingSpawn.position;
            
            StateMachine.Initialize(RespawnState);
        }

        private void Update() {
            Core.Update();
            StateMachine.CurrentState.Update();   
        }

        private void FixedUpdate() 
        {
            StateMachine.CurrentState.FixedUpdate();    
        }

        private void OnTriggerEnter2D(Collider2D collision) 
        {
            // if (collision.tag == "SafeZone") 
            // {
            //     isSafe = true;
            // }

            // if (collision.tag == "ChargeZone") 
            // {
            //     canPowerUp = true;
            // }
        }

        private void OnTriggerStay2D(Collider2D collision) 
        {
            // if (collision.tag == "SafeZone") 
            // {
            //     isSafe = true;
            // }

            // if (collision.tag == "ChargeZone") 
            // {
            //     canPowerUp = true;
            // }
        }

        private void OnTriggerExit2D(Collider2D collision) 
        {
            // if (collision.tag == "SafeZone") 
            // {
            //     isSafe = false;
            // }

            // if (collision.tag == "ChargeZone") 
            // {
            //     canPowerUp = false;
            // }
        }

    #endregion

    #region Set Functions

        public void SetColliderHeight(float height, float offset) 
        {
            Vector2 center = playerBoxCollider.offset;
            workspace.Set(playerBoxCollider.size.x, height);
            
            center.y += offset;

            playerBoxCollider.size = workspace;
            playerBoxCollider.offset = center;
        }

        public void SetSpawnPoint(Vector3 newPosition) 
        {
            RespawnPoint.position   = newPosition;
        }

        // public void SetIsSafe(bool value) 
        // {
        //     isSafe = value;
        // }

        // public void SetIsPoweredUp(bool value) 
        // {
        //     isPoweredUp = value;

        //     if (value)
        //     {
        //         Animator.SetBool("isPowered", true);
        //     } else {
        //         Animator.SetBool("isPowered", false);
        //     }
        // }

    #endregion

    #region Get Functions

        // public bool GetIsPoweredUp() 
        // {
        //     return isPoweredUp;
        // }

        // public bool GetCanPowerUp() 
        // {
        //     return canPowerUp;
        // }

    #endregion

    #region Trigger Functions

        // private void AnimationTrigger()                 => StateMachine.CurrentState.AnimationTrigger();
        // private void AnimationFinishedTrigger()         => StateMachine.CurrentState.AnimationFinishedTrigger();
        // private void AnimationStartMovementTrigger()    => StateMachine.CurrentState.AnimationStartMovementTrigger();
        // private void AnimationStopMovementTrigger()     => StateMachine.CurrentState.AnimationStopMovementTrigger();
        // private void AnimationTurnOffFlip()             => StateMachine.CurrentState.AnimationTurnOffFlip();
        // private void AnimationTurnOnFlip()              => StateMachine.CurrentState.AnimationTurnOnFlip();
        // private void AnimationActionTrigger()           => StateMachine.CurrentState.AnimationActionTrigger();

    #endregion
    
    
    #region Other Functions

    public void ResetGame() {
        Core.Movement.CheckIfShouldFlip(1);
        SetSpawnPoint(StartingSpawn.position);
        StateMachine.ChangeState(RespawnState);
    }

    public void CameraShake() {
        // StartCoroutine(CameraController.ShakeTheCamera(1f, 0.5f));
    }

    #endregion

}
