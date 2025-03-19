using UnityEngine;
using UnityEngine.EventSystems;

namespace COMP305
{
    public class ButtonSoundEffect : MonoBehaviour, IPointerEnterHandler
    {
        public AudioClip hoverSound;

        // 当鼠标进入按钮时触发此方法
        public void OnPointerEnter(PointerEventData eventData)
        {
            // 获取自身的 AudioSource 组件
            AudioSource audioSource = GetComponent<AudioSource>();

            // 检查是否有音频源和音效
            if (audioSource != null && hoverSound != null)
            {
                audioSource.PlayOneShot(hoverSound);
            }
        }
    }
}
