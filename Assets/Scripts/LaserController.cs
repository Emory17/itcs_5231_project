using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Platformer
{
    public class LaserController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] float lifespan = 5f;

        void Awake()
        {
            Destroy(gameObject,lifespan);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}
