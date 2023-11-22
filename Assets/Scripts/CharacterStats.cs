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
            Debug.Log("Win " + win == null ? "Found" : "Not Found");
            win.SetActive(false);
            lose = GameObject.Find("LoseMenu");
            Debug.Log("Lose " + lose == null ? "Found" : "Not Found");
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
                win.active = true;
            }
            else if (this.CompareTag("Enemy"))
            {
                //win.active = true;
                lose.active = true;
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

    public void attackCharacter(CharacterStats targetCharacter)
    {
        if (targetCharacter != null) 
        {
            targetCharacter.removeHealth(attack * (1/targetCharacter.defense));
        }
    }
}
