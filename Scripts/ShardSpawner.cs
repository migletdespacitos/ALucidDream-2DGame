using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class represents the Shard Spawner

public class ShardSpawner : MonoBehaviour
{
    public static ShardSpawner shardSpawner;

    public GameObject shardPrefab;
    public float spawnDelay = 10f;
    public int maxShards = 10;
    private int shardsSpawned = 0;

    private void Awake()
    {
        shardSpawner = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnShardWithDelay());
    }

    private IEnumerator SpawnShardWithDelay()
    {
        while (shardsSpawned < maxShards)
        {
            SpawnShard();
            shardsSpawned++;
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void SpawnShard()
    {
        if (shardsSpawned <= maxShards)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-200, 600), Random.Range(-140, 90), 0); //randomly decides the coordinates for shards to spawn
            Instantiate(shardPrefab, randomPosition, Quaternion.identity);
        }
    }
}
