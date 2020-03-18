using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DangerBar : MonoBehaviour
{
    private Transform birdSprite, barSprite;
    public PlayableDirector enterDanger;

    private void Start()
    {
        birdSprite = transform.Find("BirdSprite");
        barSprite = birdSprite.Find("Container_Bar");
        barSprite.localScale = new Vector3(1f, 0.0001f);
        DangerScript.dangerState = 0;
    }

    public void Update()
    {
        SetBarSize(StatsController.globalThreatLevel/15);
        if(StatsController.globalThreatLevel > 8 && DangerScript.dangerState == 0)
        {
            enterDanger.Play();
            DangerScript.dangerState = 0;
        }
    }

    public void SetBarSize (float barSize)
    {
        barSprite.localScale = new Vector3(1f, barSize);
    }
    
}
