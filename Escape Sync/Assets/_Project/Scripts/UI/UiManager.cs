using System;
using UnityEngine;

namespace COMP305
{
    public class UiManager : MonoBehaviour
    {
        [Header("GameOver Panel")]
        [SerializeField] GameObject gameOverPanel;
        
        [Header("Sound")]
        [SerializeField] private AudioClip gameOverSound;

        public static UiManager Instance { get; private set; } // Singleton pattern

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void GameOver()
        {
            gameOverPanel.SetActive(true);
            SoundManager.instance.playeSound(gameOverSound);
        }
    }
}
