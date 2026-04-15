// Libraries
using UnityEngine;
using UnityEngine.UI;

//Author:Jia
//Description: This script is intended to manage the requirements panel in the main menu, which will display the requirements for unlocking different towers and levels.

public class Requirements : MainMenuState
{
    GameObject requirementsPanel;

    // Unlock requirements for towers and levels, these are the keys for PlayerPrefs to check if the player has unlocked the corresponding tower or level

    // Slingshot
    private const string slingLvl2 = "unlock_slingshot_lvl2";
    private const string slingLvl3 = "unlock_slingshot_lvl3";

    // Catapult
    private const string catapultLvl1 = "unlock_catapult_lvl1";
    private const string catapultLvl2 = "unlock_catapult_lvl2";
    private const string catapultLvl3 = "unlock_catapult_lvl3";

    // Freezer
    private const string freezeLvl1 = "unlock_freeze_lvl1";
    private const string freezeLvl2 = "unlock_freeze_lvl2";
    private const string freezeLvl3 = "unlock_freeze_lvl3";

    // Farm
    private const string farmLvl1 = "unlock_farm_lvl1";
    private const string farmLvl2 = "unlock_farm_lvl2";
    private const string farmLvl3 = "unlock_farm_lvl3";


    public override void EnterState(MainMenuStateController m)
    {
        requirementsPanel = m.GetRequirementsPanel();
        m.DisplayMainMenu(false);
        requirementsPanel.SetActive(true);

        // Find Cancel button in requirements panel
        Button cancelButton = requirementsPanel.transform.Find("Requirements_XButton").GetComponent<Button>();

        // Set the buttons with the functionalities
        cancelButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new MainMenu());
            Debug.Log("Requirements Cancel Button Clicked");
        });

        // If the player has unlocked a building, the corresponding requirement will show a green box
        // Slingshot
        if (PlayerPrefs.GetInt(slingLvl2, 0) == 1) requirementsPanel.transform.Find("Scroll/Holder/Req_2/Green_Box").gameObject.SetActive(true);
        if (PlayerPrefs.GetInt(slingLvl3, 0) == 1) requirementsPanel.transform.Find("Scroll/Holder/Req_3/Green_Box").gameObject.SetActive(true);
        // Catapult
        if (PlayerPrefs.GetInt(catapultLvl1, 0) == 1) requirementsPanel.transform.Find("Scroll/Holder/Req_4/Green_Box").gameObject.SetActive(true);
        if (PlayerPrefs.GetInt(catapultLvl2, 0) == 1) requirementsPanel.transform.Find("Scroll/Holder/Req_5/Green_Box").gameObject.SetActive(true);
        if (PlayerPrefs.GetInt(catapultLvl3, 0) == 1) requirementsPanel.transform.Find("Scroll/Holder/Req_6/Green_Box").gameObject.SetActive(true);
        // Freezer
        if (PlayerPrefs.GetInt(freezeLvl1, 0) == 1) requirementsPanel.transform.Find("Scroll/Holder/Req_7/Green_Box").gameObject.SetActive(true);
        if (PlayerPrefs.GetInt(freezeLvl2, 0) == 1) requirementsPanel.transform.Find("Scroll/Holder/Req_8/Green_Box").gameObject.SetActive(true);
        if (PlayerPrefs.GetInt(freezeLvl3, 0) == 1) requirementsPanel.transform.Find("Scroll/Holder/Req_9/Green_Box").gameObject.SetActive(true);
        // Farm
        if (PlayerPrefs.GetInt(farmLvl1, 0) == 1) requirementsPanel.transform.Find("Scroll/Holder/Req_10/Green_Box").gameObject.SetActive(true);
        if (PlayerPrefs.GetInt(farmLvl2, 0) == 1) requirementsPanel.transform.Find("Scroll/Holder/Req_11/Green_Box").gameObject.SetActive(true);
        if (PlayerPrefs.GetInt(farmLvl3, 0) == 1) requirementsPanel.transform.Find("Scroll/Holder/Req_12/Green_Box").gameObject.SetActive(true);
    }

    public override void ExitState(MainMenuStateController m)
    {
        requirementsPanel.SetActive(false);
        m.DisplayMainMenu(true);
        Debug.Log("Exiting Requirements State");
    }
    
}
