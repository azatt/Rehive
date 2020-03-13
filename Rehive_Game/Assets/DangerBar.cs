using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DangerBar : MonoBehaviour
{
    private Transform birdSprite, barSprite;
    public GameObject dangerState;

    private void Start()
    {
        birdSprite = transform.Find("BirdSprite");
        barSprite = birdSprite.Find("Container_Bar");
        barSprite.localScale = new Vector3(1f, 0.0001f);
    }

    public void Update()
    {
        SetBarSize(StatsController.globalThreatLevel/10);
        if(StatsController.globalThreatLevel > 5)
        {
            dangerState.SetActive(true);
        }
    }

    public void SetBarSize (float barSize)
    {
        barSprite.localScale = new Vector3(1f, barSize);
    }
    
}
