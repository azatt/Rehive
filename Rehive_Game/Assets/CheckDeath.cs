﻿using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckDeath : MonoBehaviour
{
    public GameObject target;
    public GameObject oldTragets;
    public CanvasGroup canvasGroup;
    public StatsController statusPlayer;

    void Start()
    {

        statusPlayer = GetComponent<StatsController>();
    }
    void Update()
    {
        CheckThreat();
    }

    protected void CheckThreat()
    {
        if (StatsController.globalThreatLevel > 10)
        {
            target.SetActive(true);
            oldTragets.SetActive(false);
            CheckBirdDistance();
            statusPlayer.dangerState = StatsController.DangerState.death;
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
            if(Vector3.Distance(transform.position,bird.transform.position) < 0.5f)
            {
                canvasGroup.DOFade(1, 2f).SetDelay(1);
                GoToGameOver();
            }
        }
    }

    IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameOver");
    }
    void GoToGameOver()
    {
        StatsController.globalThreatLevel = 0;
        StatsController.speedValue = 0;
        StatsController.camoValue = 0;
        StatsController.sizeValue = 0;
        StartCoroutine(LoadGameOver());
    }
}
