using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

//Referenced from Trever Mock
public class DataPersistenceManager : MonoBehaviour
{    
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;

    private string selectedSaveId = "test";

    public static DataPersistenceManager instance { get; private set; }

    public bool IsLoadingGame { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded Called");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        StartCoroutine(LoadGame());
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded Called");
        SaveGame();
    }

    public IEnumerator ChangeSelectedSaveId(string newSaveId, System.Action<int> callback)
    {
        // update the profile to use for saving and loading
        this.selectedSaveId = newSaveId;

        // Load the game, which will use that saveId, updating our game data accordingly
        yield return StartCoroutine(LoadGame());

        callback(1);
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public IEnumerator LoadGame()
    {
        IsLoadingGame = true;

        while (FirebaseManager.instance.IsSavingGame)
        {
            yield return null;
        }
        
        // Load any saved data from a file using the data handler;
        StartCoroutine(FirebaseManager.instance.LoadGameData(selectedSaveId, (loadedGameData) =>
        {
            this.gameData = loadedGameData;

            // if no data can be loaded, don't continue
            if (this.gameData == null)
            {
                Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
                IsLoadingGame = false;
                return;
            }

            Debug.Log("Pushing loaded data to all other scripts that need it");
            // push the loaded data to all other scripts that need it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }

            Debug.Log("Shards collected: " + this.gameData.shardsCollected + "\nExp earned: " + this.gameData.expEarned);
            IsLoadingGame = false;
        }));
    }

    public void SaveGame()
    {
        // if we don't have any data to save, Log a warning here
        if (this.gameData == null)
        {
            Debug.LogWarning("No Data was found. A New Game needs to be started before data can be saved.");
            return;
        }
        
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        // save that data to a file using the data handler
        StartCoroutine(FirebaseManager.instance.UpdateGameData(selectedSaveId, gameData));

        Debug.Log("Shards collected: " + this.gameData.shardsCollected + "\nExp earned: " + this.gameData.expEarned);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return this.gameData != null;
    }

    public IEnumerator GetAllSavesGameData(System.Action<Dictionary<string, GameData>> callback)
    {
        Dictionary<string, GameData> saveDictionary = new Dictionary<string, GameData>();

        yield return StartCoroutine(FirebaseManager.instance.LoadAllSaves((loadedSaveDictionary) =>
        {
            saveDictionary = loadedSaveDictionary;
        }));

        callback(saveDictionary);
    }

    public void LoadSavedLevel()
    {
        GameManager.instance.ChangeScene(this.gameData.currentScene);
    }
}
