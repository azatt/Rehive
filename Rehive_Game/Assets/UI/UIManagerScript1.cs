using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManagerScript1 : MonoBehaviour
{
    public GameObject PauseMenu;
    public RectTransform Canvas, PausePanel, OptionsPanel, GamePanel;
    float growDelay = 0.2f;
    float screenHeight, screenWidth;

    void Start()
    {
        Resolution screenRes = Screen.currentResolution;
        screenHeight = screenRes.height;
        screenWidth = screenRes.width;
        PausePanel.position = new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y + screenHeight, 0);
        OptionsPanel.position = new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y - screenHeight, 0);
    }

    public void ButtonAnimation(RectTransform button)
    {
        button.DOScale(1.3f, 0.1f);
        button.DOScale(1, 0.1f).SetDelay(0.1f);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(sceneName: "MainMenu");
    }

    public void PauseGame()
    {
        PausePanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y, 0), 0.5f, false).SetDelay(growDelay);
        GamePanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y + screenHeight, 0), 0.5f, false).SetDelay(growDelay);
        //TO DO: disable player movement and all other things that don't stop moving with the above
    }

    public void ReturnGame()
    {
        PausePanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y + screenHeight, 0), 0.5f, false).SetDelay(growDelay);
        GamePanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y, 0), 0.5f, false).SetDelay(growDelay);
        //TO DO: enable player movement and all other things that have been disabled
    }

    public void PauseToOptions()
    {
        PausePanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y + screenHeight, 0), 0.5f, false).SetDelay(growDelay);
        OptionsPanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y, 0), 0.5f, false).SetDelay(growDelay);
    }

    public void OptionsToPause()
    {
        OptionsPanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y - screenHeight, 0), 0.5f, false).SetDelay(growDelay);
        PausePanel.DOMove(new Vector3(Canvas.anchoredPosition.x, Canvas.anchoredPosition.y, 0), 0.5f, false).SetDelay(growDelay);
    }
}
