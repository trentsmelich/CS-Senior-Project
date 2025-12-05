using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Unlocks : MainMenuState
{
    GameObject unlocksPanel;
    private GameObject[] towers;
    private String towerType;

    public override void EnterState(MainMenuStateController Main)
    {
        unlocksPanel = Main.GetUnlocksPanel();
        towers = Main.GetTowers();
        unlocksPanel.SetActive(true);

        towerType = "Damage";
        MakeLists(Main);

        Button backButton = unlocksPanel.transform.Find("Unlocks_XButton").GetComponent<Button>();


        backButton.onClick.AddListener(() =>
        {
            Main.SetState(new MainMenu());
            Debug.Log("Back Button Clicked");
        });

        Button damageButton = unlocksPanel.transform.Find("Damage_Button").GetComponent<Button>();
        Button farmButton = unlocksPanel.transform.Find("Farm_Button").GetComponent<Button>();
        Button statButton = unlocksPanel.transform.Find("Stat_Button").GetComponent<Button>();

        damageButton.onClick.AddListener(() =>
        {
            //Filter to show only damage upgrades
            towerType = "Damage";
            MakeLists(Main);
        });

        farmButton.onClick.AddListener(() =>
        {
            //Filter to show only farm upgrades
            towerType = "Farm";
            MakeLists(Main);
        });

        statButton.onClick.AddListener(() =>
        {
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

                // If the tower is locked, show locked color dark grey
                // if ()
                /*Color32 darkGrey = new Color32(50, 50, 50, 255); //color32 means RGBA color in a 32 bits format
                button.GetComponent<Image>().color = darkGrey;*/

                // When the tower button with image is clicked
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    
                    // If the tower is locked, do nothing and return
                    // if () return;

                    // Open tower info screen
                    unlocksPanel.transform.Find("Tower_Info_Screen").gameObject.SetActive(true);

                    // Set tower info screen texts to tower info
                    unlocksPanel.transform.Find("Tower_Info_Screen/Tower_Image").GetComponent<Image>().sprite = tower.GetComponent<TowerParent>().TowerImage;

                    // set the attribute text box differently depending on its tower type
                    if(tower.GetComponent<TowerParent>().TowerType == "Damage")
                    {
                        // Set the tower attack attributes
                        unlocksPanel.transform.Find("Tower_Info_Screen/Tower_Texts/Attribute_Description").GetComponent<TextMeshProUGUI>().text = 
                        "Attack Attributes\n" +
                        "Level:"+ "<pos=125>" + tower.GetComponent<TowerParent>().Level.ToString() + "</pos>\n" + "\n" +
                        "Damage:" + "<pos=125>" + tower.GetComponent<TowerParent>().TowerDamage.ToString() + "</pos>\n" + "\n" +
                        "Range:" + "<pos=125>" + tower.GetComponent<TowerParent>().TowerRange.ToString() + "</pos>\n" + "\n" +
                        "Speed:" + "<pos=125>" + tower.GetComponent<TowerParent>().Speed.ToString() + "</pos>\n" + "\n" +
                        "Cooldown:" + "<pos=125>" + tower.GetComponent<TowerParent>().AttackCooldown.ToString() + "</pos>\n" + "\n" +
                        "Cost:" + "<pos=125>" + tower.GetComponent<TowerParent>().TowerCost.ToString() + "</pos>";
                    }
                    else if (tower.GetComponent<TowerParent>().TowerType == "Farm")
                    {
                        // Set the farm attributes
                        unlocksPanel.transform.Find("Tower_Info_Screen/Tower_Texts/Attribute_Description").GetComponent<TextMeshProUGUI>().text = 
                        "Farming Attributes\n" +
                        "Level:"+ "<pos=125>" + tower.GetComponent<TowerParent>().Level.ToString() + "</pos>\n" + "\n" +
                        "Profit:" + "<pos=125>" + "000" + "</pos>\n" + "\n" +
                        "Cost:" + "<pos=125>" + tower.GetComponent<TowerParent>().TowerCost.ToString() + "</pos>";
                    }
                    else
                    {
                        // Set the stat modifier attributes
                        unlocksPanel.transform.Find("Tower_Info_Screen/Tower_Texts/Attribute_Description").GetComponent<TextMeshProUGUI>().text = 
                        "Stat Attributes\n" +
                        "Level:"+ "<pos=125>" + tower.GetComponent<TowerParent>().Level.ToString() + "</pos>\n" + "\n" +
                        "Modify:" + "<pos=125>" + "000" + "</pos>\n" + "\n" +
                        "Cost:" + "<pos=125>" + tower.GetComponent<TowerParent>().TowerCost.ToString() + "</pos>";
                        
                    }

                    

                    // Close tower info screen
                    Button xButton = unlocksPanel.transform.Find("Tower_Info_Screen/X_Button").GetComponent<Button>();
                    xButton.onClick.AddListener(() =>
                    {
                        unlocksPanel.transform.Find("Tower_Info_Screen").gameObject.SetActive(false);
                    });
                    

                });
                
            }
            

            
        }

    }


    public override void ExitState(MainMenuStateController Main)
    {
        unlocksPanel.SetActive(false);
    }
    
    
}
