using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    static public GameObject playerModel;
    static private CharacterStats player;
    private GameObject enemyModel = null;
    private CharacterStats enemy;
    static private bool isFighting = false;
    static private GameObject currentEnemy = null;
    static private Slider playerHealthBar;
    static private Slider enemyHealthBar;
    private float imageTargetDimensionX;
    private float imageTargetDimensionY;
    private float lastAttackTime = 0f;
    private bool isPlayersTurnToAttack = true;

#pragma warning disable CS0618 // Type or member is obsolete
    private VirtualButtonBehaviour buttonBehaviour;

    // Start is called before the first frame update
    private void Awake()
    {
        buttonBehaviour = GetComponentInChildren<VirtualButtonBehaviour>();
        buttonBehaviour.RegisterOnButtonPressed(OnButtonPressed);

        playerHealthBar = GameObject.Find("Player Health Bar").GetComponentInChildren<Slider>();
        enemyHealthBar = GameObject.Find("Enemy Health Bar").GetComponentInChildren<Slider>();

        Vector2 imageDimensions = this.GetComponent<ImageTargetBehaviour>().GetSize();
        imageTargetDimensionX = imageDimensions.x;
        imageTargetDimensionY = imageDimensions.y;

        enemy = GetComponent<CharacterStats>();

        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                enemyModel = child.gameObject;
                break;
            }
        }

        if (playerModel == null)
        {
            player = GetComponent<CharacterStats>();
            playerModel = GameObject.Instantiate(GameObject.Find("CharacterSelectMenu").GetComponent<CharacterSelect>().selectedCharacter); // Duplicate character
            playerModel.active = false;
            playerModel.layer = LayerMask.NameToLayer("Default"); // Set layer to Default (instead of UI)
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("--OnButtonPressed: " + vb.VirtualButtonName);

        Debug.Log("--Character: " + playerModel.name);

        isFighting = !isFighting;

        // TODO:Button pressed multiple times moves enemy out of card area

        if (isFighting)
        {
            if (currentEnemy == null)
            {
                // Enemy fighting setup
                currentEnemy = this.gameObject;
                enemyModel.transform.position = new Vector3(enemyModel.transform.position.x - imageTargetDimensionX / 2, enemyModel.transform.position.y, enemyModel.transform.position.z);
                enemyModel.transform.Rotate(0, -90, 0);

                // Player fighting setup
                playerModel.transform.parent = this.transform;
                playerModel.transform.position = this.transform.position;
                playerModel.transform.localScale = new Vector3(.025f, .025f, .025f);
                playerModel.transform.position += new Vector3(imageTargetDimensionX / 2, 0, 0);
                playerModel.transform.LookAt(enemyModel.transform);
                playerModel.active = true;
            }
        }
        else
        {
            if (currentEnemy == this.gameObject)
            {
                // Restore
                currentEnemy = null;
                enemyModel.transform.position = new Vector3(enemyModel.transform.position.x + imageTargetDimensionX / 2, enemyModel.transform.position.y, enemyModel.transform.position.z);
                enemyModel.transform.Rotate(0, 90, 0);
                playerModel.active = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFighting && currentEnemy == this.gameObject) {
            float time = Time.time;
            
            if (time > lastAttackTime + .5f)
            {
                Debug.Log("Time: " + time.ToString());
                Debug.Log("Last: " + lastAttackTime.ToString());
                lastAttackTime = time;

                if (isPlayersTurnToAttack)
                {
                    player.attackCharacter(enemy);
                    Debug.Log("Enemy Health: " + enemy.getHealth().ToString());
                    enemyHealthBar.maxValue = enemy.getMaxHealth();
                    enemyHealthBar.value = enemy.getHealth();
                }
                else
                {
                    enemy.attackCharacter(player);
                    Debug.Log("Player Health: " + player.getHealth().ToString());
                    playerHealthBar.maxValue = player.getMaxHealth();
                    playerHealthBar.value = player.getHealth();
                }                    
                
                isPlayersTurnToAttack = !isPlayersTurnToAttack;
            }            
        }
    }
}
