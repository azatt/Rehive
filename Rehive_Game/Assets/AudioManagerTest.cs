using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerTest : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect1;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.Instance.PlaySoundEffect(soundEffect1, 1);
        }
    }
}
