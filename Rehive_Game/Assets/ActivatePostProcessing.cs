using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ActivatePostProcessing : MonoBehaviour
{
    public PostProcessVolume volume;
    //public PostProcessingProfile ppProfile;

    void Start()
    {
        PostProcessVolume volume = gameObject.GetComponent<PostProcessVolume>();
    }

    
}
