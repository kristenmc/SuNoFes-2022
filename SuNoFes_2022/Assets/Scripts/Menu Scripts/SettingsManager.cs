using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Canvas _settingsCanvas;

    [SerializeField] private Settings _savedSettings;
    [SerializeField] private Settings _defaultSettings; 
    [SerializeField] private Slider _allSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Start()
    {
        LoadPrefs();
    }

    public void CloseSettings()
    {
        SavePrefs();
        _settingsCanvas.enabled = false;
    }

    public void LoadPrefs()
    {
        _allSlider.value = _savedSettings.AllVolume;
        AkSoundEngine.SetRTPCValue("AllVolume", _savedSettings.AllVolume);
        _musicSlider.value = _savedSettings.MusicVolume;
        AkSoundEngine.SetRTPCValue("MusicVolume", _savedSettings.MusicVolume);
        _sfxSlider.value = _savedSettings.SFXVolume;
        AkSoundEngine.SetRTPCValue("SFXVolume", _savedSettings.SFXVolume);

    }

    public void SavePrefs()
    {
        _savedSettings.AllVolume = _allSlider.value;
        _savedSettings.MusicVolume = _musicSlider.value;
        _savedSettings.SFXVolume = _sfxSlider.value;
    }

    public void ResetPrefs()
    {
        _allSlider.value = _savedSettings.AllVolume = _defaultSettings.AllVolume;
        _musicSlider.value = _savedSettings.MusicVolume = _defaultSettings.MusicVolume;
        _sfxSlider.value = _savedSettings.SFXVolume = _defaultSettings.SFXVolume;
    }
}
