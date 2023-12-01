using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class CharacterSelect : MonoBehaviour
{
    public GameObject selectedCharacter;
    public GameObject[] availableCharacterArray;
    private int selectedCharacterPosition;
    private CharacterStats characterStats;
    private TextMeshProUGUI characterStatsText;

    // Start is called before the first frame update
    void Start()
    {
        selectedCharacterPosition = 0;
        selectedCharacter = availableCharacterArray[selectedCharacterPosition];
        selectedCharacter.SetActive(true);

        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (text.name == "CharacterStats")
            {
                characterStatsText = text;
            }
        }

        updateCharacterStatsText();
    }

    /* Next character rotation on-screen button */
    public void onButtonPressed()
    {
        selectedCharacter.SetActive(false);

        int numberOfAvailableCharacters = availableCharacterArray.Length;

        selectedCharacterPosition++;

        if (selectedCharacterPosition >= numberOfAvailableCharacters)
        {
            selectedCharacterPosition = 0;
        }

        selectedCharacter = availableCharacterArray[selectedCharacterPosition];
        selectedCharacter.SetActive(true);

        updateCharacterStatsText();
    }

    /* Display character stats in the text object */
    private void updateCharacterStatsText()
    {
        float maxHealth;
        float attack;
        float defense;

        characterStats = selectedCharacter.GetComponent<CharacterStats>();

        maxHealth = characterStats.getMaxHealth();
        attack = characterStats.getAttack();
        defense = characterStats.getDefense();

        characterStatsText.text =   "Max Health: " + maxHealth.ToString().PadLeft(3,' ') + "\n" +
                                    "Attack: " + attack.ToString().PadLeft(4, ' ') + "\n" +
                                    "Defense: " + defense.ToString().PadLeft(4, ' ') + "\n";
    }
}
