using UnityEngine;

namespace COMP305
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance { get; private set; }
        private AudioSource soundSource;
        private AudioSource musicSource;

        private void Awake()
        {
            // Singleton Check
            if (instance == null)
            {
                instance = this;
                // DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject); // Prevent duplicates
                return;
            }

            // Initialize AudioSources
            soundSource = GetComponent<AudioSource>();
            if (transform.childCount > 0)
            {
                musicSource = transform.GetChild(0).GetComponent<AudioSource>();
            }
            else
            {
                Debug.LogError("Music AudioSource not found!");
            }

            // Assign initial values
            ChangeMusicVolume(0);
            ChangeSoundVolume(0);
        }

        public void PlaySound(AudioClip _sound, bool shouldLoop = false)
        {
            if (_sound != null)
            {
                if (shouldLoop)
                {
                    // For looping sounds, use Play() instead of PlayOneShot()
                    soundSource.clip = _sound;
                    soundSource.loop = true; // Enable looping
                    soundSource.Play();
                }
                else
                {
                    // Default behavior (one-shot)
                    soundSource.PlayOneShot(_sound);
                }
            }
            else
            {
                Debug.LogError("Sound clip is null!");
            }
        }

        // New method to stop all sound effects
        public void StopAllSounds()
        {
            if (soundSource != null)
            {
                soundSource.Stop();
            }
        }

        // Optional: Method to stop specific sound
        public void StopSound(AudioClip _sound)
        {
            // This is more complex and would require tracking active sounds
            // For simple implementation, use StopAllSounds()
        }

        // Optional: Method to stop music
        public void StopMusic()
        {
            if (musicSource != null && musicSource.isPlaying)
            {
                musicSource.Stop();
            }
        }

        private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
        {
            if (source == null)
            {
                Debug.LogError($"AudioSource for {volumeName} is missing!");
                return;
            }

            // Get and adjust volume
            float currentVolume = PlayerPrefs.GetFloat(volumeName, 1f);
            currentVolume += change;

            // Clamp between 0 and 1
            currentVolume = Mathf.Clamp01(currentVolume);

            // Apply new volume
            float finalVolume = currentVolume * baseVolume;
            source.volume = finalVolume;

            // Save the new volume
            PlayerPrefs.SetFloat(volumeName, currentVolume);
        }

        public void ChangeSoundVolume(float _change)
        {
            ChangeSourceVolume(1f, "soundVolume", _change, soundSource);
        }

        public void ChangeMusicVolume(float _change)
        {
            ChangeSourceVolume(0.3f, "musicVolume", _change, musicSource);
        }
    }
}