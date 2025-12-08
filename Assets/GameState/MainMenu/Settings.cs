using UnityEngine;
using UnityEngine.UI;

public class Settings : MainMenuState
{
    GameObject settingsPanel;

    public override void EnterState(MainMenuStateController Main)
    {
        settingsPanel = Main.GetSettingsPanel();
        settingsPanel.SetActive(true);

        Button backButton = settingsPanel.transform.Find("Options_XButton").GetComponent<Button>();

        backButton.onClick.AddListener(() =>
        {
            Main.PlayButtonClickSound();
            Main.SetState(new MainMenu());
            Debug.Log("Back Button Clicked");
        });
    }
    public override void ExitState(MainMenuStateController Main)
    {
        settingsPanel.SetActive(false);
    }
    
}