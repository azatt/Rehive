using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DangerScript : MonoBehaviour
{
    public PlayableDirector exitDangerState;
    public PlayableDirector currentDangerState;
    public static int dangerState;

    void Update()
    {
        if (StatsController.globalThreatLevel < 8)
        {
            int a = 0;
            if (DangerScript.dangerState == 1)
            {
                exitDangerState.Play();
                dangerState = 2;
                currentDangerState.Stop();
            }
        }
    }
}
