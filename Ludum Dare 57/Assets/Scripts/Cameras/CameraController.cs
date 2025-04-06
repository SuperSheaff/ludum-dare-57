using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    /// <summary>
    /// Activates the specified camera and disables all others.
    /// </summary>
    public void SwitchToCamera(GameObject cameraToActivate)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        if (cameraToActivate != null)
        {
            cameraToActivate.SetActive(true);
        }
    }
}
