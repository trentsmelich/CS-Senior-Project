// Libraries
using UnityEngine;
using UnityEngine.UI;
//Author:Jia
//Description: This script manages the keybinds menu in the settings

public class Keybinds : MainMenuState
{
    GameObject keybindsPanel;
    private Button backButton;

    public override void EnterState(MainMenuStateController Main)
    {
        // Set the new panel variable from the controller
        Main.GetSettingsPanel().SetActive(true);
        keybindsPanel = Main.GetKeybindsPanel();
        keybindsPanel.SetActive(true);

        // Find the options close button
        backButton = keybindsPanel.transform.Find("Keybinds_XButton").GetComponent<Button>();

        // Set the functionality for the back button
        backButton.onClick.AddListener(() =>
        {
            Main.PlayButtonClickSound();
            Main.SetState(new Settings());
        });
    }
    public override void ExitState(MainMenuStateController Main)
    {
        //Close the panel
        backButton.onClick.RemoveAllListeners();
        keybindsPanel.SetActive(false);
    }
    
}
