using UnityEngine;
using UnityEngine.EventSystems;

namespace COMP305
{
    public class ButtonSoundEffect : MonoBehaviour, IPointerEnterHandler
    {
        public AudioClip hoverSound;

        // �������밴ťʱ�����˷���
        public void OnPointerEnter(PointerEventData eventData)
        {
            // ��ȡ����� AudioSource ���
            AudioSource audioSource = GetComponent<AudioSource>();

            // ����Ƿ�����ƵԴ����Ч
            if (audioSource != null && hoverSound != null)
            {
                audioSource.PlayOneShot(hoverSound);
            }
        }
    }
}
