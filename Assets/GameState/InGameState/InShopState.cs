using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InShopState : GameState
{
    private GameObject shopScreen;
    private GameObject[] towers;
    //private RectTransform listParent;
    private String towerType;

    public override void EnterState(GameStateController Game)
    {
        //towers = Game.GetTowers();
        shopScreen = Game.GetShopScreen();
        towers = Game.GetTowers();
        
        // Set The Tower UI Panels to inactive since the player is not viewing them yet
        shopScreen.transform.Find("Tower_Info_Screen").gameObject.SetActive(false);

        // Implementation for entering the in-shop state
        Game.ShowPlayerUI(false);
        // open shop UI
        shopScreen.SetActive(true);
        // pause time
        Time.timeScale = 0;

        //Exit shop button
        Button xButton = shopScreen.transform.Find("Shop_XButton").GetComponent<Button>();

        xButton.onClick.AddListener(() =>
        {
            Game.SetState(new gameIdleState());
        });
    
        //In shop upgrade filter buttons
        Button damageButton = shopScreen.transform.Find("Filter/Damage_Button").GetComponent<Button>();
        Button farmButton = shopScreen.transform.Find("Filter/Farm_Button").GetComponent<Button>();
        Button statButton = shopScreen.transform.Find("Filter/Stat_Button").GetComponent<Button>();

        damageButton.onClick.AddListener(() =>
        {
            //Filter to show only damage upgrades
            FilterButton(0);
            towerType = "Damage";
            MakeLists(Game);
            //shopScreen.GetComponent<Shop>().FilterUpgrades("Damage");
        });

        farmButton.onClick.AddListener(() =>
        {
            //Filter to show only farm upgrades
            FilterButton(1);
            towerType = "Farm";
            MakeLists(Game);
            //shopScreen.GetComponent<Shop>().FilterUpgrades("Farm");
        });

        statButton.onClick.AddListener(() =>
        {
            //Filter to show only stat upgrades
            FilterButton(2);
            towerType = "Stat";
            MakeLists(Game);
            //shopScreen.GetComponent<Shop>().FilterUpgrades("Stat");
        });
        
    }
    

    private void FilterButton(int index)
    {
        shopScreen.transform.Find("Filter/ScrollView").gameObject.SetActive(true);
        
        if(index == 0)
        {
            // Filter to show only damage upgrades

            
        }
        else if(index == 1)
        {
            // Filter to show only farm upgrades
            
            
            
        }
        else if(index == 2)
        {
            // Filter to show only stat upgrades
            
            
        }
        
    }

    private void MakeLists(GameStateController Game)
    {
        //loop thropugh list of towers instantiate buttons for all the towers with listeners then assign image of tower to button
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        //remove all childs of content under the scroll view
        Transform contentTransform = shopScreen.transform.Find("Filter/ScrollView/Viewport/Content");
        foreach (Transform child in contentTransform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (GameObject tower in towers)
        {
            //instantiate button prefab under the scroll view, content
            GameObject button = GameObject.Instantiate(Game.GetTowerButtonPrefab(), shopScreen.transform.Find("Filter/ScrollView/Viewport/Content"));
            //set button image to tower image
            button.GetComponent<Image>().sprite = tower.GetComponent<TowerParent>().TowerImage;
            //add listener to button to open tower upgrade screen


            button.GetComponent<Button>().onClick.AddListener(() =>
            {

                Debug.Log("Tower Button Clicked: ");

                // Open tower info screen
                shopScreen.transform.Find("Tower_Info_Screen").gameObject.SetActive(true);
                shopScreen.transform.Find("Tower_Info_Screen/Tower_Texts/Text_NotEnoughCoins").gameObject.SetActive(false);

                // Set tower info screen texts to tower info
                // ShopScreen.transform.Find("Tower_Info_Screen/Tower_Texts/TowerName_Text").GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerParent>().TowerName;
                shopScreen.transform.Find("Tower_Info_Screen/Tower_Image").GetComponent<Image>().sprite = tower.GetComponent<TowerParent>().TowerImage;
                
                Button xButton = shopScreen.transform.Find("Tower_Info_Screen/X_Button").GetComponent<Button>();
                Button buyButton = shopScreen.transform.Find("Tower_Info_Screen/Buy_Button").GetComponent<Button>();

                // Close tower info screen
                xButton.onClick.AddListener(() =>
                {
                    shopScreen.transform.Find("Tower_Info_Screen").gameObject.SetActive(false);
                });

                // When buy button is clicked
                buyButton.onClick.AddListener(() =>
                {
                    //check if player has enough coins to purchase tower
                    if(playerStats.coins >= tower.GetComponent<TowerParent>().TowerCost)
                    {
                        Debug.Log("Purchasing tower: ");
                        playerStats.coins -= tower.GetComponent<TowerParent>().TowerCost;

                        //If the building is purchased, set the tower to be placed and change state to building state
                        Game.SetPlaceTower(tower);
                        Game.SetState(new BuildingState());

                    }
                    else
                    {
                        shopScreen.transform.Find("Tower_Info_Screen/Tower_Texts/Text_NotEnoughCoins").gameObject.SetActive(true);
                    }
                });
                

            });

            
        }


    }

    public override void UpdateState(GameStateController Game)
    {
        // Implementation for updating the in-shop state
    }

    public override void ExitState(GameStateController Game)
    {
        // close shop UI
        shopScreen.SetActive(false);
        // resume time
        Time.timeScale = 1;
        // Implementation for exiting the in-shop state
        Game.ShowPlayerUI(true);

    }
}