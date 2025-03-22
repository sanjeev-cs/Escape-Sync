using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Audio;

namespace COMP305
{
    public class Collect_Fruit : MonoBehaviour
    {
        public Animator animator;
        public AudioClip hoverSound;
        private AudioSource audioSource;
        private bool hasPlayedSound = false;

        public void Start()
        {
           
            audioSource = GetComponent<AudioSource>();

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            animator = GetComponent<Animator>();
            
            if (collision.CompareTag("Player") && !hasPlayedSound) 
            {
               
                animator.SetTrigger("Disappear");
                audioSource.PlayOneShot(hoverSound);
                hasPlayedSound = true;

             
                // gameManager.AddScore(1);

                Destroy(gameObject, 0.5f); 
            }
        }
    }
}

