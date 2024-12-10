using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this class represents Game Controller, which controls gameplay
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public EnemySpawner enemySpawner;
    public Text victoryText;
    public Text gameOverText;

    public GameObject shardsPrefab;
    public GameObject player;

    public PlayerMovement playerMovement;
    public Fireball projectile;

    private bool playerLost = false;

    public Button btnExit;
    public Button btnNextLevel;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        victoryText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        btnExit.gameObject.SetActive(false);
        btnNextLevel.gameObject.SetActive(false);
        playerMovement.enabled = true;
        projectile.enabled = true;
    }
        

    void Update()
    {
        if (!playerLost && enemySpawner.IsSpawningComplete() && GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && GameObject.FindGameObjectsWithTag("Shards").Length == 0)
        {
            EndLevel();
        }
    }

    void EndLevel()
    {
        GameObject[] destroyableObjects = GameObject.FindGameObjectsWithTag("Destroyable");
        foreach (GameObject obj in destroyableObjects)
        {
            Destroy(obj);
        }
        victoryText.gameObject.SetActive(true);
        StartCoroutine(EndGameWithDelay());
    }

    public void GameOver()
    {
        playerLost = true;
        enemySpawner.StopSpawning();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        GameObject[] destroyableObjects = GameObject.FindGameObjectsWithTag("Destroyable");
        foreach (GameObject obj in destroyableObjects)
        {
            Destroy(obj);
        }
        gameOverText.gameObject.SetActive(true);
        btnExit.gameObject.SetActive(true);
    }

    IEnumerator EndGameWithDelay()
    {
        yield return new WaitForSeconds(1);
        if (player != null)
        {
            playerMovement.enabled = false;
            projectile.enabled = false;
        }
        if (player != null)
        {
            player.SetActive(false);
        }
        btnExit.gameObject.SetActive(true);
        btnNextLevel.gameObject.SetActive(true);
    }

    public void OnClickNextLevel2()
    {
        DataPersistenceManager.instance.SaveGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 2 Scene");
    }

    public void OnClickNextLevel3()
    {
        DataPersistenceManager.instance.SaveGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 3 Scene");
    }

    public void OnClickNextLevel4()
    {
        DataPersistenceManager.instance.SaveGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Final Boss Scene");
    }

    public void OnClickNextLevel5()
    {
        DataPersistenceManager.instance.SaveGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Final Scene");
    }
}