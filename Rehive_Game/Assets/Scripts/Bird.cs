using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public StatsController statusPlayer;
    private float previousThreatLevel;
    public float distanceLevel;
    public float threatLevel;
    public GameObject intersection;
    public List<bool> inViewAccountant;
    public enum ViewState { inView, notInView}
    public ViewState viewOnPlayer = ViewState.notInView;
    private float distance;
    public int viewInstancesAmount = 10;
    public float DurationCheckViewState = 1f;
    public float DurationCheckViewInstance = 0.1f;
    public int viewInstanceThreshold = 4;
    public LayerMask layerMask;

    void Start()
    {
        inViewAccountant = new List<bool>();
        statusPlayer = player.GetComponent<StatsController>();
        StartCoroutine(CheckRoutinePlayerInView());
        StartCoroutine(CheckRoutineChangeOfState());
    }

    private IEnumerator CheckRoutineChangeOfState()
    {

        while (true)
        {

            CheckListForThresHold();
            yield return new WaitForSeconds(DurationCheckViewState);
        }
    }

    private void CheckListForThresHold()
    {
        int sum = 0;
        foreach(bool list in inViewAccountant)
        {

            if (list)
            {
                sum++;
            }
            
        }
        if(sum >= viewInstanceThreshold && statusPlayer.dangerState != StatsController.DangerState.safeZone)
        {
            if (viewOnPlayer == ViewState.notInView)
            {
                statusPlayer.threatCount++;
                viewOnPlayer = ViewState.inView;
            }
        }
        else
        {
            if (viewOnPlayer == ViewState.inView)
            {
                statusPlayer.threatCount--;
                viewOnPlayer = ViewState.notInView;
            }
        }
    }

    private IEnumerator CheckRoutinePlayerInView()
    {
        while (true)
        {
            AddListIfPlayerInView();
            yield return new WaitForSeconds(DurationCheckViewInstance);
        }

    }

    private void AddListIfPlayerInView()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = statusPlayer.targetPoint - transform.position;
        Ray rayToPlayer = new Ray(transform.position, directionToPlayer.normalized);
        Debug.DrawRay(transform.position, directionToPlayer);
        LayerMask layerMask = ~(1 << 8 | 1 << 9);
        Physics.Raycast(rayToPlayer, out hit, 100f, layerMask);
        distance = directionToPlayer.magnitude;
        if (hit.transform.tag == "player")
        {
            inViewAccountant.Add(true);
        }
        else
        {
            inViewAccountant.Add(false);
        }
        intersection = hit.transform.gameObject;
       
        if(inViewAccountant.Count > viewInstancesAmount)
        {
            inViewAccountant.RemoveAt(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        previousThreatLevel = threatLevel;
        distanceLevel = 1 / Mathf.Pow(distance, 1f); //* camoReduction;
        if(viewOnPlayer == ViewState.inView)
        {
            threatLevel = distanceLevel; 
            float camoReduction = 1- ((float)statusPlayer.currentColorIndex / 10f);
            statusPlayer.threatLevelRate -= previousThreatLevel;
            statusPlayer.threatLevelRate += threatLevel;
          
            statusPlayer.totalThreatLevel += threatLevel * Time.deltaTime;

        }
        else
        {
            threatLevel = 0;
            statusPlayer.threatLevelRate -= previousThreatLevel;

        }
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

        }
        else
        {

        }
    }
}
