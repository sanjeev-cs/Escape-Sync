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
            // �ڱ༭���д�ӡ��Ϣ��ȷ�ϵ���
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // �ڹ�����Ӧ�ó������˳���Ϸ
        Application.Quit();
#endif
        }



    }
}
