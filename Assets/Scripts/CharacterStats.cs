using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float maxHealth = 20.0f; // Total health of a character
    public float attack = 5.0f; // Amount of possible damage to health of target character
    public float defense = 3.0f; // Amount of reduction from damage to health
    private float health; // Current health of a character
    private CharacterStats targetCharacter;

    void Awake()
    {
        health = maxHealth;
    }

    public void removeHealth(float amount)
    {
        health -= amount;

        if (health <= 0.0f)
        {
            // destroy
            // trigger something
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

    public void attackCharacter()
    {
        if (targetCharacter != null) 
        {
            targetCharacter.removeHealth(attack - targetCharacter.defense);
        }
    }

    public void setTargetCharacter(GameObject target)
    {
        targetCharacter = target.GetComponent<CharacterStats>();
    }
}
