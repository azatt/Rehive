using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StatsController : MonoBehaviour
{
    // Start is called before the first frame update
    public Stats playerStats;
    [SerializeField] float transitionStartTime;
    [SerializeField] float growingTime = 2;
    [SerializeField] float colorChangingTime = 2;
    [SerializeField] Vector3 startingScale;
    [SerializeField] Vector3 targetScale;
    [SerializeField] GameObject camoBody;
    [SerializeField] GameObject scalingBody;
    [SerializeField] Color[] colors;
    [SerializeField] Material[] materials;

    public float distanceGrowth = 0.15f;
    public static float globalThreatLevel, camoValue, sizeValue, speedValue;
    public enum GrowingState { growing, stagnating }
    public enum DangerState { hidden, safeZone, danger, waitingForUpdate, death }

    GrowingState growingState;
    public float beforeSizeTimesInitialSize;
    private float initialSize;
    public DangerState dangerState;
    private Climb movementController;
    private ProceduralAnim bodyController;
    public UIController UIController;
    private Color endingColor;
    public float timeColorProgressInterpolation;
    private Color startingColor;
    public Color currentColor;
    public int currentColorIndex;
    public int coroutinesRunning;
    public float initialScale;
    public GameObject collObj;
    public float sizeThresholdDoubleSize = 40;
    public int threatCount;
    public float totalThreatLevel;
    public float threatLevelRate;
    public float reductionRate = 0.4f;
    private float endSize;
    public float afterSizeTimesInitialSize;
    public float fractionOfTransitionSize;
    public float reductionNettoRate;

    [SerializeField] private AudioClip eatSoundEffect;

    void Start()
    {
        playerStats = new Stats(0, 0, 0);
        growingState = GrowingState.stagnating;
        dangerState = DangerState.danger;

        movementController = GetComponent<Climb>();
        bodyController = FindObjectOfType<ProceduralAnim>();
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
        //totalThreatLevel *= Mathf.Pow(0.90f, Time.deltaTime);
        totalThreatLevel += threatLevelRate * Time.deltaTime * (1 - (float)playerStats.camo/150f * 0.6f);
        if(totalThreatLevel > 0)
        {
            reductionNettoRate= reductionRate;
            if(dangerState == DangerState.safeZone)
            {
                reductionNettoRate *= 4;
            }
            else if (threatCount == 0)
            {

                reductionNettoRate *= 1.5f;
            }

            totalThreatLevel -= reductionNettoRate* Time.deltaTime;
        }
        //UIController.threatLevel.text = "Count: " + threatCount.ToString() + " ThreatLevel:" + totalThreatLevel.ToString();
        globalThreatLevel = totalThreatLevel;
        if(dangerState == DangerState.death)
        {
            globalThreatLevel = 11;
        }
    }

    private IEnumerator CheckDangerState()
    {
        while (true)
        {

            if (threatCount > 0 && dangerState != DangerState.safeZone && dangerState != DangerState.death)
            {
                EnterDangerState();
            }
            else if(dangerState != DangerState.safeZone && dangerState != DangerState.death)
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
        AudioManager.Instance.PlaySoundEffect(eatSoundEffect, 1);
        gameObject.transform.parent.parent.GetComponent<PlayableDirector>().Play();
        gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if(dangerState == DangerState.death) { return; }
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
        //UIController.dangerText.text = "You are in DANGER!";
    }
    public void EnterHiddenState()
    {
        dangerState = DangerState.hidden;
        try
        {

            //UIController.dangerText.text = "You are hidden!";
            //UIController.threatLevel.text = "ThreatLevel:0";
        }
        catch
        {

        }
    }


    public void EnterSafeZoneState()
    {
        if(dangerState == DangerState.death) { return; }
        dangerState = DangerState.safeZone;
        //UIController.dangerText.text = "You are in a safezone!";

    }

    private void AddToCamo(int amount)
    {
        playerStats.AddStats(Stats.Type.Camo, (amount*10)/12);
        //UIController.camoText.text = "Camo:" + playerStats.camo.ToString();
        camoValue = playerStats.camo;
        int colorIndex = Mathf.Clamp(playerStats.camo / 10, 0, 12);

        if (colorIndex > currentColorIndex)
        {
            StartCoroutine(startInterpolatedColor(colors[colorIndex]));
        }
        currentColorIndex = colorIndex;
    }

    private void AddToSpeed(int amount)
    {

        playerStats.AddStats(Stats.Type.Speed, amount);
        //UIController.speedText.text = "Speed:" + playerStats.speed.ToString();
        speedValue = playerStats.speed;
        bodyController.minDistance -= distanceGrowth / 2;
        //movementController.climbSpeed = movementController.initialSpeed + playerStats.speed / 10;
        movementController.climbSpeed += 1;
    }

    private void AddToSize(int amount)
    {

        growingState = GrowingState.growing;
        beforeSizeTimesInitialSize = 1 + playerStats.size / sizeThresholdDoubleSize;
        playerStats.AddStats(Stats.Type.Size, amount);
        afterSizeTimesInitialSize = 1 + playerStats.size / sizeThresholdDoubleSize;
        //UIController.sizeText.text = "Size:" + playerStats.size.ToString();
        sizeValue = playerStats.size;
        startInterpolatedGrowth();
    }

    private void startInterpolatedGrowth()
    {
        startingScale = scalingBody.transform.localScale;
        distanceGrowth += 0.1f;
        bodyController.minDistance += distanceGrowth;
        transitionStartTime = Time.time;
        float amountTimesInitialSize = playerStats.size / sizeThresholdDoubleSize +1;
        float scaleFromSize =  amountTimesInitialSize * initialScale;
        targetScale = new Vector3(scaleFromSize, scaleFromSize, scaleFromSize);
    }

    private void growInterpolate()
    {
        fractionOfTransitionSize = (Time.time - transitionStartTime) / growingTime;

        scalingBody.transform.localScale = Vector3.Lerp(
            startingScale,
            targetScale,
            fractionOfTransitionSize);

        float currentSizeTimesInitialSize = beforeSizeTimesInitialSize + (afterSizeTimesInitialSize - beforeSizeTimesInitialSize) * fractionOfTransitionSize;
        bodyController.SetOffsetFromTreeFromSize(currentSizeTimesInitialSize); 
        if(fractionOfTransitionSize > 1)
        {
            growingState = GrowingState.stagnating;
            currentSizeTimesInitialSize = afterSizeTimesInitialSize;
            bodyController.SetOffsetFromTreeFromSize(currentSizeTimesInitialSize); 

        }
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
        get
        {
            //return transform.position + new Vector3(0, 0.5f, 0);
            return transform.position + new Vector3(0, 0, 0);
        }
    }

}
