using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public StatsController statusPlayer;
    public int threatLevel;
    public GameObject intersection;
    public bool playerInView;
    void Start()
    {
        statusPlayer = player.GetComponent<StatsController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfHidden();
    }

    public void CheckIfHidden()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = statusPlayer.targetPoint - transform.position;         
        Ray rayToPlayer = new Ray(transform.position, directionToPlayer.normalized);
        Debug.DrawRay(transform.position, directionToPlayer);
        Physics.Raycast(rayToPlayer, out hit);
        if (hit.transform == player.transform)
        {

            if (!playerInView)
            {
                playerInView = true;
                statusPlayer.threatCount++;
            }
            statusPlayer.EnterDangerState();
            statusPlayer.threatLevel += (1 / directionToPlayer.magnitude) * 10 * Time.deltaTime;
        }
        else
        {

            if (playerInView)
            {
                playerInView = false;
                statusPlayer.threatCount--;
            }
            intersection = hit.transform.gameObject;
            statusPlayer.EnterHiddenState();
        }
    }
}
