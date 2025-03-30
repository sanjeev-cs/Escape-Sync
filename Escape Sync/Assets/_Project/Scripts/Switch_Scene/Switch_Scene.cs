using UnityEngine;
using UnityEngine.SceneManagement;

namespace COMP305
{
    public class Switch_Scene : MonoBehaviour
    {
        public void Switch(){
            Debug.Log("Start Game Button Clicked");
            SceneManager.LoadScene("Level-1",LoadSceneMode.Single);
    }
        public void Run() {
            Debug.Log("Start Game Button Clicked");
        }

        public void QuitApplication()
        {
            // 在编辑器中打印消息以确认调用
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // 在构建的应用程序中退出游戏
        Application.Quit();
#endif
        }



    }
}
