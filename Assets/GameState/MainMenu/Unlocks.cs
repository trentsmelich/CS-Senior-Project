using UnityEngine;
using UnityEngine.UI;

public class Unlocks : MainMenuState
{
    GameObject unlocksPanel;

    public override void EnterState(MainMenuStateController Main)
    {
        unlocksPanel = Main.GetUnlocksPanel();
        unlocksPanel.SetActive(true);

        Button backButton = unlocksPanel.transform.Find("Unlocks_XButton").GetComponent<Button>();

        backButton.onClick.AddListener(() =>
        {
            Main.SetState(new MainMenu());
            Debug.Log("Back Button Clicked");
        });
    }
    public override void ExitState(MainMenuStateController Main)
    {
        unlocksPanel.SetActive(false);
    }
    
}
