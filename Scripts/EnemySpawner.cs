using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class represents the enemy spawner (Referenced to DapperDino Creating Game Mechanics video)

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();
    public float spawnRate;

    protected float x, y;
    protected Vector3 spawnPosition;
    protected int counter = 0;

    public static bool finishedSpawning = false;
    protected bool stopSpawning = false;

    public virtual void Start()
    {
        finishedSpawning = false; //Reset when a new level starts
        StartCoroutine(SpawnEnemy());
    }

    public virtual IEnumerator SpawnEnemy()
    {
        while (counter < 30 & !stopSpawning)
        {
            counter += 1;
            x = Random.Range(-200, 600);
            y = Random.Range(-140, 90);
            spawnPosition.x = x;
            spawnPosition.y = y;
            Instantiate(Enemies[0], spawnPosition, Quaternion.identity);
            Enemy.activeEnemies++;
            yield return new WaitForSeconds(spawnRate);
        }
        finishedSpawning = true; //spawner has finished spawning enemies
    }

    public void StopSpawning()
    {
        stopSpawning = true;
    }

    public bool IsSpawningComplete()
    {
        return finishedSpawning;
    }
}