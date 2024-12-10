using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class represents the characteristics of the player's projectile (Referenced to DapperDino Creating Game Mechanics video)

public class Projectile : MonoBehaviour
{
    public float damage;
    public float range = 10f; // The maximum distance the projectile can travel

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(initialPosition, transform.position) >= range)
        {
            Destroy(gameObject); //destroys projectile when it is out of range
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name != "Player")
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<Enemy>().DealDamage(damage); //deals damage if it collides with Enemies
            }
            Destroy(gameObject); //destroys projectile when it collides with any object that is not the player
        }
    }
}
