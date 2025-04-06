using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }

    [SerializeField] private Material crtMaterial;
    [SerializeField] private string shaderPropertyName = "_Scan_Lines_Amount";

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

            // CinemachineCamera cam = cameraToActivate.GetComponent<CinemachineCamera>();
            // UpdateShaderFromCamera(cam);
        }
    }

    public void UpdateShaderFromCamera(CinemachineCamera cam)
    {
        if (crtMaterial == null || cam == null) return;

        // Set to shader
        crtMaterial.SetFloat(shaderPropertyName, cam.Lens.OrthographicSize * 4);
    }
}
