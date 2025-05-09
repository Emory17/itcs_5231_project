using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace Platformer
{
    public class EnemyController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Self] Rigidbody rb;
        [SerializeField] Rigidbody playerrb;

        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 300f;
        [SerializeField] float moveRange = 100f;

        [Header("Health Settings")]
        [SerializeField] int maxHealth = 3;

        int chealth;
        void Awake()
        {
            chealth = maxHealth;
        }

        private void FixedUpdate()
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                HandleMovement();
                HandleHealth();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("PlayerDamage"))
            {
                chealth -= 1;
            }
            if (other.gameObject.CompareTag("Respawn"))
            {
                Destroy(gameObject);
            }
        }

        void HandleMovement()
        {
            Vector3 lookAt = playerrb.transform.position;
            lookAt.y = transform.position.y;
            transform.LookAt(lookAt);

            if(Vector3.Distance(transform.position, playerrb.transform.position) <= moveRange)
            {
                Vector3 velocity = transform.forward * (moveSpeed * Time.fixedDeltaTime);
                rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
            }
        }

        void HandleHealth()
        {
            if (chealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
