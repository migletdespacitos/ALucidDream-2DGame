using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class represents the characteristics of the shards

public class Shards : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats.playerStats.CollectShards(); //accounts for shards collected
            Destroy(gameObject); //destroys object when it collides with player
        }
    }
}
