
// Libraries
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Unlocks : MainMenuState
{
    // Set the variable list and get the panel
    GameObject unlocksPanel;
    private GameObject[] towers;
    private String towerType;

    public override void EnterState(MainMenuStateController Main)
    {
        // Set the panel and get the towers
        unlocksPanel = Main.GetUnlocksPanel();
        towers = Main.GetTowers();
        unlocksPanel.SetActive(true);

        // Default damage filter when the player first enter the shop
        towerType = "Damage";
        MakeLists(Main);

        // Find the unlock close button
        Button backButton = unlocksPanel.transform.Find("Unlocks_XButton").GetComponent<Button>();

        // Set the unlock close button
        backButton.onClick.AddListener(() =>
        {
            Main.PlayButtonClickSound();
            Main.SetState(new MainMenu());
            Debug.Log("Back Button Clicked");
        });

        // Find the button
        Button damageButton = unlocksPanel.transform.Find("Damage_Button").GetComponent<Button>();
        Button farmButton = unlocksPanel.transform.Find("Farm_Button").GetComponent<Button>();
        Button statButton = unlocksPanel.transform.Find("Stat_Button").GetComponent<Button>();

        // Give the filter buttons functionality
        damageButton.onClick.AddListener(() =>
        {
            Main.PlayButtonClickSound();
            //Filter to show only damage upgrades
            towerType = "Damage";
            MakeLists(Main);
        });

        farmButton.onClick.AddListener(() =>
        {
            Main.PlayButtonClickSound();
            //Filter to show only farm upgrades
            towerType = "Farm";
            MakeLists(Main);
        });

        statButton.onClick.AddListener(() =>
        {
            Main.PlayButtonClickSound();
            //Filter to show only stat upgrades
            towerType = "Stat";
            MakeLists(Main);
        });

    }


    private void MakeLists(MainMenuStateController Main)
    {
        //loop thropugh list of towers instantiate buttons for all the towers with listeners then assign image of tower to button

        //remove all childs of content under the scroll view
        Transform contentTransform = unlocksPanel.transform.Find("ScrollView/Viewport/Content");
        foreach (Transform child in contentTransform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (GameObject tower in towers)
        {
            if(tower.GetComponent<TowerParent>().TowerType == towerType)
            {
                // Instantiate button prefab under the scroll view, content
                GameObject button = GameObject.Instantiate(Main.GetTowerButtonPrefab(), unlocksPanel.transform.Find("ScrollView/Viewport/Content"));

                // Set button image to tower image
                button.GetComponent<Image>().sprite = tower.GetComponent<TowerParent>().TowerImage;
                button.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Level " + tower.GetComponent<TowerParent>().Level.ToString();

                if (tower.GetComponent<TowerParent>().Unlocked)
                {
                    // When the tower button with image is clicked
                    button.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        //Play Button Sound
                        Main.PlayButtonClickSound();

                        // Open tower info screen
                        unlocksPanel.transform.Find("Tower_Info_Screen").gameObject.SetActive(true);

                        // Set tower info screen texts to tower info
                        unlocksPanel.transform.Find("Tower_Info_Screen/Tower_Image").GetComponent<Image>().sprite = tower.GetComponent<TowerParent>().TowerImage;
                        unlocksPanel.transform.Find("Tower_Info_Screen/Tower_Texts/Tower_Name").GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerParent>().GetName();
                        unlocksPanel.transform.Find("Tower_Info_Screen/Tower_Texts/Attribute_Description").GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerParent>().GetAttributes();
                        unlocksPanel.transform.Find("Tower_Info_Screen/Tower_Texts/Text_Tower_Description").GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerParent>().GetDescription();                  

                        // Close tower info screen
                        Button xButton = unlocksPanel.transform.Find("Tower_Info_Screen/X_Button").GetComponent<Button>();
                        xButton.onClick.AddListener(() =>
                        {
                            Main.PlayButtonClickSound();
                            unlocksPanel.transform.Find("Tower_Info_Screen").gameObject.SetActive(false);
                        });
                        

                    });
                }
                else
                {
                    //modify tower image to make it dark dark dark grey out to show its locked
                    ColorBlock cb = button.GetComponent<Button>().colors;
                    cb.normalColor = new Color(0.2f, 0.2f, 0.2f);
                    cb.highlightedColor = new Color(0.2f, 0.2f, 0.2f);
                    cb.pressedColor = new Color(0.2f, 0.2f, 0.2f);
                    cb.selectedColor = new Color(0.2f, 0.2f, 0.2f);
                    button.GetComponent<Button>().colors = cb;
                }

                
            }
            

            
        }

    }


    public override void ExitState(MainMenuStateController Main)
    {
        // Close the panel when the player clicked the X button
        unlocksPanel.SetActive(false);
    }
    
    
}
