using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Battle : MonoBehaviour
{
    static public GameObject character;
    private GameObject enemy = null;
    static private bool isFighting = false;
    static private GameObject currentEnemy = null;
    private float imageTargetDimensionX;
    private float imageTargetDimensionY;

#pragma warning disable CS0618 // Type or member is obsolete
    private VirtualButtonBehaviour buttonBehaviour;

    // Start is called before the first frame update
    private void Awake()
    {
        buttonBehaviour = GetComponentInChildren<VirtualButtonBehaviour>();
        buttonBehaviour.RegisterOnButtonPressed(OnButtonPressed);

        Vector2 imageDimensions = this.GetComponent<ImageTargetBehaviour>().GetSize();
        imageTargetDimensionX = imageDimensions.x;
        imageTargetDimensionY = imageDimensions.y;
        Debug.Log("--Dimensions: " + imageTargetDimensionX + ", " + imageTargetDimensionY);

        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                enemy = child.gameObject;
                break;
            }
        }

        if (character == null)
        {
            character = GameObject.Instantiate(GameObject.Find("CharacterSelectMenu").GetComponent<CharacterSelect>().selectedCharacter); // Duplicate character
            character.active = false;
            character.layer = LayerMask.NameToLayer("Default"); // Set layer to Default (instead of UI)
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("--OnButtonPressed: " + vb.VirtualButtonName);

        Debug.Log("--Character: " + character.name);

        isFighting = !isFighting;

        // TODO:Button pressed multiple times moves enemy out of card area

        if (isFighting)
        {
            if (currentEnemy == null)
            {
                // Enemy fighting setup
                currentEnemy = this.gameObject;
                enemy.transform.position = new Vector3(enemy.transform.position.x - imageTargetDimensionX / 2, enemy.transform.position.y, enemy.transform.position.z);
                enemy.transform.Rotate(0, -90, 0);

                // Character fighting setup
                character.transform.parent = this.transform;
                character.transform.position = this.transform.position;
                character.transform.localScale = new Vector3(.025f, .025f, .025f);
                character.transform.position += new Vector3(imageTargetDimensionX / 2, 0, 0);
                character.transform.LookAt(enemy.transform);
                character.active = true;
            }
        }
        else
        {
            if (currentEnemy == this.gameObject)
            {
                // Restore
                currentEnemy = null;
                enemy.transform.position = new Vector3(enemy.transform.position.x + imageTargetDimensionX / 2, enemy.transform.position.y, enemy.transform.position.z);
                enemy.transform.Rotate(0, 90, 0);
                character.active = false;
            }
        }
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
