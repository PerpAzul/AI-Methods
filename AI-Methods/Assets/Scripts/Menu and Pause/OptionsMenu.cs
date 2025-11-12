using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using Unity.Cinemachine;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private CinemachineInputAxisController inputAxisController;
    
    void Start()
    {
        // Fullscreen
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        SetFullScreen(isFullscreen);

        // Graphics Quality
        SetGraphicsQuality(PlayerPrefs.GetInt("Quality", 2));

        // Resolution 
        SetResolution(PlayerPrefs.GetInt("Resolution", 1));

        // Volume
        SetVolume(PlayerPrefs.GetFloat("Volume", 1f));

        // Sensitivity
        SetSensitivity(PlayerPrefs.GetFloat("Sensitivity", 2f));
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        fullscreenToggle.isOn = isFullScreen;
        PlayerPrefs.SetInt("Fullscreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetGraphicsQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("Quality", index);
        PlayerPrefs.Save();
        graphicsDropdown.value = index;
    }

    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(800, 600, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1980, 1080, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(2160, 1440, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(3840, 2160, Screen.fullScreen);
                break;
        }
        PlayerPrefs.SetInt("Resolution", index);
        PlayerPrefs.Save();
        resolutionDropdown.value = index;
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
        volumeSlider.value = volume;
        audioMixer.SetFloat("volume", volume);
    }
    
    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        PlayerPrefs.Save();
        sensitivitySlider.value = sensitivity;
        
        if (inputAxisController != null)
        {
            
        }
    }
}
