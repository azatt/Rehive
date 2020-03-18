using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CheckEnterDangerState : MonoBehaviour
{
    public PlayableDirector enterDangerState;
    public PlayableDirector dangerState;
    public PlayableDirector exitDangerState;
    public float enterDanger;

    void Update()
    {
        if (StatsController.globalThreatLevel > enterDanger)
        {
            dangerState.Play();
            DangerScript.dangerState = 1;
            enterDangerState.Stop();
        }
        else
        {
            exitDangerState.Play();
            DangerScript.dangerState = 2;
            enterDangerState.Stop();
        }
    }
}
