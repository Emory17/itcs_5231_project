using KBCore.Refs;
using UnityEngine;

namespace Platformer
{
    public class CollisionHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Rigidbody rb;

        Transform platform; // The platform, if any, we are on top of
        Vector3 respawn = new Vector3 (0, 2, 0);

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlatform"))
            {
                // If the contact normal is pointing up, we've collided with the top of the platform
                ContactPoint contact = other.GetContact(0);
                if (contact.normal.y < 0.5f) return;

                platform = other.transform;
                transform.SetParent(platform);
            }
            if (other.gameObject.CompareTag("Respawn"))
            {
                rb.position = respawn;
                rb.rotation = Quaternion.identity;
                rb.velocity = Vector3.zero;
            }
        }

        void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlatform"))
            {
                transform.SetParent(null);
                platform = null;
            }
        }
    }
}
