using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Platformer
{
    public class MainMenu : MonoBehaviour
    {
    public bool isPlay; 
    public bool isQuit;
    void OnMouseUp(){
        if(isPlay){
            SceneManager.LoadScene(1);
            GetComponent<Renderer>().material.color = Color.cyan;
            }
        if(isQuit){
            Application.Quit();
        }
    }
    }
}
