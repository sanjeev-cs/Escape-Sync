using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Audio;

namespace COMP305
{
    public class Collect_Fruit : MonoBehaviour
    {
        public Animator animator;
        public AudioClip hoverSound; // 定义要播放的音效
        private AudioSource audioSource;
        private bool hasPlayedSound = false;

        public void Start()
        {
            // 正确获取Animator组件
            audioSource = GetComponent<AudioSource>();

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            animator = GetComponent<Animator>();
            
            if (collision.CompareTag("Player") && !hasPlayedSound) // 检测碰撞对象是否是玩家
            {
                // 播放消失动画
                animator.SetTrigger("Disappear");
                audioSource.PlayOneShot(hoverSound);

                // 更新分数
                // gameManager.AddScore(1);

                // 可以在动画结束后销毁对象
                Destroy(gameObject, 0.5f); // 1秒后销毁，可以根据动画时长调整
            }
        }
    }
}

