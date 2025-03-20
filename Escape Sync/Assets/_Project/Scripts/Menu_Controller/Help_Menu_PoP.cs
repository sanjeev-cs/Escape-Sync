using UnityEngine;

namespace COMP305
{
    public class Help_Menu_PoP : MonoBehaviour
    {
        public GameObject helpMenu;
        
        private bool viewd = false;

        void Start()
        {
            helpMenu.SetActive(false); // ȷ����ʼ״̬�����ذ����˵�
        }

        void Update()
        {
            // �����Ҫ�رղ˵��������ڴ�����߼�
            if (helpMenu.activeSelf && Input.GetKeyDown(KeyCode.H)) // ����H���ǹرղ˵�
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
            } // ��ʾ�����˵�
        }

        private void CloseHelpMenu()
        {
            helpMenu.SetActive(false); // ���ذ����˵�
            viewd = true;
        }
    }
}