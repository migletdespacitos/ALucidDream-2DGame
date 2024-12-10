using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this class represents Player Stats (Referenced to DapperDino Creating Game Mechanics video on Player Stats)

public class PlayerStats : MonoBehaviour, IDataPersistence
{
    public static PlayerStats playerStats;

    public GameObject player;
    public Text healthText;
    public Slider healthSlider;
    public Slider ultimateSkillSlider;
    public GameController gameController;

    public float health;
    public float maxHealth;
    public float heal;
    public bool isAlive = true;

    public float healingThreshold = 30f;
    public float healingRate = 5f;

    public int exp = 0;

    public Text expValue;

    public int maxUltimateUses = 8;
    private int ultimateUses = 0;

    public Text skillsUsed;

    public Text shardsCollectedText;
    public int shardsCollected = 0;

    public int currentScene = 2;


    void Awake()
    {
        if (playerStats != null)
        {
            Destroy(playerStats);
        }
        else
        {
            playerStats = this;
        }
    }

    // Method that IDataPersistence interface requires

    public void LoadData(GameData data)
    {
        //Referenced from Trever Mock
        this.shardsCollected = data.shardsCollected;
        this.exp = data.expEarned;
        this.currentScene = data.currentScene;
    }

    // Method that IDataPersistence interface requires
    public void SaveData(GameData data)
    {
        //Referenced from Trever Mock
        data.shardsCollected = this.shardsCollected;
        data.expEarned = this.exp;

        if (GameManager.instance.GetCurrentScene() >= 2)
        {
            data.currentScene = GameManager.instance.GetCurrentScene();
        }   
    }

    void Start()
    {
        health = maxHealth;
        SetHealthUI();
        ultimateSkillSlider.value = 0;
    }

    void Update()
    {
        if (isAlive && health < healingThreshold)
        {
            HealCharacter();
        }
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        CheckDeath();
        SetHealthUI();
    }

    public void HealCharacter()
    {
        health += healingRate * Time.deltaTime;
        CheckOverheal();
        SetHealthUI();
    }

    private void SetHealthUI()
    {
        healthSlider.value = CalculateHealthPercentage();
        healthText.text = Mathf.Ceil(health).ToString() + " / " + Mathf.Ceil(maxHealth).ToString();
    }

    private void CheckOverheal()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            isAlive = false;
            Destroy(player);
            health = 0;
            gameController.GameOver();
        }
    }

    float CalculateHealthPercentage()
    {
        return health / maxHealth;
    }

    public void AddExp(int amount)
    {
        exp += amount;
        expValue.text = "EXP: " + exp.ToString();
        Debug.Log("Player EXP: " + exp);
    }

    public void IncreaseSkill(float increaseAmount)
    {
        ultimateSkillSlider.value += increaseAmount;
        if (ultimateSkillSlider.value > ultimateSkillSlider.maxValue)
        {
            ultimateSkillSlider.value = ultimateSkillSlider.maxValue;
        }
    }

    public bool CanUseUltimate()
    {
        return ultimateUses < maxUltimateUses;
    }

    public void UseUltimate()
    {
        if (CanUseUltimate())
        {
            ultimateSkillSlider.value = 0;
            ultimateUses += 1;
            skillsUsed.text = $"Ultimate Skill Used {ultimateUses}/8";
        }
    }

    public void CollectShards()
    {
        shardsCollected++;
        shardsCollectedText.text = "Shards of Reflection: " + shardsCollected.ToString();
        Debug.Log("Shards collected!");
    }
}
