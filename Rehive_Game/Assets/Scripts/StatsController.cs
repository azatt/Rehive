using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    // Start is called before the first frame update
    Stats playerStats;
    [SerializeField] float transitionStartTime;
    [SerializeField] float growingTime;
    [SerializeField] Vector3 startingScale;
    [SerializeField] Vector3 targetScale;
    [SerializeField] GameObject body;
    [SerializeField] Material[] materials;
    
    public float threatLevel;
    public enum GrowingState { growing, stagnating }
    public enum DangerState { hidden, safeZone, danger, waitingForUpdate }

    GrowingState growingState;
    public DangerState dangerState;
    private CaterPillarMovement movementController;
    public UIController UIController;

    void Start()
    {
        playerStats = new Stats(0, 0, 0);
        growingState = GrowingState.stagnating;
        dangerState = DangerState.danger;

        movementController = GetComponent<CaterPillarMovement>();
        UIController = FindObjectOfType<UIController>();
        body = transform.Find("CaterPillarBody").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        print(playerStats.size);
        if (growingState == GrowingState.growing)
        {
            growInterpolate();
        }
    }
    void OnTriggerEnter(Collider otherCollider)
    {

        switch (otherCollider.gameObject.tag)
        {
            case PowerUp.Tag.Speed:
                AddToSpeed(10);
                Destroy(otherCollider.gameObject);
                break;
            case PowerUp.Tag.Size:
                AddToSize(10);
                Destroy(otherCollider.gameObject);
                break;
            case PowerUp.Tag.Camo:
                AddToCamo(10);
                Destroy(otherCollider.gameObject);
                break;
            case PowerUp.Tag.SafeZone:
                EnterSafeZoneState();
                break;
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        switch (otherCollider.gameObject.tag)
        {
            case PowerUp.Tag.SafeZone:
                EnterWaitForUpdateState();
                break;
        }
    }

    private void EnterWaitForUpdateState()
    {
        dangerState = DangerState.waitingForUpdate;
    }

    public void EnterDangerState()
    {
        dangerState = DangerState.danger;
        UIController.dangerText.text = "You are in DANGER!";
    }
    public void EnterHiddenState()
    {
        dangerState = DangerState.hidden;
        UIController.dangerText.text = "You are hidden!";
        UIController.threatLevel.text = "ThreatLevel:0";
    }


    public void EnterSafeZoneState()
    {
        dangerState = DangerState.safeZone;
        UIController.dangerText.text = "You are in a safezone!";
        UIController.threatLevel.text = "ThreatLevel:0";

    }

    private void AddToCamo(int amount)
    {
        playerStats.AddStats(Stats.Type.Camo, amount);
        UIController.camoText.text = "Camo:" + playerStats.camo.ToString();
        int colorIndex = Mathf.Clamp(playerStats.camo / 10, 0, 4);
        body.GetComponent<Renderer>().enabled = true;
        body.GetComponent<Renderer>().sharedMaterial = materials[colorIndex];
    }

    private void AddToSpeed(int amount)
    {
        playerStats.AddStats(Stats.Type.Speed, amount);
        UIController.speedText.text = "Speed:" + playerStats.speed.ToString();
        movementController.speed = movementController.initialSpeed + playerStats.speed / 10;
    }

    private void AddToSize(int amount)
    {
        growingState = GrowingState.growing;
        playerStats.AddStats(Stats.Type.Size, amount);
        startingScale = transform.localScale;
        transitionStartTime = Time.time;
        UIController.sizeText.text = "Size:" + playerStats.size.ToString();
        float scaleFromSize = 1 + (float)(playerStats.size) / 20f;
        targetScale = new Vector3(scaleFromSize, scaleFromSize, scaleFromSize);
    }

    private void growInterpolate()
    {
        float fractionOfTransition = (Time.time - transitionStartTime) / growingTime;

        transform.localScale = Vector3.Lerp(
            startingScale,
            targetScale,
            fractionOfTransition);

        // Check if the fall is finished
        if (fractionOfTransition >= 1) growthComplete();            //wanneer t karakter klaar is met vallen gaat deze naar 'ReachedFallDestination()'
    }

    private void growthComplete()
    {
        growingState = GrowingState.stagnating;
    }
    public Vector3 targetPoint
    {
        get{
            return transform.position + new Vector3(0, 0.5f, 0);
        }
    }

}
