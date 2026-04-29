// Libraries
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Author:Jia
//Description: This script manages the player selection menu in the main menu.

public class PlayerSelect : MainMenuState
{
    GameObject playerSelectPanel;
    private int currentPlayerSelected;
    private const string PlayerSelected = "PlayerSelected";


    public override void EnterState(MainMenuStateController m)
    {
        // Load the player select panel and display it, while hiding the main menu
        playerSelectPanel = m.GetPlayerSelectPanel();
        m.DisplayMainMenu(false);
        playerSelectPanel.SetActive(true);

        // Load the current player selected from PlayerPrefs, if not found, default to 0 (first player)
        currentPlayerSelected = 0;
        currentPlayerSelected = PlayerPrefs.GetInt(PlayerSelected, 0);
        UpdatePlayerSelectPanel();

        // Find Player Select buttons in player select panel
        Button cancelButton = playerSelectPanel.transform.Find("Player_Select_XButton").GetComponent<Button>();
        Button leftArrowButton = playerSelectPanel.transform.Find("Left_Button").GetComponent<Button>();
        Button rightArrowButton = playerSelectPanel.transform.Find("Right_Button").GetComponent<Button>();
        Button selectButton = playerSelectPanel.transform.Find("Select_Button").GetComponent<Button>();

        // Set the buttons with the functionalities
        cancelButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new MainMenu());
        });

        leftArrowButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            currentPlayerSelected--;
            if (currentPlayerSelected < 0)
            {
                currentPlayerSelected = 2;
            }
            UpdatePlayerSelectPanel();
        });

        rightArrowButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            currentPlayerSelected++;
            if (currentPlayerSelected > 2)
            {
                currentPlayerSelected = 0;
            }
            UpdatePlayerSelectPanel();
        });

        selectButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            UpdatePlayerSelected();
            m.SetState(new MainMenu());
        });

    }

    public override void ExitState(MainMenuStateController m)
    {
        playerSelectPanel.SetActive(false);
        m.DisplayMainMenu(true);
    }

    private void UpdatePlayerSelectPanel()
    {
        // Update the player select panel to show the currently selected player
        if(currentPlayerSelected == 0)
        {
            // Show Player 0 only, hide Player 1 and Player 2
            playerSelectPanel.transform.Find("Player_0").gameObject.SetActive(true);
            playerSelectPanel.transform.Find("Player_1").gameObject.SetActive(false);
            playerSelectPanel.transform.Find("Player_2").gameObject.SetActive(false);
        }
        else if(currentPlayerSelected == 1)
        {
            // Show Player 1 only, hide Player 0 and Player 2
            playerSelectPanel.transform.Find("Player_0").gameObject.SetActive(false);
            playerSelectPanel.transform.Find("Player_1").gameObject.SetActive(true);
            playerSelectPanel.transform.Find("Player_2").gameObject.SetActive(false);
        }
        else if(currentPlayerSelected == 2)
        {
            // Show Player 2 only, hide Player 0 and Player 1
            playerSelectPanel.transform.Find("Player_0").gameObject.SetActive(false);
            playerSelectPanel.transform.Find("Player_1").gameObject.SetActive(false);
            playerSelectPanel.transform.Find("Player_2").gameObject.SetActive(true);   
        }
        else
        {
            // This should never happen, but if it does, default to showing the first player
            playerSelectPanel.transform.Find("Player_0").gameObject.SetActive(true);
            playerSelectPanel.transform.Find("Player_1").gameObject.SetActive(false);
            playerSelectPanel.transform.Find("Player_2").gameObject.SetActive(false); 
        }
    }

    private void UpdatePlayerSelected()
    {
        PlayerPrefs.SetInt(PlayerSelected, currentPlayerSelected);
        PlayerPrefs.Save();
    }
}
