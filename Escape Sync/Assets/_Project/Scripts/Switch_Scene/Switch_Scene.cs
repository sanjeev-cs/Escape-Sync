using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace COMP305
{
    public class Switch_Scene : MonoBehaviour
    {
        public void Switch(){
            Debug.Log("Start Game Button Clicked");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
        public void Run() {
            Debug.Log("Start Game Button Clicked");
        }

        public void QuitApplication()
        {
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
