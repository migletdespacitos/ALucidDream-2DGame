using System.Collections;
using UnityEngine;

//this class inherits from EnemySpawner and spawns Nightmare mobs specifically

public class NightmareSpawner : EnemySpawner
{
    public override IEnumerator SpawnEnemy()
    {
        while (counter < 10 && !stopSpawning)
        {
            counter++;
            Vector3 spawnPosition = new Vector3(Random.Range(-200, 600), Random.Range(-140, 90), 0);
            Instantiate(Enemies[0], spawnPosition, Quaternion.identity);
            Enemy.activeEnemies++;
            yield return new WaitForSeconds(spawnRate);
        }
        finishedSpawning = true;
    }
}
