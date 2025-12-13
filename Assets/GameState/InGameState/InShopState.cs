using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Author:Jia and Trent
//Description: This script manages the in shop state, including displaying the shop UI, filtering towers, and handling user interactions within the shop.
public class InShopState : GameState
{
    private GameObject shopScreen;
    private GameObject[] towers;
    //private RectTransform listParent;
    private String towerType;

    private AudioSource coinSFX;

    public override void EnterState(GameStateController Game)
    {
        //towers = Game.GetTowers();
        shopScreen = Game.GetShopScreen();
        towers = Game.GetTowers();

        //set Coin SFX
        coinSFX = GameObject.Find("SFX/Coin_SFX").GetComponent<AudioSource>();
        
        // Set The Tower UI Panels to inactive since the player is not viewing them yet
        shopScreen.transform.Find("Tower_Info_Screen").gameObject.SetActive(false);

        // Implementation for entering the in-shop state
        Game.ShowPlayerUI(false);
        // open shop UI
        shopScreen.SetActive(true);
        // pause time
        Time.timeScale = 0;

        // Default filter to damage towers in the scroll view
        towerType = "Damage";
        MakeLists(Game);
        shopScreen.transform.Find("Filter/ScrollView").gameObject.SetActive(true);

        //Exit shop button
        Button xButton = shopScreen.transform.Find("Shop_XButton").GetComponent<Button>();

        xButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            Game.SetState(new gameIdleState());
        });
    
        //In shop upgrade filter buttons
        Button damageButton = shopScreen.transform.Find("Filter/Damage_Button").GetComponent<Button>();
        Button farmButton = shopScreen.transform.Find("Filter/Farm_Button").GetComponent<Button>();
        Button statButton = shopScreen.transform.Find("Filter/Stat_Button").GetComponent<Button>();

        damageButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            //Filter to show only damage upgrades
            towerType = "Damage";
            MakeLists(Game);
        });

        farmButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            //Filter to show only farm upgrades
            towerType = "Farm";
            MakeLists(Game);
        });

        statButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            //Filter to show only stat upgrades
            towerType = "Stat";
            MakeLists(Game);
        });
        
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
            if(tower.GetComponent<TowerParent>().TowerType == towerType)
            {
                    //instantiate button prefab under the scroll view, content
                GameObject button = GameObject.Instantiate(Game.GetTowerButtonPrefab(), shopScreen.transform.Find("Filter/ScrollView/Viewport/Content"));
                //set button image to tower image
                button.GetComponent<Image>().sprite = tower.GetComponent<TowerParent>().TowerImage;
                button.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Level " + tower.GetComponent<TowerParent>().Level.ToString();
                //add listener to button to open tower upgrade screen
                //clear all listeners first
                button.GetComponent<Button>().onClick.RemoveAllListeners();
                if(tower.GetComponent<TowerParent>().Unlocked)
                {
                    button.GetComponent<Button>().onClick.AddListener(() =>
                    {
                
                        Debug.Log("Tower Button Clicked: ");
                        //Play Button Sound
                        Game.PlayButtonClickSound();

                        // Open tower info screen
                        shopScreen.transform.Find("Tower_Info_Screen").gameObject.SetActive(true);
                        shopScreen.transform.Find("Tower_Info_Screen/Tower_Texts/Text_NotEnoughCoins").gameObject.SetActive(false);

                        // Set tower info screen texts to tower info
                        shopScreen.transform.Find("Tower_Info_Screen/Tower_Image").GetComponent<Image>().sprite = tower.GetComponent<TowerParent>().TowerImage;
                        shopScreen.transform.Find("Tower_Info_Screen/Tower_Texts/Tower_Name").GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerParent>().GetName();
                        shopScreen.transform.Find("Tower_Info_Screen/Tower_Texts/Attribute_Description").GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerParent>().GetAttributes();
                        shopScreen.transform.Find("Tower_Info_Screen/Tower_Texts/Text_Tower_Description").GetComponent<TextMeshProUGUI>().text = tower.GetComponent<TowerParent>().GetDescription(); 
                        
                        // Set the buttons functionlity
                        Button xButton = shopScreen.transform.Find("Tower_Info_Screen/X_Button").GetComponent<Button>();
                        Button buyButton = shopScreen.transform.Find("Tower_Info_Screen/Buy_Button").GetComponent<Button>();
                        //clear all listeners first
                        xButton.onClick.RemoveAllListeners();
                        buyButton.onClick.RemoveAllListeners();

                        // Close tower info screen
                        xButton.onClick.AddListener(() =>
                        {
                            Game.PlayButtonClickSound();
                            shopScreen.transform.Find("Tower_Info_Screen").gameObject.SetActive(false);
                        });

                        // When buy button is clicked
                        buyButton.onClick.AddListener(() =>
                        {
                            //Play Coin SFX
                            coinSFX.Play();
                            //check if player has enough coins to purchase tower
                            if(playerStats.coins >= tower.GetComponent<TowerParent>().TowerCost)
                            {
                                Debug.Log("Purchasing tower");
                                shopScreen.transform.Find("Tower_Info_Screen/Tower_Texts/Text_NotEnoughCoins").gameObject.SetActive(false);
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