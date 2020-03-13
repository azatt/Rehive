using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerBar : MonoBehaviour
{
    private Transform birdSprite, barSprite;

    private void Start()
    {
        birdSprite = transform.Find("BirdSprite");
        barSprite = birdSprite.Find("Container_Bar");
        barSprite.localScale = new Vector3(1f, 0.0001f);
    }

    public void SetBarSize (float barSize)
    {
        barSprite.localScale = new Vector3(1f, barSize);
    }
    
    
}
