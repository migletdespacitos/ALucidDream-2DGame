using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Referenced from Trever Mock

public class SaveSlotsUI : MonoBehaviour
{
    [Header("SaveSlotsUI Buttons")]
    [SerializeField]
    private Button backButton;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        // Disable all buttons while scene is loading
        DisableMenuButtons();
        
        // update the selected save id to be used for data persistence
        StartCoroutine(DataPersistenceManager.instance.ChangeSelectedSaveId(saveSlot.GetSaveId(), callback =>
        {
            if (!isLoadingGame)
            {
                // create a new game - which will initialise our data to a clean slate
                DataPersistenceManager.instance.NewGame();
            }

            // save the game anytime before loading a new scene
            DataPersistenceManager.instance.SaveGame();

            // Load the scene
            DataPersistenceManager.instance.LoadSavedLevel();
        }));
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        // set this menu to be active
        this.gameObject.SetActive(true);

        // set mode
        this.isLoadingGame = isLoadingGame;
        
        // Load all of the profiles that exist
        StartCoroutine(DataPersistenceManager.instance.GetAllSavesGameData((loadedSaveDictionary) =>
        {
            // Loop through each save slot in the UI and set the content appropriately
            foreach (SaveSlot saveSlot in saveSlots)
            {
                GameData saveIdData = null;
                loadedSaveDictionary.TryGetValue(saveSlot.GetSaveId(), out saveIdData);
                saveSlot.SetData(saveIdData);

                if (saveIdData == null && isLoadingGame)
                {
                    saveSlot.SetInteractable(false);
                }
                else
                {
                    saveSlot.SetInteractable(true);
                }
            }
        }));
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
