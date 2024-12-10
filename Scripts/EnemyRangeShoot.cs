using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class inherits from EnemyAttack and represents the Range ability of enemies (With Assistance from ChatGPT)

public class EnemyRangeShoot : EnemyAttack
{
    public GameObject projectile;
    public float minDamage;
    public float maxDamage;
    public float projectileForce;
    public float angle;
    public float cooldown;
    public float attackDistance = 60f; // the distance at which the enemy starts shooting

    public override void Start()
    {
        base.Start();
        StartCoroutine(ShootPlayer());
    }

    public virtual IEnumerator ShootPlayer()
    {
        yield return new WaitForSeconds(cooldown);
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= attackDistance)
            {
                GameObject fireball = Instantiate(projectile, transform.position, Quaternion.identity);
                Debug.Log(player.transform.position);
                Vector2 myPosition = transform.position;
                Vector2 targetPosition = player.transform.position;
                Vector2 direction = (targetPosition - myPosition).normalized;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
                fireball.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                fireball.GetComponent<Rigidbody>().velocity = direction * projectileForce;
                fireball.GetComponent<EnemyProjectile>().damage = Random.Range(minDamage, maxDamage);
            }
            StartCoroutine(ShootPlayer());
        }
    }
}