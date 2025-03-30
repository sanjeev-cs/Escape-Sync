using UnityEngine;

namespace COMP305
{
    public class HealthCollectible : MonoBehaviour
    {
        [SerializeField] private float healthValue;
        [Header("Sound")]
        [SerializeField] private AudioClip healthCollectibleSound;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                SoundManager.instance.PlaySound(healthCollectibleSound);
                collision.GetComponent<Health>().AddHealth(healthValue);
                gameObject.SetActive(false);
            }
        }
    }
}
