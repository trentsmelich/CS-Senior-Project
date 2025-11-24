using UnityEngine;
using UnityEngine.UI;

public class InShopState : GameState
{
    private GameObject shopScreen;
    //private GameObject[] towers;
    //private RectTransform listParent;

    public override void EnterState(GameStateController Game)
    {
        //towers = Game.GetTowers();
        shopScreen = Game.GetShopScreen();
        
        //listParent = shopScreen.transform.Find("ListParent").gameObject.GetComponent<RectTransform>();

        /*foreach (GameObject tower in towers)
        {
            GameObject towerIcon = GameObject.Instantiate(tower, listParent);
            
        }*/

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
            
            //shopScreen.GetComponent<Shop>().FilterUpgrades("Damage");
        });

        farmButton.onClick.AddListener(() =>
        {
            //Filter to show only farm upgrades
            FilterButton(1);
            //shopScreen.GetComponent<Shop>().FilterUpgrades("Farm");
        });

        statButton.onClick.AddListener(() =>
        {
            //Filter to show only stat upgrades
            FilterButton(2);
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

    private void MakeLists()
    {
        
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