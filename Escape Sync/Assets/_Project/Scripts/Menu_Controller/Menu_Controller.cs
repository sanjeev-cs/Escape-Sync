using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public Slider volumeSlider;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // ≥ı ºªØ…Ë÷√
        settingsMenu.SetActive(false);
        volumeSlider.value = audioSource.volume;

        volumeSlider.onValueChanged.AddListener(OnVolumeChange);
    }

   public void OpenSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Mute() {
        volumeSlider.value = 0;
    }
   
    void OnVolumeChange(float value)
    {
        audioSource.volume = value;
    }
}