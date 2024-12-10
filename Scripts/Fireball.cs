using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class represents the projectile mechanic for the player

public class Fireball : MonoBehaviour
{
    public GameObject projectile;
    public GameObject ultimateProjectile;  // Ultimate skill's projectile prefab
    public float minDamage;
    public float maxDamage;
    public float projectileForce;
    public float ultimateProjectileForce;  // Ultimate skill's projectile force
    public float angle;
    public PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && Time.timeScale == 1)
        {
            // Normal Attack
            FireProjectile(projectile, projectileForce, minDamage, maxDamage);
        }

        if (Input.GetKeyDown(KeyCode.Space) && playerStats.ultimateSkillSlider.value == playerStats.ultimateSkillSlider.maxValue && playerStats.CanUseUltimate() && Time.timeScale == 1)
        {
            // Ultimate Skill
            FireProjectile(ultimateProjectile, ultimateProjectileForce, maxDamage * 2, maxDamage * 3);
            playerStats.UseUltimate();  // Reset the Ultimate Skill Slider
        }
    }

    void FireProjectile(GameObject projectilePrefab, float force, float minDamage, float maxDamage) //With assistance from ChatGPT
    {
        GameObject fireball = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 myPosition = transform.position;
        Vector2 direction = (mousePosition - myPosition).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        fireball.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        fireball.GetComponent<Rigidbody>().velocity = direction * force;
        fireball.GetComponent<Projectile>().damage = Random.Range(minDamage, maxDamage);
    }
}
