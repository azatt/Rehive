using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManagerScript2 : MonoBehaviour
{
    public CanvasGroup canvasGroup, canvasFade;
    public RectTransform RetryButton, MainMenuButton, GameOver;
    float growDelay = 0.2f;

    void Awake()
    {
        canvasFade.DOFade(0, 1f);
    }

    public void ButtonAnimation(RectTransform button)
    {
        button.DOScale(1.3f, 0.1f);
        button.DOScale(1, 0.1f).SetDelay(0.1f);
    }
    public void ReturnToGame()
    {
        FadeMenu();
        StartCoroutine(LoadGame());
    }
    public void GoToMainMenu()
    {
        FadeMenu();
        StartCoroutine(LoadMenu());
    }

    void FadeMenu()
    {
        RetryButton.DOScale(0.5f, 0.8f).SetDelay(growDelay);
        MainMenuButton.DOScale(0.7f, 0.8f).SetDelay(growDelay);
        GameOver.DOScale(0.7f, 0.8f).SetDelay(growDelay);
        canvasGroup.DOFade(0, 0.8f).SetDelay(growDelay);
        canvasFade.DOFade(1, 0.4f).SetDelay(0.6f);
    }


    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName: "MainMenu");
    }
    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName: "FinalScene");
    }
}
