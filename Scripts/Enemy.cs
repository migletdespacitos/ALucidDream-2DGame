using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class represents the basic Enemy attributes (Referenced to DapperDino Creating Game Mechanics video for Enemy health and damage logic)

public class Enemy : MonoBehaviour
{
    //components
    public float health;
    public float maxHealth;

    public GameObject healthBar;
    public Slider healthBarSlider;

    public static int activeEnemies = 0;

    public int expValue = 1;

    // Start is called before the first frame update
    public virtual void Start()
    {
        health = maxHealth;
        activeEnemies++;
    }

    public virtual void DealDamage(float damage)
    {
        healthBar.SetActive(true);
        health -= damage;
        CheckDeath();
        healthBarSlider.value = CalculateHealthPercentage();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            activeEnemies--;
            PlayerStats.playerStats.IncreaseSkill(0.2f);
            PlayerStats.playerStats.AddExp(expValue);
        }
    }

    private float CalculateHealthPercentage()
    {
        return (health / maxHealth);
    }
}