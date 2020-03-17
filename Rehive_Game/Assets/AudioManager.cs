using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    instance = new GameObject("Created AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private AudioSource sfxSource;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        sfxSource = this.gameObject.AddComponent<AudioSource>();

    }

    public void PlaySoundEffect(AudioClip audioClip, float volume)
    {
        sfxSource.PlayOneShot(audioClip, volume);
    }

    public void SetSoundEffectVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}