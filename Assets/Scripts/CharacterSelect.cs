using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public GameObject selectedCharacter;
    public GameObject[] availableCharacterArray;
    private int selectedCharacterPosition;
    // Start is called before the first frame update
    void Start()
    {
        selectedCharacterPosition = 0;
        selectedCharacter = availableCharacterArray[selectedCharacterPosition];
        selectedCharacter.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedCharacter != null) { }
    }

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
    }
}
