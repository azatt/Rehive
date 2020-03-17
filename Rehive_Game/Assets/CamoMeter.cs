using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamoMeter : MonoBehaviour
{
    private Transform camoContainer, clippingMask;

    private void Start()
    {
        clippingMask = transform.Find("ClippingMask_Camo");
        camoContainer = clippingMask.Find("CamoContainer");
        camoContainer.localScale = new Vector3(0.0001f, 1f);
    }

    private void Update()
    {
        SetBarSize(StatsController.camoValue / 100);
    }

    public void SetBarSize(float camobarSize)
    {
        camoContainer.localScale = new Vector3(camobarSize, 1f);
    }
}
