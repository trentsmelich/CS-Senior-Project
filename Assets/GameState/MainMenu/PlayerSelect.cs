// Libraries
using UnityEngine;
using UnityEngine.UI;
//Author:Jia
//Description: This script manages the player selection menu in the main menu.

public class PlayerSelect : MainMenuState
{
    GameObject playerSelectPanel;
    public override void EnterState(MainMenuStateController m)
    {
        playerSelectPanel = m.GetPlayerSelectPanel();
        m.DisplayMainMenu(false);
        playerSelectPanel.SetActive(true);

        // Find Player Select buttons in player select panel
        Button cancelButton = playerSelectPanel.transform.Find("Player_Select_XButton").GetComponent<Button>();

        // Set the buttons with the functionalities
        cancelButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new MainMenu());
            Debug.Log("Player Select Cancel Button Clicked");
        });

    }

    public override void ExitState(MainMenuStateController m)
    {
        playerSelectPanel.SetActive(false);
        m.DisplayMainMenu(true);
        Debug.Log("Exited Player Select State");
    }
}
