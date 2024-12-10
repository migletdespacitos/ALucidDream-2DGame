using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class controls the cutscene for Level 2

public class Level2Cut : MonoBehaviour
{
    public GameObject levelPanel;
    public GameObject levelCharacter;
    public Text levelText;

    private string[] levelSteps = { "You: I can feel you...what exactly do you want?", "Hooded Figure: I am here to guide you. You do want to escape right?.", "Your journey will get tougher. You do remember the shards that you collected?", "You: Well...yes. I don't know what it does, though.", "Hooded Figure: It is called the Shards of Reflection. You need those to build the portal back to reality.", "I do congratulate you for defeating the enemies before. You would have gotten stronger from that.", "You: I suppose so.", "Hooded Figure: I shall now grant you a new projectile.", "You can now shoot out arrows, which deals much more damage.", "You:Thanks. Anyways, just tell me how to rebuild the portal.", "Hooded Figure: Not so fast. We will see each other again. Now, just survive and find those shards..." };

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

