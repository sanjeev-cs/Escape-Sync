using UnityEngine;

namespace COMP305
{
    public class Help_Menu_PoP : MonoBehaviour
    {
        public GameObject helpMenu;
        
        private bool viewd = false;

        void Start()
        {
            helpMenu.SetActive(false); // 确保初始状态下隐藏帮助菜单
        }

        void Update()
        {
            // 如果需要关闭菜单，可以在此添加逻辑
            if (helpMenu.activeSelf && Input.GetKeyDown(KeyCode.H)) // 假设H键是关闭菜单
            {
                CloseHelpMenu();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && viewd == false)
            {
                OpenHelpMenu();
            }
        }

        private void OpenHelpMenu()
        {
            if (viewd == false)
            {
                helpMenu.SetActive(true);
            } // 显示帮助菜单
        }

        private void CloseHelpMenu()
        {
            helpMenu.SetActive(false); // 隐藏帮助菜单
            viewd = true;
        }
    }
}