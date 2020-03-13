using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioVolume : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolumeSlave", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFXVolumeSlave", Mathf.Log10(sliderValue) * 20);
    }
}
