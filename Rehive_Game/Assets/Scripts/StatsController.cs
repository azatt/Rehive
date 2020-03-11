using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    // Start is called before the first frame update
    Stats playerStats;
    [SerializeField] float transitionStartTime;
    [SerializeField] float growingTime, colorChangingTime;
    [SerializeField] Vector3 startingScale;
    [SerializeField] Vector3 targetScale;
    [SerializeField] GameObject body;
    [SerializeField] Color[] colors;
    [SerializeField] Material material;
    
    public float threatLevel;
    public int threatCount;
    public enum GrowingState { growing, stagnating }
    public enum DangerState { hidden, safeZone, danger, waitingForUpdate }

    GrowingState growingState;
    public DangerState dangerState;
    private CaterPillarMovement movementController;
    public UIController UIController;
    private Color endingColor;
    public float timeColorProgressInterpolation;
    private Color startingColor;
    public Color currentColor;
    public int currentColorIndex;
    public int coroutinesRunning;

    void Start()
    {
        playerStats = new Stats(0, 0, 0);
        growingState = GrowingState.stagnating;
        dangerState = DangerState.danger;

        movementController = GetComponent<CaterPillarMovement>();
        UIController = FindObjectOfType<UIController>();
        body = transform.Find("CaterPillarBody").gameObject;
        material = body.GetComponent<Renderer>().material;
        currentColor = colors[0];

    }

    // Update is called once per frame
    void Update()
    {
        if (growingState == GrowingState.growing)
        {
            growInterpolate();
        }
        threatLevel *= Mathf.Pow(0.90f, Time.deltaTime);
        UIController.threatLevel.text = "Count: " + threatCount.ToString() + " ThreatLevel:" +threatLevel.ToString();
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
        //UIController.threatLevel.text = "ThreatLevel:0";
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

        if(colorIndex > currentColorIndex)
        {
            print("change");
            StartCoroutine(startInterpolatedColor(colors[colorIndex]));
        }
        currentColorIndex = colorIndex;
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
        float scaleFromSize = 1 + (float)(playerStats.size) / 40f;
        targetScale = new Vector3(scaleFromSize, scaleFromSize, scaleFromSize);
        transform.gameObject.AddComponent<Bird>();
    }

    private void growInterpolate()
    {
        float fractionOfTransition = (Time.time - transitionStartTime) / growingTime;

        transform.localScale = Vector3.Lerp(
            startingScale,
            targetScale,
            fractionOfTransition);

        // Check if the fall is finished
    }
    IEnumerator startInterpolatedColor(Color newColor)
    {

        coroutinesRunning++;
        print("colorchange");
        startingColor = currentColor;
        endingColor = newColor;
        timeColorProgressInterpolation = 0f;
       
        while(timeColorProgressInterpolation <  colorChangingTime)
        {
            timeColorProgressInterpolation += Time.deltaTime;
             colorChangeInterpolate();

            yield return 0;
        }

        coroutinesRunning--;
    }

   private void colorChangeInterpolate()
   {
       float fractionOfTransition = timeColorProgressInterpolation / colorChangingTime;

       Color color = Color.Lerp(
           startingColor,
           endingColor,
           fractionOfTransition);

        // Check if the fall is finished
        currentColor = color;
        material.SetColor("_myColor", currentColor);
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
