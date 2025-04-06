using UnityEngine;

public class CameraAreaTrigger : MonoBehaviour
{
    [SerializeField] private GameObject cameraToUse;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.instance.SwitchToCamera(cameraToUse);
        }
    }
}
