using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings")]
public class Settings : MonoBehaviour
{
    [SerializeField] [Range(0.0001f, 1f)] private float _allVolume = 1f;
    public float AllVolume{ get{return _allVolume;} set{_allVolume = value;}}
    [SerializeField] [Range(0.0001f, 1f)] private float _musicVolume = 1f;
    public float MusicVolume { get{return _musicVolume;} set{_musicVolume = value;} }
    [SerializeField] [Range(0.0001f, 1f)] private float _sfxVolume = 1f;
    public float SFXVolume { get{return _sfxVolume;} set{_sfxVolume = value;} }
}
