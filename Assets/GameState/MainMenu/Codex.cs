// Libraries
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Author:Jia
//Description: This script manages the codex screen in the main menu. It will display information about the different enemies
public class Codex : MainMenuState
{
    GameObject codexPanel;

    private Sprite[] enemySprites;
    private string[] enemyNames;
    private string[] enemyDamages;
    private string[] enemyHealths;
    private string[] enemySpeeds;
    private string[] enemyLevels;
    private string[] enemyAbilities;

    public override void EnterState(MainMenuStateController m)
    {
        // Get the codex panel from the Main Menu State Controller
        codexPanel = m.GetCodexPanel();

        // Get enemy information from the Main Menu State Controller
        enemySprites = m.GetEnemySprites();
        enemyNames = m.GetEnemyNames();
        enemyDamages = m.GetEnemyDamages();
        enemyHealths = m.GetEnemyHealths();
        enemySpeeds = m.GetEnemySpeeds();
        enemyLevels = m.GetEnemyLevels();
        enemyAbilities = m.GetEnemyAbilities();

        // Display the codex panel and hide the main menu panel
        m.DisplayMainMenu(false);
        codexPanel.SetActive(true);

        // Find Cancel button in codex panel
        Button cancelButton = codexPanel.transform.Find("Codex_XButton").GetComponent<Button>();

        // Set the buttons with the functionalities
        cancelButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new MainMenu());
        });

        // Assign the enemy information to the buttons in the codex panel, and set the button listeners to display the corresponding information when clicked
        for (int i = 0; i < enemySprites.Length; i++)
        {
            int arrayIndex = i; // Capture the current value of i for use in the lambda expression, avoid closure issue

            // Find the button in the codex panel corresponding to the current enemy, +1 for Button_1, Button_2, etc.
            Button btn = codexPanel.transform.Find("Enemy_Buttons/Button_" + i).GetComponent<Button>();

            btn.onClick.AddListener(() =>
            {
                m.PlayButtonClickSound();
                codexPanel.transform.Find("Enemy_Image").GetComponent<Image>().sprite = enemySprites[arrayIndex];
                codexPanel.transform.Find("Value_Text/Name_ValueText").GetComponent<TextMeshProUGUI>().text = enemyNames[arrayIndex];
                codexPanel.transform.Find("Value_Text/Health_ValueText").GetComponent<TextMeshProUGUI>().text = enemyHealths[arrayIndex];
                codexPanel.transform.Find("Value_Text/Damage_ValueText").GetComponent<TextMeshProUGUI>().text = enemyDamages[arrayIndex];
                codexPanel.transform.Find("Value_Text/Speed_ValueText").GetComponent<TextMeshProUGUI>().text = enemySpeeds[arrayIndex];
                codexPanel.transform.Find("Value_Text/Level_ValueText").GetComponent<TextMeshProUGUI>().text = enemyLevels[arrayIndex];
                codexPanel.transform.Find("Value_Text/Ability_ValueText").GetComponent<TextMeshProUGUI>().text = enemyAbilities[arrayIndex];
            });
        }
        
    }

    public override void ExitState(MainMenuStateController m)
    {
        codexPanel.SetActive(false);
        m.DisplayMainMenu(true);
    }
}
