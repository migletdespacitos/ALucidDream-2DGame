using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this class represents the cutscene for level 3

public class Level3Cut : MonoBehaviour
{
    public GameObject levelPanel;
    public GameObject levelCharacter;
    public Text levelText;

    private string[] levelSteps = { "You: This place is starting to feel familliar.", "I think I know where I am now. I am stuck in my own dream.", "Hooded Figure: You are right. Except now, the Nightmare begins.", "You: What do you mean?", "Hooded Figure: A new kind of enemy is coming for you. However, it is in another dimension.", "It can attack you from anywhere, but you cannot. You must attune in order to attack it.", "Use Q to switch dimensions.", "Once you are in the Nightmare dimension, you can attack these special enemies. The normal enemies will not be able to attack you in this dimension.", "You: What? Can I just wake up from this dream now?", "Hooded Figure: No. Escape the Nightmare and you may just be able to escape.", "Thanks to the experience points you have gotten in the previous level, your ultimate skill will be upgraded.", "I'll see you again soon."};

    private int currentStep = 0;

    void Start()
    {
        levelCharacter.SetActive(false);
        Time.timeScale = 0; //Pauses the game at the start
        UpdateStep();
    }

    public void NextStepButton()
    {
        levelCharacter.SetActive(true); //character appears when cutscene starts
        currentStep++;
        if (currentStep >= levelSteps.Length)
        {
            levelCharacter.SetActive(false);
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

