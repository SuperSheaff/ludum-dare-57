using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region State Variables

    public StateMachine<PlayerController> StateMachine;
    public static PlayerController instance             { get; private set; }

    public PlayerStartState         StartState          { get; private set; }
    public PlayerRespawnState       RespawnState        { get; private set; }
    public PlayerDeathState         DeathState          { get; private set; }
    public PlayerIdleState          IdleState           { get; private set; }
    public PlayerMoveState          MoveState           { get; private set; }
    public PlayerJumpState          JumpState           { get; private set; }
    public PlayerInAirState         InAirState          { get; private set; }
    public PlayerLandState          LandState           { get; private set; }
    public PlayerTeleportState      TeleportState       { get; private set; }

    [SerializeField]
    public GameSettings Settings;

    [SerializeField]
    public Transform TeleportMarker     { get; set; }
    public bool IsTeleportMarkerOut     { get; set; }
    public bool HasTeleportedInAir      { get; set; }
    public float TeleportTime;

    #endregion

    #region Components

    public Core                 Core                { get; private set; }
    public Animator             Animator            { get; private set; }
    public PlayerInputHandler   InputHandler        { get; private set; }
    public Rigidbody2D          playerRigidBody     { get; private set; }
    public BoxCollider2D        playerBoxCollider   { get; private set; }

    #endregion

    #region Spawning Transforms

    [SerializeField]
    public Transform StartingSpawn;

    [SerializeField]
    public Transform RespawnPoint;

    #endregion

    #region Velocity Variables

    private Vector2             workspace;
    private Vector2             referenceVelocity;

    #endregion

    #region Unity Callback Functions

        private void Awake() 
        {

            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;

            Core = GetComponentInChildren<Core>();

            StateMachine        = new StateMachine<PlayerController>(Settings.debugMode);

            DeathState          = new PlayerDeathState(this, Settings);
            RespawnState        = new PlayerRespawnState(this, Settings);
            StartState          = new PlayerStartState(this, Settings);
            IdleState           = new PlayerIdleState(this, Settings);
            MoveState           = new PlayerMoveState(this, Settings);
            JumpState           = new PlayerJumpState(this, Settings);
            InAirState          = new PlayerInAirState(this, Settings);
            LandState           = new PlayerLandState(this, Settings);
            TeleportState       = new PlayerTeleportState(this, Settings);

            IsTeleportMarkerOut = false;
            HasTeleportedInAir = false;

            TeleportTime        = Time.time;
        }

        private void Start() {
            Animator            = GetComponent<Animator>();
            InputHandler        = GetComponent<PlayerInputHandler>();
            playerRigidBody     = GetComponent<Rigidbody2D>();
            playerBoxCollider   = GetComponent<BoxCollider2D>();

            referenceVelocity       = Vector2.zero;
            RespawnPoint.position   = StartingSpawn.position;
            
            StateMachine.Initialize(StartState);
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
            if (collision.CompareTag("Obstacle"))
            {
                StateMachine.ChangeState(RespawnState);
            }
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

        public void ResetTeleportMarker()
        {
            if (TeleportMarker != null)
            {
                Destroy(TeleportMarker.gameObject);
                TeleportMarker = null;
            }

            IsTeleportMarkerOut = false;
            HasTeleportedInAir = false;
            TeleportTime = Time.time;
        }

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

    // Triggers an animation event
    public void AnimationEvent(string eventName)
    {
        StateMachine.CurrentState.OnAnimationEvent(eventName);
    }

    #endregion

}
