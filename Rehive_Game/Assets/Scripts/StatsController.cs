using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StatsController : MonoBehaviour
{
    // Start is called before the first frame update
    Stats playerStats;
    [SerializeField] float transitionStartTime;
    [SerializeField] float growingTime = 2 ;
    [SerializeField] float colorChangingTime = 2;
    [SerializeField] Vector3 startingScale;
    [SerializeField] Vector3 targetScale;
    [SerializeField] GameObject camoBody;
    [SerializeField] GameObject scalingBody;
    [SerializeField] Color[] colors;
    [SerializeField] Material[] materials;
    
    public float totalThreatLevel;
    public int threatCount;
    public enum GrowingState { growing, stagnating }
    public enum DangerState { hidden, safeZone, danger, waitingForUpdate }

    GrowingState growingState;
    public DangerState dangerState;
    private Climb movementController;
    public UIController UIController;
    private Color endingColor;
    public float timeColorProgressInterpolation;
    private Color startingColor;
    public Color currentColor;
    public int currentColorIndex;
    public int coroutinesRunning;
    public float initialScale;
    public GameObject collObj;
    public float scalingSpeed = 40;

    void Start()
    {
        playerStats = new Stats(0, 0, 0);
        growingState = GrowingState.stagnating;
        dangerState = DangerState.danger;

        movementController = GetComponent<Climb>();
        UIController = FindObjectOfType<UIController>();
        materials = camoBody.GetComponent<SkinnedMeshRenderer>().materials;
        currentColor = colors[0];
        SetColorOfMaterials();
        
        StartCoroutine(CheckDangerState());
        initialScale = scalingBody.transform.localScale.x;

    }

    private void SetColorOfMaterials()
    {
        foreach (Material material in materials)
        {
            material.SetColor("_CamoColor", currentColor);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (growingState == GrowingState.growing)
        {
            growInterpolate();
        }
        totalThreatLevel *= Mathf.Pow(0.90f, Time.deltaTime);
        UIController.threatLevel.text = "Count: " + threatCount.ToString() + " ThreatLevel:" + totalThreatLevel.ToString();


    }

    private IEnumerator CheckDangerState()
    {
        while (true)
        {

            if (threatCount > 0)
            {
                EnterDangerState();
            }
            else
            {
                EnterHiddenState();
            }
            yield return new WaitForSeconds(0.1f);
            
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {

        switch (otherCollider.gameObject.tag)
        {
            case PowerUp.Tag.Speed:
                AddToSpeed(10);
                StartCoroutine(EatLeaf(otherCollider.gameObject));
                break;
            case PowerUp.Tag.Size:
                AddToSize(10);
                StartCoroutine(EatLeaf(otherCollider.gameObject));
                break;
            case PowerUp.Tag.Camo:
                AddToCamo(10);
                StartCoroutine(EatLeaf(otherCollider.gameObject));
                break;
            case PowerUp.Tag.SafeZone:
                EnterSafeZoneState();
                break;
        }
    }

    private IEnumerator EatLeaf(GameObject gameObject)
    {
        gameObject.transform.parent.parent.GetComponent<PlayableDirector>().Play();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
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
        try
        {

        UIController.dangerText.text = "You are hidden!";
        //UIController.threatLevel.text = "ThreatLevel:0";
        }
        catch
        {

        }
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
            StartCoroutine(startInterpolatedColor(colors[colorIndex]));
        }
        currentColorIndex = colorIndex;
    }

    private void AddToSpeed(int amount)
    {

        playerStats.AddStats(Stats.Type.Speed, amount);
        UIController.speedText.text = "Speed:" + playerStats.speed.ToString();
        //movementController.climbSpeed = movementController.initialSpeed + playerStats.speed / 10;
        movementController.climbSpeed += 1;
    }

    private void AddToSize(int amount)
    {
        
        growingState = GrowingState.growing;
        playerStats.AddStats(Stats.Type.Size, amount);
        UIController.sizeText.text = "Size:" + playerStats.size.ToString();
        startInterpolatedGrowth();
    }

    private void startInterpolatedGrowth()
    {
        startingScale = scalingBody.transform.localScale;
        transitionStartTime = Time.time;
        float scaleFromSize = initialScale + (float)(playerStats.size) / scalingSpeed * initialScale;
        targetScale = new Vector3(scaleFromSize, scaleFromSize, scaleFromSize);
    }

    private void growInterpolate()
    {
        float fractionOfTransition = (Time.time - transitionStartTime) / growingTime;

        scalingBody.transform.localScale = Vector3.Lerp(
            startingScale,
            targetScale,
            fractionOfTransition);

        // Check if the fall is finished
    }
    IEnumerator startInterpolatedColor(Color newColor)
    {

        coroutinesRunning++;
        startingColor = currentColor;
        endingColor = newColor;
        timeColorProgressInterpolation = 0f;

        while (timeColorProgressInterpolation < colorChangingTime && coroutinesRunning <= 1)
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
        SetColorOfMaterials();
   }

    private void growthComplete()
    {
        growingState = GrowingState.stagnating;
    }
    public Vector3 targetPoint
    {
        get{
            //return transform.position + new Vector3(0, 0.5f, 0);
            return transform.position + new Vector3(0, 0, 0);
        }
    }

}
