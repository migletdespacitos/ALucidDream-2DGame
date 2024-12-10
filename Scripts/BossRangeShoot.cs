using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class inherits from EnemyRangeShoot and represents the Boss' ability to shoot projectiles

public class BossRangeShoot : EnemyRangeShoot
{
    public float dualProjectilesOffset = 1f; // Offset between the two projectiles

    public override IEnumerator ShootPlayer()
    {
        yield return new WaitForSeconds(cooldown);
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= attackDistance)
            {
                // Fire the first projectile
                GameObject projectile1 = Instantiate(projectile, transform.position, Quaternion.identity);
                FireProjectile(projectile1);

                // Calculate the offset position for the second projectile
                Vector3 offset = transform.right * dualProjectilesOffset;

                // Fire the second projectile with offset position
                GameObject projectile2 = Instantiate(projectile, transform.position + offset, Quaternion.identity);
                FireProjectile(projectile2);
            }
            StartCoroutine(ShootPlayer());
        }
    }

    private void FireProjectile(GameObject projectile)
    {
        Vector2 myPosition = transform.position;
        Vector2 targetPosition = player.transform.position;
        Vector2 direction = (targetPosition - myPosition).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        projectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        projectile.GetComponent<Rigidbody>().velocity = direction * projectileForce;
        projectile.GetComponent<EnemyProjectile>().damage = Random.Range(minDamage, maxDamage);
    }
}
