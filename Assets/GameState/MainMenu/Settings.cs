
// Libraries
using UnityEngine;
using UnityEngine.UI;
//Author:Jia
//Description: This script manages the settings menu, including handling button interactions for navigating back to the main menu.
public class Settings : MainMenuState
{
    // Declare the verable for the settings panel
    GameObject settingsPanel;

    public override void EnterState(MainMenuStateController Main)
    {
        // Set the new panel variable from the controller
        settingsPanel = Main.GetSettingsPanel();
        settingsPanel.SetActive(true);

        // Find the options close button
        Button backButton = settingsPanel.transform.Find("Options_XButton").GetComponent<Button>();

        // Set the functionality for the back button
        backButton.onClick.AddListener(() =>
        {
            Main.PlayButtonClickSound();
            Main.SetState(new MainMenu());
            Debug.Log("Back Button Clicked");
        });
    }
    public override void ExitState(MainMenuStateController Main)
    {
        //Close the panel
        settingsPanel.SetActive(false);
    }
    
}