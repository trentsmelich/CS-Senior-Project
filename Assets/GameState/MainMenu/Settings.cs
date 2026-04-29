
// Libraries
using UnityEngine;
using UnityEngine.UI;
//Author:Jia
//Description: This script manages the settings menu, including handling button interactions for navigating back to the main menu.
public class Settings : MainMenuState
{
    // Declare the verable for the settings panel
    GameObject settingsPanel;

    private Button backButton;
    private Button keybindsButton;

    public override void EnterState(MainMenuStateController Main)
    {
        // Set the new panel variable from the controller
        settingsPanel = Main.GetSettingsPanel();
        settingsPanel.SetActive(true);

        // Find the options close button
        backButton = settingsPanel.transform.Find("Options_XButton").GetComponent<Button>();
        keybindsButton = settingsPanel.transform.Find("Keybinds_Button").GetComponent<Button>();

        // Set the functionality for the back button
        backButton.onClick.AddListener(() =>
        {
            Main.PlayButtonClickSound();
            Main.SetState(new MainMenu());
            Debug.Log("Back Button Clicked");
        });

        // Set the functionality for the keybinds button
        keybindsButton.onClick.AddListener(() =>
        {
            Main.PlayButtonClickSound();
            Main.SetState(new Keybinds());
            Debug.Log("Keybinds Button Clicked");
        });
    }
    public override void ExitState(MainMenuStateController Main)
    {
        //Close the panel
        backButton.onClick.RemoveAllListeners();
        keybindsButton.onClick.RemoveAllListeners();
        settingsPanel.SetActive(false);
    }
    
}