using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class ExitSpawner : MonoBehaviour
    {
        [SerializeField] GameObject exitPrefab;

        Boolean spawned = false;

        // Update is called once per frame
        void Update()
        {
            if (!spawned && GameObject.FindWithTag("Data") == null)
            {
                spawned = true;
                var exit = Instantiate(exitPrefab, transform.position, transform.rotation);
            }
        }
    }
}
