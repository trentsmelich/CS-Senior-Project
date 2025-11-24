using System;
using UnityEngine;
using UnityEngine.UI;

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
        

        

        // Implementation for entering the in-shop state
        Game.ShowPlayerUI(false);
        // open shop UI
        shopScreen.SetActive(true);
        // pause time
        Time.timeScale = 0;
        // Default to farm upgrades panel
        FilterButton(1); 

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
        
        if(index == 0)
        {
            //Filter to show only damage upgrades
            shopScreen.transform.Find("Filter/Damage_ScrollView").gameObject.SetActive(true);
            shopScreen.transform.Find("Filter/Farm_ScrollView").gameObject.SetActive(false);
            shopScreen.transform.Find("Filter/Stat_ScrollView").gameObject.SetActive(false);
        }
        else if(index == 1)
        {
            //Filter to show only farm upgrades
            shopScreen.transform.Find("Filter/Damage_ScrollView").gameObject.SetActive(false);
            shopScreen.transform.Find("Filter/Farm_ScrollView").gameObject.SetActive(true);
            shopScreen.transform.Find("Filter/Stat_ScrollView").gameObject.SetActive(false);
            
        }
        else if(index == 2)
        {
            //Filter to show only stat upgrades
            shopScreen.transform.Find("Filter/Damage_ScrollView").gameObject.SetActive(false);
            shopScreen.transform.Find("Filter/Farm_ScrollView").gameObject.SetActive(false);
            shopScreen.transform.Find("Filter/Stat_ScrollView").gameObject.SetActive(true);
            
        }
        
    }

    private void MakeLists(GameStateController Game)
    {
        //loop thropugh list of towers instantiate buttons for all the towers with listeners then assign image of tower to button
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        foreach (GameObject tower in towers)
        {
            //instantiate button prefab under farm scroll view, content
            GameObject button = GameObject.Instantiate(Game.GetTowerButtonPrefab(), shopScreen.transform.Find("Filter/Farm_ScrollView/Viewport/Content"));
            //set button image to tower image
            button.GetComponent<Image>().sprite = tower.GetComponent<TowerParent>().TowerImage;
            //add listener to button to open tower upgrade screen
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                if(playerStats.coins >= tower.GetComponent<TowerParent>().TowerCost)
                {
                    playerStats.coins -= tower.GetComponent<TowerParent>().TowerCost;
                    Game.SetPlaceTower(tower);
                }
                


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