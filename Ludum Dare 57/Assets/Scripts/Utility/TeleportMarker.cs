using UnityEngine;

public class TeleportMarker : MonoBehaviour
{
    private Animator animator;

    [Header("Player Size")]
    [SerializeField] private float checkHeight = 1.5f;
    [SerializeField] private float checkWidth = 0.5f;

    [Header("Teleport Logic")]
    [SerializeField] private int maxChecks = 10;
    [SerializeField] private float verticalStep = 5f;
    [SerializeField] private LayerMask collisionMask;

    [Header("Wall Detection")]
    [SerializeField] private float wallCheckOffset = 5f;
    [SerializeField] private float contactRayDistance = 10f;

    public bool IsWallGrabbable { get; private set; }
    public int WallDirection { get; private set; } // -1 = left, 1 = right

    public enum SurfaceContact
    {
        None,
        LeftWall,
        RightWall,
        Ground,
        Ceiling
    }

    public SurfaceContact CurrentContact { get; private set; } = SurfaceContact.None;

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

        CurrentContact = CheckCardinalContact();
        Debug.Log(CurrentContact);

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
        Vector2 origin = transform.position;
        float rayLength = checkHeight + 0.1f;
        float offsetBuffer = 0.05f; // small gap to avoid overlap
        Vector2 playerSize = new Vector2(checkWidth, checkHeight);

        RaycastHit2D hitDown = Physics2D.Raycast(origin, Vector2.down, rayLength, collisionMask);
        RaycastHit2D hitUp   = Physics2D.Raycast(origin, Vector2.up,   rayLength, collisionMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin, Vector2.left, rayLength, collisionMask);
        RaycastHit2D hitRight= Physics2D.Raycast(origin, Vector2.right,rayLength, collisionMask);

        // 1. If there's something below, place the player on top of it
        if (hitDown.collider != null)
        {
            return hitDown.point + Vector2.up * (playerSize.y * 0.5f + offsetBuffer);
        }

        // 2. If there's something above, place the player below it
        if (hitUp.collider != null)
        {
            return hitUp.point + Vector2.down * (playerSize.y * 0.5f + offsetBuffer);
        }

        // 3. Left wall — place to the right of it
        if (hitLeft.collider != null)
        {
            return hitLeft.point + Vector2.right * (playerSize.x * 0.5f + offsetBuffer);
        }

        // 4. Right wall — place to the left of it
        if (hitRight.collider != null)
        {
            return hitRight.point + Vector2.left * (playerSize.x * 0.5f + offsetBuffer);
        }

        // 5. Fallback: current marker position + slight upward offset
        return origin + Vector2.up * (playerSize.y * 0.5f + offsetBuffer);
    }

    public SurfaceContact CheckCardinalContact()
    {
        Vector2 origin = transform.position;
        LayerMask mask = collisionMask;

        if (Physics2D.Raycast(origin, Vector2.down, contactRayDistance, mask))
            return SurfaceContact.Ground;

        if (Physics2D.Raycast(origin, Vector2.up, contactRayDistance, mask))
            return SurfaceContact.Ceiling;

        if (Physics2D.Raycast(origin, Vector2.left, contactRayDistance, mask))
            return SurfaceContact.LeftWall;

        if (Physics2D.Raycast(origin, Vector2.right, contactRayDistance, mask))
            return SurfaceContact.RightWall;

        return SurfaceContact.None;
    }

    private void DetectWallGrabSpot()
    {
        IsWallGrabbable = false;
        WallDirection = 0;

        // Use the result from CheckCardinalContact() to know if we're on a wall
        SurfaceContact contact = CheckCardinalContact();

        // Only proceed if we're on a left or right wall
        if (contact != SurfaceContact.LeftWall && contact != SurfaceContact.RightWall)
            return;

        WallDirection = (contact == SurfaceContact.LeftWall) ? -1 : 1;

        Vector2 origin = transform.position;
        float halfHeight = checkHeight * 0.5f;

        // Check for space above (for clearance)
        bool blockedAbove = Physics2D.Raycast(origin, Vector2.up, halfHeight + 1f, collisionMask);

        // Check for ground below too close
        bool groundTooClose = Physics2D.Raycast(origin, Vector2.down, halfHeight + 1f, collisionMask);

        if (!blockedAbove && !groundTooClose)
        {
            IsWallGrabbable = true;
        }

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

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector2.down * (checkHeight + 0.1f));
        Gizmos.DrawRay(transform.position, Vector2.up   * (checkHeight + 0.1f));
        Gizmos.DrawRay(transform.position, Vector2.left * (checkHeight + 0.1f));
        Gizmos.DrawRay(transform.position, Vector2.right* (checkHeight + 0.1f));

        // Cardinal direction debug rays
        Gizmos.color = Color.red;
        Gizmos.DrawRay(basePosition, Vector2.left * contactRayDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(basePosition, Vector2.right * contactRayDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(basePosition, Vector2.down * contactRayDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(basePosition, Vector2.up * contactRayDistance);
    }
}
