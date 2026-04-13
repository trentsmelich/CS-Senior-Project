// Libraries
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Author:Jia
//Description: This script manages the codex screen in the main menu. It will display information about the different enemies
public class Codex : MainMenuState
{
    GameObject codexPanel;

    [SerializeField] private Sprite[] enemySprites;
    [SerializeField] private string[] enemyNames;
    [SerializeField] private string[] enemyDamages;
    [SerializeField] private string[] enemyHealths;
    [SerializeField] private string[] enemyLevels;
    [SerializeField] private string[] enemyDescriptions;

    public override void EnterState(MainMenuStateController m)
    {
        codexPanel = m.GetCodexPanel();
        m.DisplayMainMenu(false);
        codexPanel.SetActive(true);

        // Find Cancel button in codex panel
        Button cancelButton = codexPanel.transform.Find("Codex_XButton").GetComponent<Button>();

        // Set the buttons with the functionalities
        cancelButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new MainMenu());
            Debug.Log("Codex Cancel Button Clicked");
        });

        //GameTutorialObject.transform.Find("Step1_Introduction/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step1Text;
        
    }

    public override void ExitState(MainMenuStateController m)
    {
        codexPanel.SetActive(false);
        m.DisplayMainMenu(true);
        Debug.Log("Exited Codex State");
    }
}
