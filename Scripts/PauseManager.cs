using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this class represents the mechanics controlling the Pause Menu (With assistance from ChatGPT)

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public PlayerMovement playerMovement;
    public PlayerStats playerStats;
    public static bool isPaused = false;
    public static bool canPause = false;


    void Start()
    {
        this.enabled = false;
    }

    void Update()
    {
        if (!canPause) return;

        if (isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        playerMovement.enabled = true;
        playerStats.enabled = true;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        playerMovement.enabled = false;
        playerStats.enabled = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}