using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Battle : MonoBehaviour
{
    //Character c;// get from CharacterSelect script - public selectedCharacter
    static public GameObject character;
    private bool isFighting = false;
    private VirtualButtonBehaviour buttonBehaviour;

    // Start is called before the first frame update
    private void Awake()
    {
        buttonBehaviour = GetComponentInChildren<VirtualButtonBehaviour>();
        buttonBehaviour.RegisterOnButtonPressed(OnButtonPressed);

        if (character == null)
        {
            character = GameObject.Instantiate(GameObject.Find("CharacterSelectMenu").GetComponent<CharacterSelect>().selectedCharacter); // Duplicate character
            character.layer = LayerMask.NameToLayer("Default"); // Set layer to Default (instead of UI)
            character.transform.parent = transform;
            character.transform.localScale = new Vector3(.001f,.001f,.001f);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("--OnButtonPressed: " + vb.VirtualButtonName);

        Debug.Log(character.name);

        isFighting = !isFighting;

        Vector2 imageDimensions = this.GetComponent<ImageTargetBehaviour>().GetSize();
        float x = imageDimensions.x;
        float y = imageDimensions.y;
        this.transform.position = new Vector3(x/2, this.transform.position.y, y/2);
        character.transform.position = new Vector3(x + x/2, character.transform.position.y, y + y/2);
    }

    // Update is called once per frame
    void Update()
    {
        //if (isFighting) {
            //Time t = now
            // if (t == t+.5 seconds) {
            //  enemyAttack
            //}
        //}
    }
}
