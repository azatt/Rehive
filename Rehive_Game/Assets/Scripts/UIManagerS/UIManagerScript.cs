using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{
    public CanvasGroup canvasGroup; 
    public RectTransform Canvas, MenuPanel, OptionsPanel;
    public RectTransform OptionsButton, PlayButton, BackButton;
    public RectTransform GameTitle;
    
    float growDelay = 0.2f;
    float screenHeight, screenWidth;

    void Start()
    {
        Resolution screenRes = Screen.currentResolution;
        screenHeight = screenRes.height;
        screenWidth = screenRes.width;
        OptionsPanel.position = new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y - screenHeight, 0);
    }

    public void ButtonAnimation(RectTransform button)
    {
        button.DOScale(1.3f, 0.1f);
        button.DOScale(1, 0.1f).SetDelay(0.1f);
    }

    public void MainToOptions()
    {
        MenuPanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y + screenHeight, 0), 0.5f, false).SetDelay(growDelay);
        OptionsPanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y, 0), 0.5f, false).SetDelay(growDelay);
    }

    public void OptionsToMain()
    {
        OptionsPanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y - screenHeight, 0), 0.5f, false).SetDelay(growDelay);
        MenuPanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y, 0), 0.5f, false).SetDelay(growDelay);
    }

    public void StartGame()
    {
        PlayButton.DOScale(0.5f, 0.8f).SetDelay(growDelay);
        OptionsButton.DOScale(0.7f, 0.8f).SetDelay(growDelay);
        GameTitle.DOScale(0.7f, 0.8f).SetDelay(growDelay);
        canvasGroup.DOFade(0, 0.8f).SetDelay(growDelay);
        SceneManager.LoadScene(sceneName: "FinalScene");
    }
}
