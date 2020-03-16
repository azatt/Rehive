using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CheckExitDanger : MonoBehaviour
{
    public PlayableDirector enterDangerState;
    public PlayableDirector exitDangerState;

    void Update()
    {
        if (StatsController.globalThreatLevel > 2)
        {
            enterDangerState.Play();
            DangerScript.dangerState = 0;
            enterDangerState.Stop();
        }
        else
        {
            exitDangerState.Stop();
            DangerScript.dangerState = 0;
        }
    }
}
