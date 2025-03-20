using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Audio;

namespace COMP305
{
    public class Collect_Fruit : MonoBehaviour
    {
        public Animator animator;
        public AudioClip hoverSound; // ����Ҫ���ŵ���Ч
        private AudioSource audioSource;
        private bool hasPlayedSound = false;

        public void Start()
        {
            // ��ȷ��ȡAnimator���
            audioSource = GetComponent<AudioSource>();

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            animator = GetComponent<Animator>();
            
            if (collision.CompareTag("Player") && !hasPlayedSound) // �����ײ�����Ƿ������
            {
                // ������ʧ����
                animator.SetTrigger("Disappear");
                audioSource.PlayOneShot(hoverSound);

                // ���·���
                // gameManager.AddScore(1);

                // �����ڶ������������ٶ���
                Destroy(gameObject, 0.5f); // 1������٣����Ը��ݶ���ʱ������
            }
        }
    }
}

