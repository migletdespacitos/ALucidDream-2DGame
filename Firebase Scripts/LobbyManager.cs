using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Referenced from Tornadoally and xzippyzachx

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    [Header("UI References")]
    [SerializeField]
    private GameObject profileUI;
    [SerializeField]
    private SaveSlotsUI saveSlotsUI;
    [Space(5f)]

    [Header("Basic Info References")]
    [SerializeField]
    private TMP_Text usernameText;
    [SerializeField]
    private TMP_Text emailText;
    [Space(5f)]

    [Header("Lobby Buttons")]
    [SerializeField]
    private Button newGameButton;
    [SerializeField]
    private Button continueGameButton;
    [SerializeField]
    private Button loadGameButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Start()
    {
        if (FirebaseManager.instance.user != null)
        {
            LoadProfile();
        }

        while (DataPersistenceManager.instance.IsLoadingGame)
        {
            yield return null;
        }

        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }

    private void LoadProfile()
    {
        if (FirebaseManager.instance.user != null)
        {
            // Get Variables
            string name = FirebaseManager.instance.user.DisplayName;
            string email = FirebaseManager.instance.user.Email;

            // Set UI
            usernameText.text = name;
            emailText.text = email;
        }
    }

    public void ClearUI()
    {
        profileUI.SetActive(false);
        saveSlotsUI.gameObject.SetActive(false);
    }

    public void ProfileUI()
    {
        ClearUI();
        profileUI.SetActive(true);
        LoadProfile();
    }

    public void SignOutButton()
    {
        FirebaseManager.instance.SignOut();
    }

    public void OnNewGameClicked()
    {
        ClearUI();
        saveSlotsUI.ActivateMenu(false);
    }

    public void OnLoadGameClicked()
    {
        ClearUI();
        saveSlotsUI.ActivateMenu(true);
    }

    public void OnContinueGameClicked()
    {
        Debug.Log("Continue Game Clicked");
        DisableLobbyButtons();

        // save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();

        // Load the next scene - which will in turn load the game because of 
        // OnSceneLoaded() in the DataPersistenceManager
        GameManager.instance.ChangeScene(2);
    }

    private void DisableLobbyButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
}
