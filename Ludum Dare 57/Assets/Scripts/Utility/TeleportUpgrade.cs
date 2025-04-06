using UnityEngine;

public class TeleportUpgrade : MonoBehaviour
{
    // [SerializeField] private AudioClip pickupSound;
    // [SerializeField] private GameObject pickupVFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            if (PlayerController.instance != null && !PlayerController.instance.HasUnlockedTeleport)
            {
                GameController.instance.ShowText(1);
                PlayerController.instance.UnlockTeleportAbility();

                // if (pickupSound != null)
                // {
                //     AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                // }

                // if (pickupVFX != null)
                // {
                //     Instantiate(pickupVFX, transform.position, Quaternion.identity);
                // }

                Destroy(gameObject);
            }
        }
    }
}
