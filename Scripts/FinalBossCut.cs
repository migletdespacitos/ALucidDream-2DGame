using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this class represents the cutscene for the Boss level

public class FinalBossCut : MonoBehaviour
{
    public GameObject levelPanel;
    public GameObject levelCharacter;
    public GameObject boss;
    public Text levelText;

    private string[] levelSteps = {"You: I have finally defeated all the enemies. I have also collected the shards. Can I finally wake up now?", "Hooded Figure: What? You really think I would allow you to do so that easily?", "You: You said I could escape!", "Hooded Figure: I could not escape this place...and I will not allow anyone to do so either", "You: What do you mean?", "Hooded Figure: I was just like you. Except, the Nightmares engulfed me and turned me into this!", "You: You look almost just like me.", "Boss: Yes, and I have become far more powerful than you can ever imagine.", "I stand between you and reality. Defeat me and escape.", "I can split my soul into several pieces and attack you all at once!", "You: I will defeat you! I have gotten much stronger and my new weapon, the fireball will help me escape!", "Boss: You will try..."};

    private int currentStep = 0;

    void Start()
    {
        levelCharacter.SetActive(true);
        Time.timeScale = 0; //Pauses the game at the start
        UpdateStep();
    }

    public void NextStepButton()
    {
        currentStep++;
        if (currentStep == 6)
        {
            levelCharacter.SetActive(false);
            boss.SetActive(true);
        }
        else if (currentStep >= levelSteps.Length)
        {
            boss.SetActive(false);
            levelPanel.SetActive(false); //Hides the panel
            Time.timeScale = 1; //Resumes the game
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
