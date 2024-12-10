using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class represents the EnemyAttack Mechanism (Witn Reference to Unity Game Development Cookbook Essentials and assistance from ChatGPT)

public class EnemyAttack : MonoBehaviour
{
    public GameObject player;
    public float speed = 10f;
    public float stopDistance = 60f; // The distance at which the enemy will stop moving towards the player
    public float avoidanceForce = 30f;

    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > stopDistance)
            {
                // Move towards player
                rb.MovePosition(Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime));
            }

            // Detect nearby enemies
            Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, 8f);

            // Apply a repulsion force to avoid each other
            foreach (Collider enemy in nearbyEnemies)
            {
                if (enemy.gameObject != gameObject) // Exclude self
                {
                    Vector3 avoidDirection = transform.position - enemy.transform.position;
                    rb.AddForce(avoidDirection.normalized * avoidanceForce);
                }
            }
        }
        FlipSprite();
    }

    void FlipSprite()
    {
        if (player.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
