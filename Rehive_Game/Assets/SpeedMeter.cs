using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    private Transform speedContainer, clippingMask;

    private void Start()
    {
        clippingMask = transform.Find("ClippingMask");
        speedContainer = clippingMask.Find("SpeedContainer");
        speedContainer.localScale = new Vector3(0.0001f, 1f);
    }

    public void SetBarSize(float speedbarSize)
    {
        speedContainer.localScale = new Vector3(speedbarSize, 1f);
    }
}
