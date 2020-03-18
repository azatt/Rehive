using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToWinningScene : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    private void Update()
    {
        canvasGroup.DOFade(1, 1f);
        GoToWinning();
    }

    IEnumerator LoadWinning()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameWin");
    }

    void GoToWinning()
    {
        StatsController.globalThreatLevel = 0;
        StatsController.speedValue = 0;
        StatsController.camoValue = 0;
        StatsController.sizeValue = 0;
        StartCoroutine(LoadWinning());
    }
}
