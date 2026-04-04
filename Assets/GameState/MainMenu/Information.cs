// Libraries
using UnityEngine;
using UnityEngine.UI;
//Author:Jia
//Description: This script manages the information screen in the main menu.

public class Information : MainMenuState
{
    GameObject informationPanel;
    public override void EnterState(MainMenuStateController m)
    {
        informationPanel = m.GetInformationPanel();
        m.DisplayMainMenu(false);
        informationPanel.SetActive(true);

        // Find Cancel button in information panel
        Button cancelButton = informationPanel.transform.Find("Info_XButton").GetComponent<Button>();

        // Set the buttons with the functionalities
        cancelButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new MainMenu());
            Debug.Log("Information Cancel Button Clicked");
        });

        // Find all the small panels in the information panel
        Button teamButton = informationPanel.transform.Find("Team_Button").GetComponent<Button>();
        Button teamPanelXButton = informationPanel.transform.Find("Team_Screen/Team_XButton").GetComponent<Button>();

        // Set the buttons with the functionalities
        teamButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            informationPanel.transform.Find("Team_Screen").gameObject.SetActive(true);
        });

        teamPanelXButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            informationPanel.transform.Find("Team_Screen").gameObject.SetActive(false);
        });

        Button assetsButton = informationPanel.transform.Find("Assets_Button").GetComponent<Button>();
        Button assetsPanelXButton = informationPanel.transform.Find("Assets_Screen/Assets_XButton").GetComponent<Button>();

        // Set the buttons with the functionalities
        assetsButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            informationPanel.transform.Find("Assets_Screen").gameObject.SetActive(true);
        });

        assetsPanelXButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            informationPanel.transform.Find("Assets_Screen").gameObject.SetActive(false);
        });

        Button aboutButton = informationPanel.transform.Find("About_Button").GetComponent<Button>();
        Button aboutPanelXButton = informationPanel.transform.Find("About_Screen/About_XButton").GetComponent<Button>();
        
        // Set the buttons with the functionalities
        aboutButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            informationPanel.transform.Find("About_Screen").gameObject.SetActive(true);
        });

        aboutPanelXButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            informationPanel.transform.Find("About_Screen").gameObject.SetActive(false);
        });

    }

    public override void ExitState(MainMenuStateController m)
    {
        informationPanel.SetActive(false);
        m.DisplayMainMenu(true);
        Debug.Log("Exited Information State");
    }
}
