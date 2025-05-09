using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class ButtonMain : MonoBehaviour
    {
        // Start is called before the first frame update
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                #if UNITY_EDITOR
                    // If running in the Unity Editor, stop playing the scene.
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    // If running as a standalone build, quit the application.
                    Application.Quit();
                #endif
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
