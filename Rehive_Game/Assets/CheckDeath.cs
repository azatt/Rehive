using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDeath : MonoBehaviour
{
    public GameObject target;
    public GameObject oldTragets;

    // Update is called once per frame
    void Update()
    {
        CheckThreat();
    }

    protected void CheckThreat()
    {
        if (StatsController.globalThreatLevel > 5)
        {
            target.SetActive(true);
            oldTragets.SetActive(false);
        }
        else
        {
            target.SetActive(false);
            oldTragets.SetActive(true);
        }
    }

    protected void CheckBirdDistance()
    {
        GameObject[] birdList = GameObject.FindGameObjectsWithTag("lb_bird");
        foreach(var bird in birdList)
        {
            if(Vector3.Distance(transform.position,bird.transform.position) < 1f)
            {
                print("DIE INSECT");
            }
        }
    }
}
