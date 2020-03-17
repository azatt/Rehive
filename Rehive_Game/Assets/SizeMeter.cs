using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeMeter : MonoBehaviour
{
    private Transform sizeContainer, clippingMask;

    private void Start()
    {
        clippingMask = transform.Find("ClippingMask_Size");
        sizeContainer = clippingMask.Find("SizeContainer");
        sizeContainer.localScale = new Vector3(0.0001f, 1f);
    }

    public void SetBarSize(float sizebarSize)
    {
        sizeContainer.localScale = new Vector3(sizebarSize, 1f);
    }
}
