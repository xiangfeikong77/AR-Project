using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using TMPro;

#pragma warning disable CS0618 // Type or member is obsolete - VirtualButtonBehaviour

public class Battle : MonoBehaviour
{
    /* Player variables */
    static public GameObject playerModel;    
    static private CharacterStats player;
    static private Slider playerHealthBar;
    static private TextMeshProUGUI playerHealthBarText;

    /* Enemy variables */
    private GameObject enemyModel = null;
    private CharacterStats enemy;
    static private Slider enemyHealthBar;
    static private TextMeshProUGUI enemyHealthBarText;

    /* Battle variables */
    static private bool isFighting = false;
    private bool isPlayersTurnToAttack = true;
    static private GameObject currentEnemy = null;
    
    /* Image target dimensions */
    private float imageTargetDimensionX;
    private float imageTargetDimensionY;

    /* Timers */
    private float lastAttackTime = 0f;  // Time of last attack
    static private float lastHealTime = 0f; // Time of last heal
    static private float lastSpecialAttackTime = 0f; // Time of last special attack
    static public float attackTime = 1f; // Attack every 1 second
    static public float healWaitTime = 5f; // Able to heal every 5 seconds
    static public float specialAttackWaitTime = 10f; // Able to use special attack every 10 seconds
    static public float toastDuration = 1f; // How long toasts last on screen

    /* Button variables */
    static private VirtualButtonBehaviour healButton;
    static private VirtualButtonBehaviour specialAttackButton;
    static private Material healthButtonMaterial;
    static private Material specialAttackButtonMaterial;
    static private Color32 buttonFade = new Color32(50, 50, 50, 255); // Fades the button when waiting for timer
    private Toast toaster; // Display button action messages
    static private float healAmount = 15f;

