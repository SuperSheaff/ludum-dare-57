using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [Tooltip("New respawn position for the player.")]
    public Transform checkpointPosition;

    private bool hasActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasActivated) return;

        if (other.CompareTag("Player") && checkpointPosition != null)
        {
            PlayerController.instance.SetSpawnPoint(checkpointPosition.position);
            hasActivated = true;
            Debug.Log("Checkpoint set at: " + checkpointPosition.position);
        }
    }

    private void OnDrawGizmos()
    {
        if (checkpointPosition != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(checkpointPosition.position, 0.3f);
        }
    }
}
