using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class inherits from EnemySpawner and allows to spawn more mobs in level 2

public class EnemySpawner2 : EnemySpawner
{
   public override IEnumerator SpawnEnemy()
    {
        while (counter < 60 & !stopSpawning)
        {
            counter += 1;
            x = Random.Range(-200, 600);
            y = Random.Range(-140, 90);
            spawnPosition.x = x;
            spawnPosition.y = y;

            // Spawn a different enemy based on whether the counter is even or odd.
            int enemyIndex = counter % 2;
            Instantiate(Enemies[enemyIndex], spawnPosition, Quaternion.identity);

            Enemy.activeEnemies++;
            yield return new WaitForSeconds(spawnRate);
        }
        finishedSpawning = true; //spawner has finished spawning enemies
    }
}