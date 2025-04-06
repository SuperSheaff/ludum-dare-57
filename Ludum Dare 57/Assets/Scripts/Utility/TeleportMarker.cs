using UnityEngine;

public class TeleportMarker : MonoBehaviour
{
    private Animator animator;

    [Header("Player Size")]
    [SerializeField] private float checkHeight = 1.5f;
    [SerializeField] private float checkWidth = 0.5f;

    [Header("Teleport Logic")]
    [SerializeField] private int maxChecks = 10;
    [SerializeField] private float verticalStep = 0.1f;
    [SerializeField] private LayerMask collisionMask;

    [Header("Wall Detection")]
    [SerializeField] private float wallCheckOffset = 0.05f;

    public bool IsWallGrabbable { get; private set; }
    public int WallDirection { get; private set; } // -1 = left, 1 = right

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (animator != null)
        {
            animator.SetBool("spin", true);
            SoundManager.instance.PlaySound("throw", this.transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        DetectWallGrabSpot();

        if (animator != null)
        {
            animator.SetBool("spin", false);
            animator.SetBool("idle", true);
            SoundManager.instance.StopSound("throw");
            SoundManager.instance.PlaySound("hit", this.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            PlayerController.instance?.ResetTeleportMarker();
        }
    }

    public Vector2 CalculateSafeTeleportPosition()
    {
        Vector2 basePosition = transform.position;

        for (int i = 0; i < maxChecks; i++)
        {
            float offsetY = i * verticalStep;
            Vector2 checkCenter = basePosition + Vector2.up * offsetY;
            Vector2 boxSize = new Vector2(checkWidth, checkHeight);

            bool isBlocked = Physics2D.OverlapBox(checkCenter, boxSize, 0f, collisionMask);
            if (!isBlocked)
                return checkCenter;
        }

        return basePosition + Vector2.up * checkHeight * 0.5f;
    }

    private void DetectWallGrabSpot()
    {
        IsWallGrabbable = false;
        WallDirection = 0;

        Vector2 origin = transform.position;
        int[] directions = { -1, 1 };

        foreach (int dir in directions)
        {
            // Step 1: Check for wall next to marker
            Vector2 wallCheckOrigin = origin + Vector2.right * dir * (checkWidth * 0.5f + wallCheckOffset);
            Vector2 wallCheckSize = new Vector2(0.05f, checkHeight);
            bool wallPresent = Physics2D.OverlapBox(wallCheckOrigin, wallCheckSize, 0f, collisionMask);

            if (!wallPresent)
                continue;

            // Step 2: Check for vertical clearance to grab
            Vector2 clearanceCheckOrigin = origin + Vector2.up * (checkHeight * 0.5f);
            Vector2 clearanceBoxSize = new Vector2(checkWidth, checkHeight);
            bool blockedAbove = Physics2D.OverlapBox(clearanceCheckOrigin, clearanceBoxSize, 0f, collisionMask);

            // Step 3: Check we're not grounded (no wall grab from floor)
            Vector2 groundCheckOrigin = origin + Vector2.down * wallCheckOffset;
            Vector2 groundBoxSize = new Vector2(checkWidth, wallCheckOffset);
            bool grounded = Physics2D.OverlapBox(groundCheckOrigin, groundBoxSize, 0f, collisionMask);

            if (!blockedAbove && !grounded)
            {
                IsWallGrabbable = true;
                WallDirection = dir;
                break;
            }
        }

#if UNITY_EDITOR
        Color gizmoColor = Color.cyan;
        Debug.DrawRay(origin + Vector2.left * (checkWidth * 0.5f + wallCheckOffset), Vector2.zero, gizmoColor, 1f);
        Debug.DrawRay(origin + Vector2.right * (checkWidth * 0.5f + wallCheckOffset), Vector2.zero, gizmoColor, 1f);
#endif
    }

    public bool ShouldGoToWallGrab(out int direction)
    {
        direction = WallDirection;
        return IsWallGrabbable;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 basePosition = transform.position;

        for (int i = 0; i < maxChecks; i++)
        {
            float offsetY = i * verticalStep;
            Vector2 checkCenter = basePosition + Vector2.up * offsetY;
            Gizmos.DrawWireCube(checkCenter, new Vector3(checkWidth, checkHeight, 0));
        }
    }
}
