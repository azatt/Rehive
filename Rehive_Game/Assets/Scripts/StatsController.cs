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
    public enum GrowingState { growing, stagnating}
    public enum DangerState { safe, danger}
    GrowingState growingState;
    DangerState dangerState;
    private CaterPillarMovement movementController;
    private UIController UIController;
    [SerializeField] GameObject body;
    [SerializeField] Material[] materials;

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
        if(growingState == GrowingState.growing)
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
                EnterSafeZone();
                break;
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        switch (otherCollider.gameObject.tag)
        {
            case PowerUp.Tag.SafeZone:
                ExitSafeZone();
                break;
        }
    }

    private void ExitSafeZone()
    {
        UIController.dangerText.text = "You are in DANGER!";
    }

    private void EnterSafeZone()
    {
        dangerState = DangerState.safe;
        UIController.dangerText.text = "You are safe now!";
    }

    private void AddToCamo(int amount)
    {
        playerStats.AddStats(Stats.Type.Camo, amount);
        UIController.camoText.text = "Camo:" + playerStats.camo.ToString();
        int colorIndex = Mathf.Clamp(playerStats.camo / 10, 0, 4);
        print("colorindex");
        print(colorIndex);
        body.GetComponent<Renderer>().enabled = true;       
        body.GetComponent<Renderer>().sharedMaterial = materials[colorIndex];
       // childObj.GetComponent<Renderer>().sharedMaterial.color
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
        targetScale = new Vector3(1 + playerStats.size/10, 1 + playerStats.size/10, 1+ playerStats.size/10);
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
        print("comple");
        growingState = GrowingState.stagnating; 
    }
}
