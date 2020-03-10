using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
     StatsController statusPlayer;
    public int threatLevel;
    public GameObject intersection;
    void Start()
    {
        statusPlayer = player.GetComponent<StatsController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (statusPlayer.dangerState == StatsController.DangerState.safeZone)
        {
            return;
        }
        CheckIfHidden();
    }

    public void CheckIfHidden()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = statusPlayer.targetPoint - transform.position;         
        Ray rayToPlayer = new Ray(transform.position, directionToPlayer.normalized);
        Physics.Raycast(rayToPlayer, out hit);
        if (hit.transform == player.transform)
        {

            statusPlayer.EnterDangerState();
            statusPlayer.threatLevel = (1 / directionToPlayer.magnitude) * 10;
            statusPlayer.UIController.threatLevel.text = "ThreatLevel:" + statusPlayer.threatLevel.ToString();

        }
        else
        {

            intersection = hit.transform.gameObject;
            statusPlayer.EnterHiddenState();
        }
    }
}
