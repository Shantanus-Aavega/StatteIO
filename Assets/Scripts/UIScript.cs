using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIScript : MonoBehaviour
{
    [SerializeField] GameObject rewardBtns, uiBtns, creditsUI, statesHolder, LevelTxt, pauseBtn, pauseUi;
    [SerializeField] LevelManager levelManager;

    void Start()
    {
        levelManager = GameObject.Find("LevelManager")?.GetComponent<LevelManager>();
    }

    void Update()
    {
        
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelManager.level);
    }

    public void ShowCredits()
    {
        if (!creditsUI.activeInHierarchy)
        {
            rewardBtns.SetActive(false);
            creditsUI.SetActive(true);
        }

        else
        {
            rewardBtns.SetActive(true);
            creditsUI.SetActive(false);
        }
    }

    public void TogglePause()
    {
        if (!pauseUi.activeInHierarchy)
        {
            pauseUi.SetActive(true);
            pauseBtn.SetActive(false);
            LevelTxt.SetActive(false);

            Time.timeScale = 0;

            statesHolder.SetActive(false);
        }

        else if (!pauseUi.activeInHierarchy)
        {
            pauseUi.SetActive(false);
            pauseBtn.SetActive(true);
            LevelTxt.SetActive(true);
            statesHolder.SetActive(true);

            Time.timeScale = 1;            
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
