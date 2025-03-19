using UnityEngine;
using UnityEngine.SceneManagement;

namespace COMP305
{
    public class Switch_Scene : MonoBehaviour
    {
        public void Switch(){
            Debug.Log("Start Game Button Clicked");
            SceneManager.LoadScene("Level-1");
    }
        public void Run() {
            Debug.Log("Start Game Button Clicked");
        }

    

    }
}
