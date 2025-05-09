using KBCore.Refs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class CollisionHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Rigidbody rb;

        Transform platform; // The platform, if any, we are on top of

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
                SceneManager.LoadScene("Level");
            }
            if (other.gameObject.CompareTag("Exit"))
            {
                SceneManager.LoadScene("WinScreen");
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
