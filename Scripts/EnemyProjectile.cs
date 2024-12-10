using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class represents the Enemy Projectile

public class EnemyProjectile : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag != "Enemy")
        {
            if (collision.tag == "Player" | collision.tag == "Nightmare Player")
            {
                PlayerStats.playerStats.DealDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
