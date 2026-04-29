// Libraries
using UnityEngine;
using UnityEngine.UI;
//Author:Jia
//Description: This script manages the information screen in the main menu.

public class Information : MainMenuState
{
    GameObject informationPanel;

    private Button cancelButton;
    private Button teamButton;
    private Button teamPanelXButton;
    private Button assetsButton;
    private Button assetsPanelXButton;
    private Button aboutButton;
    private Button aboutPanelXButton;

    public override void EnterState(MainMenuStateController m)
    {
        informationPanel = m.GetInformationPanel();
        m.DisplayMainMenu(false);
        informationPanel.SetActive(true);

        // Find Cancel button in information panel
        cancelButton = informationPanel.transform.Find("Info_XButton").GetComponent<Button>();

        // Set the buttons with the functionalities
        cancelButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new MainMenu());
            Debug.Log("Information Cancel Button Clicked");
        });

        // Find all the small panels in the information panel
        teamButton = informationPanel.transform.Find("Team_Button").GetComponent<Button>();
        teamPanelXButton = informationPanel.transform.Find("Team_Screen/Team_XButton").GetComponent<Button>();

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

        assetsButton = informationPanel.transform.Find("Assets_Button").GetComponent<Button>();
        assetsPanelXButton = informationPanel.transform.Find("Assets_Screen/Assets_XButton").GetComponent<Button>();

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

        aboutButton = informationPanel.transform.Find("About_Button").GetComponent<Button>();
        aboutPanelXButton = informationPanel.transform.Find("About_Screen/About_XButton").GetComponent<Button>();

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
        cancelButton.onClick.RemoveAllListeners();;
        teamButton.onClick.RemoveAllListeners();
        teamPanelXButton.onClick.RemoveAllListeners();
        assetsButton.onClick.RemoveAllListeners();
        assetsPanelXButton.onClick.RemoveAllListeners();
        aboutButton.onClick.RemoveAllListeners();
        aboutPanelXButton.onClick.RemoveAllListeners();

        informationPanel.SetActive(false);
        m.DisplayMainMenu(true);
        Debug.Log("Exited Information State");
    }
}
