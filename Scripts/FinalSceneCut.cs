using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this class represents the final cutscene

public class FinalSceneCut : MonoBehaviour
{
    public GameObject levelPanel;
    public GameObject player;
    public GameObject boss;
    public Text levelText;
    public Text endingText;

    public Button btnExit;

    private string[] levelSteps = { "You: It is over. I have defeated you.", "Boss: Good for you. You are indeed the strongest to have entered this dream.", "You: What will you do now?", "Boss: I will finally be set free, just like you.", "You: How long have you been stuck in this dream?", "Boss: I entered this dream in 1989. I have no idea how long I have been in here.", "You: It is 2023. It has been 34 long years.", "Boss: I guess I will be coming back to a very different world.", "You: When we get back to reality, I will come and find you.", "Boss: That would be nice.", "You: I will see you soon." };
    private int currentStep = 0;

    void Start()
    {
        player.SetActive(true);
        boss.SetActive(true);
        btnExit.gameObject.SetActive(false);
        endingText.gameObject.SetActive(false);
        Time.timeScale = 0; //Pauses the game at the start
        UpdateStep();
    }

    public void NextStepButton()
    {
        currentStep++;
        if (currentStep >= levelSteps.Length)
        {
            boss.SetActive(false);
            player.SetActive(false);
            levelPanel.SetActive(false); //Hides the panel
            btnExit.gameObject.SetActive(true);
            endingText.gameObject.SetActive(true);
        }
        else
        {
            UpdateStep();
        }
    }

    private void UpdateStep()
    {
        levelText.text = levelSteps[currentStep];
    }
}
