using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class inherits from Enemy and adds unique characteristics to Nightmare mobs

public class Nightmare : Enemy
{
    public GameObject player;

    public override void DealDamage(float damage)
    {

        if (player.CompareTag("Player"))
        {
            // Player is in the Normal Dimension, no damage is dealt
            Debug.Log("Player is in the Normal Dimension. No damage is dealt.");
        }
        else if (player.CompareTag("Nightmare Player"))
        {
            // Player is in the Nightmare Dimension, normal damage is dealt
            base.DealDamage(damage);
        }
    }
}
