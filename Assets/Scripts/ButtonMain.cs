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
        Time.timeScale = 1;  // Resume game time if it was paused
        SceneManager.LoadScene("MainMenu");
    }
    }
}
