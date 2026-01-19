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
    //[SerializeField] private CinemachineInputAxisController inputAxisController;
    
    void Start()
    {
        // Fullscreen
        bool isFullscreen = VariableStore.GetFullscreen();
        SetFullScreen(isFullscreen);

        // Graphics Quality
        SetGraphicsQuality(VariableStore.GetQuality());

        // Resolution 
        SetResolution(VariableStore.GetResolution());

        // Volume
        SetVolume(VariableStore.GetVolume());

        // Sensitivity
        SetSensitivity(VariableStore.GetSensitivity());
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        fullscreenToggle.isOn = isFullScreen;
        VariableStore.SetFullscreen(isFullScreen);
    }

    public void SetGraphicsQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        VariableStore.SetQuality(index);
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
        VariableStore.SetResolution(index);
        resolutionDropdown.value = index;
    }

    public void SetVolume(float volume)
    {
        VariableStore.SetVolume(volume);
        volumeSlider.value = volume;
        audioMixer.SetFloat("volume", volume);
    }
    
    public void SetSensitivity(float sensitivity)
    {
        VariableStore.SetSensitivity(sensitivity);
        sensitivitySlider.value = sensitivity;
    }
}
