using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Platformer
{
    public class MainMenu : MonoBehaviour
    {
    public void PlayButton(){
        SceneManager.LoadScene("EmoryTest");
    }
    public void QuitGame()
    {
    #if UNITY_EDITOR
        // If running in the Unity Editor, stop playing the scene.
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        // If running as a standalone build, quit the application.
        Application.Quit();
    #endif
    }
    }
}
