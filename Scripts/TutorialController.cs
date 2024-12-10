using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class represents the Tutorial cutscene in Level 1

public class TutorialController : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject tutorialCharacter;
    public Text tutorialText;
    public PauseManager pauseManager;

    private string[] tutorialSteps = { "You: Where am I? It feels like a dream. And who are you?", "Hooded Figure: There is no need for you to know who I am for now. I will simply tell you what you can do.", "You are able to move around this dream world. Use WASD to move around.", "Right-click to shoot projectiles.", "You: What projectile? I only have a carnation.","Hooded Figure: That is exactly what you will be using.", "Spacebar will trigger your special ability, which can only be used 8 times.", "Use your mouse to aim before firing the special ability.", "There will be shards around the map. Collect all of them.", "Enemies will come for you. Fight them off and survive if you wanna escape..."};

    private int currentStep = 0;

    public bool tutorialFinished = false;

    void Start()
    {
        tutorialCharacter.SetActive(false);
        Time.timeScale = 0; //Pauses the game at the start
        UpdateTutorialStep();
    }

    public void NextStepButton()
    {
        tutorialCharacter.SetActive(true); //character appears when tutorial starts
        currentStep++;
        if (currentStep >= tutorialSteps.Length)
        {
            tutorialCharacter.SetActive(false);
            tutorialPanel.SetActive(false); //Hides the tutorial panel
            Time.timeScale = 1; //Resumes the game
            tutorialFinished = true;
            PauseManager.canPause = true;
            pauseManager.enabled = true;
            
        }
        else
        {
            UpdateTutorialStep();
        }
    }

    private void UpdateTutorialStep()
    {
        tutorialText.text = tutorialSteps[currentStep];
    }
}
