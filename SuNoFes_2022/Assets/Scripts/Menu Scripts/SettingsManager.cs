using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Not my code. It is being used by the game though.
public class SettingsManager : MonoBehaviour
{
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
    }

    public void LoadPrefs()
    {
        _allSlider.value = _savedSettings.AllVolume;
        AkSoundEngine.SetRTPCValue("AllVolume", _savedSettings.AllVolume * 100);
        _musicSlider.value = _savedSettings.MusicVolume;
        AkSoundEngine.SetRTPCValue("MusicVolume", _savedSettings.MusicVolume * 100);
        _sfxSlider.value = _savedSettings.SFXVolume;
        AkSoundEngine.SetRTPCValue("SFXVolume", _savedSettings.SFXVolume * 100);

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

    public void SetAllVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue("AllVolume", volume*100);
    }

    public void SetMusicVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue("MusicVolume", volume*100);
    }

    public void SetSFXVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue("SFXVolume", volume*100);
    }
}
