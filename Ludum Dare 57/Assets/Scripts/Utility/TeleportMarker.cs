using UnityEngine;

public class TeleportMarker : MonoBehaviour
{
    [Header("Player Size")]
    [SerializeField] private float checkHeight = 1.5f;
    [SerializeField] private float checkWidth = 0.5f;

    [Header("Teleport Logic")]
    [SerializeField] private int maxChecks = 10;
    [SerializeField] private float verticalStep = 0.1f;
    [SerializeField] private LayerMask collisionMask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (PlayerController.instance != null)
            {
                PlayerController.instance.ResetTeleportMarker();
            }
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

        // Fallback
        return basePosition + Vector2.up * checkHeight * 0.5f;
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