    // Start is called before the first frame update
    private void Awake()
    {
        /* Button initialization */
        VirtualButtonBehaviour fightButton = GetComponentInChildren<VirtualButtonBehaviour>();
        fightButton.RegisterOnButtonPressed(OnFightButtonPressed);

        healButton = GameObject.Find("HealButton").GetComponent<VirtualButtonBehaviour>();
        specialAttackButton = GameObject.Find("SpecialAttackButton").GetComponent<VirtualButtonBehaviour>();

        healButton.RegisterOnButtonPressed(OnHealButtonPressed);
        specialAttackButton.RegisterOnButtonPressed(OnSpecialAttackButtonPressed);

        healthButtonMaterial = GameObject.Find("HealButtonPlane").GetComponent<Renderer>().material;
        specialAttackButtonMaterial = GameObject.Find("SpecialAttackButtonPlane").GetComponent<Renderer>().material;

        /* Health Bar initialization */
        GameObject playerHealthBarParent = GameObject.Find("Player Health Bar");
        GameObject enemyHealthBarParent = GameObject.Find("Enemy Health Bar");

        playerHealthBar = playerHealthBarParent.GetComponentInChildren<Slider>();
        enemyHealthBar = enemyHealthBarParent.GetComponentInChildren<Slider>();

        foreach (TextMeshProUGUI text in playerHealthBarParent.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Debug.Log(text);
            if (text.name.Equals("HealToast"))
            {
                playerHealthBarText = text;
            }
        }

        foreach (TextMeshProUGUI text in enemyHealthBarParent.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (text.name.Equals("SpecialAttackToast"))
            {
                enemyHealthBarText = text;
            }
        }

        /* Toast initialization */
        toaster = gameObject.AddComponent<Toast>();

        /* Dimension initialization */
        Vector2 imageDimensions = this.GetComponent<ImageTargetBehaviour>().GetSize();
        imageTargetDimensionX = imageDimensions.x;
        imageTargetDimensionY = imageDimensions.y;

        /* Enemy initialization */
        enemy = GetComponent<CharacterStats>();
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                enemyModel = child.gameObject;
                break;
            }
        }

        /* Player initialization */
        if (playerModel == null)
        {
            Debug.Log("playermodel");
            player = GetComponent<CharacterStats>();
            GameObject selectedCharacter = GameObject.Find("CharacterSelectMenu").GetComponent<CharacterSelect>().selectedCharacter;
            player = selectedCharacter.GetComponent<CharacterStats>();
            playerModel = GameObject.Instantiate(selectedCharacter); // Duplicate character
            playerModel.active = false;
            playerModel.layer = LayerMask.NameToLayer("Default"); // Set layer to Default (instead of UI)
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFighting && currentEnemy == this.gameObject)
        {
            float time = Time.time;

            /* Battle */
            if (time > lastAttackTime + (attackTime / 2))
            {
                bool isSpecial = false;
                lastAttackTime = time;

                if (isPlayersTurnToAttack)
                {

                    player.attackCharacter(enemy, isSpecial);
                    updateEnemyHealthBar();
                }
                else
                {
                    enemy.attackCharacter(player, isSpecial);
                    updatePlayerHealthBar();
                }

                isPlayersTurnToAttack = !isPlayersTurnToAttack;
            }

            /* Reset button colors if enough time has passed */
            if (time > lastHealTime + healWaitTime)
            {
                healthButtonMaterial.color = Color.white;
            }

            if (time > lastSpecialAttackTime + specialAttackWaitTime)
            {
                specialAttackButtonMaterial.color = Color.white;
            }
        }
    }

    public void OnFightButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("--OnButtonPressed: " + vb.VirtualButtonName);

        Debug.Log("--Character: " + playerModel.name);

        isFighting = !isFighting;

        if (isFighting)
        {
            Time.timeScale = 1; // Unpause

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
                Time.timeScale = 0; // Pause

                // Restore
                currentEnemy = null;
                enemyModel.transform.position = new Vector3(enemyModel.transform.position.x + imageTargetDimensionX / 2, enemyModel.transform.position.y, enemyModel.transform.position.z);
                enemyModel.transform.Rotate(0, 90, 0);
                playerModel.active = false;
            }
        }
    }

    public void OnHealButtonPressed(VirtualButtonBehaviour vb)
    {
        if (isFighting && currentEnemy == this.gameObject)
        {
            Debug.Log("--OnButtonPressed Heal: " + vb.VirtualButtonName);
            float time = Time.time;

            if (time > lastHealTime + healWaitTime)
            {
                toaster.showToast(playerHealthBarText, "Healed", toastDuration);
                Debug.Log("Heal");
                Debug.Log("Player Before Health: " + player.getHealth().ToString());
                player.addHealth(healAmount);
                Debug.Log("Player After Health: " + player.getHealth().ToString());
                updatePlayerHealthBar();
                lastHealTime = time;
                healthButtonMaterial.color = buttonFade;
            }
        }
    }

    public void OnSpecialAttackButtonPressed(VirtualButtonBehaviour vb)
    {
        if (isFighting && currentEnemy == this.gameObject)
        {
            Debug.Log("--OnButtonPressed Special: " + vb.VirtualButtonName);
            float time = Time.time;

            if (time > lastSpecialAttackTime + specialAttackWaitTime)
            {
                toaster.showToast(enemyHealthBarText, "Special", toastDuration);
                Debug.Log("Special Attack");
                bool isSpecial = true;
                player.attackCharacter(enemy, isSpecial);
                updateEnemyHealthBar();
                lastSpecialAttackTime = time;
                specialAttackButtonMaterial.color = buttonFade;
            }
        }
    }

    private void updatePlayerHealthBar()
    {
        Debug.Log("Player Health: " + player.getHealth().ToString());
        playerHealthBar.maxValue = player.getMaxHealth();
        playerHealthBar.value = player.getHealth();
    }

    private void updateEnemyHealthBar()
    {
        Debug.Log("Enemy Health: " + enemy.getHealth().ToString());
        enemyHealthBar.maxValue = enemy.getMaxHealth();
        enemyHealthBar.value = enemy.getHealth();
    }
}
