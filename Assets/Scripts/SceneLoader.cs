using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SceneLoader : MonoBehaviour {
        public void QuitGame(){
            Application.Quit();
            Debug.Log("Quitting Game...");
        }
    }
}
