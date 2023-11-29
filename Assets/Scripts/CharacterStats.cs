using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia.ARFoundation;

public class CharacterStats : MonoBehaviour
{
    public float maxHealth = 20.0f; // Total health of a character
    public float attack = 5.0f; // Amount of possible damage to health of target character
    public float defense = 3.0f; // Amount of reduction from damage to health
    private float health; // Current health of a character
    static private GameObject win;
    static private GameObject lose;

    void Awake()
    {
        health = maxHealth;

        if (win == null)
        {
            win = GameObject.Find("WinMenu");
            lose = GameObject.Find("LoseMenu");
            win.SetActive(false);
            lose.SetActive(false);
        }
    }

    public void removeHealth(float amount)
    {
        if (amount > 0)
        {
            health -= amount;
        }

        if (health <= 0.0f)
        {
            // destroy
            // trigger something
            if (this.CompareTag("Player"))
            {
                //lose.active = true;
                lose.SetActive(true);
                Time.timeScale = 0; // Pause
            }
            else if (this.CompareTag("Enemy"))
            {
                //win.active = true;
                win.SetActive(true);
                Time.timeScale = 0; // Pause
            }
        }
    }

    public void addHealth(float amount)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public float getHealth()
    {
        return health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public float getAttack()
    {
        return attack;
    }

    public float getDefense()
    {
        return defense;
    }

    public void attackCharacter(CharacterStats targetCharacter, bool isSpecial)
    {
        if (targetCharacter != null) 
        {
            float damage = attack * (10 / (targetCharacter.defense + 10));

            if (isSpecial)
            {
                damage *= 3;
            }

            Debug.Log("Damage: " + damage.ToString());  
            targetCharacter.removeHealth(damage);
        }
    }
}
