using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpState : GameState
{
    private GameObject levelUpUI;
    private Button offer1;
    private Button offer2;
    private Button offer3;

    //Define all player stats
    public List<string> playerStats = new List<string> { "Health", "Speed", "Damage", "Attack Speed", "Profit Multiplier", "Experience Multiplier" };
    public override void EnterState(GameStateController Game)
    {
        //Get player stats from player stats script
        GameObject player = GameObject.FindWithTag("Player");
        PlayerStats stats = player.GetComponent<PlayerStats>();
        // Implementation for entering the level-up state
        //open level-up UI
        levelUpUI = Game.GetUpgradeScreen();
        levelUpUI.SetActive(true);
        //make 3 random stats to increase
        List<string> selectedStats = playerStats.OrderBy(s => UnityEngine.Random.value).Take(3).ToList();
        List<float> statIncreases = new List<float>();
        //make each stat increase between 15 and 30
        for (int i = 0; i < 3; i++)
        {
            statIncreases.Add(UnityEngine.Random.Range(15f, 30f));
        }
        //make buttons and set text
        offer1 = levelUpUI.transform.Find("Offer_1").GetComponent<Button>();
        offer2 = levelUpUI.transform.Find("Offer_2").GetComponent<Button>();
        offer3 = levelUpUI.transform.Find("Offer_3").GetComponent<Button>();
        TMP_Text offer1text = offer1.GetComponentInChildren<TMP_Text>();
        TMP_Text offer2text = offer2.GetComponentInChildren<TMP_Text>();   
        TMP_Text offer3text = offer3.GetComponentInChildren<TMP_Text>();
        offer1text.text = $"{selectedStats[0]} +{statIncreases[0].ToString("F2")}%";
        offer2text.text = $"{selectedStats[1]} +{statIncreases[1].ToString("F2")}%";
        offer3text.text = $"{selectedStats[2]} +{statIncreases[2].ToString("F2")}%";
        //add listeners to buttons
        offer1.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            //increase player stat
            stats.ModifyStat(selectedStats[0], statIncreases[0]);
            //exit level-up state
            //if place tower is not null that means we are in the building state
            //so go back into after leveling up
            if(Game.GetPlaceTower() != null)
            {
                Game.SetState(new BuildingState());
            }
            else
            {
                Game.SetState(new gameIdleState());
            }
            Debug.Log("Offer 1 Selected");
        });
        offer2.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            //increase player stat
            stats.ModifyStat(selectedStats[1], statIncreases[1]);
            //exit level-up state
            //if place tower is not null that means we are in the building state
            //so go back into after leveling up
            if(Game.GetPlaceTower() != null)
            {
                Game.SetState(new BuildingState());
            }
            else
            {
                Game.SetState(new gameIdleState());
            }
            Debug.Log("Offer 2 Selected");
        });
        offer3.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            //increase player stat
            stats.ModifyStat(selectedStats[2], statIncreases[2]);
            //exit level-up state
            //if place tower is not null that means we are in the building state
            //so go back into after leveling up
            if(Game.GetPlaceTower() != null)
            {
                Game.SetState(new BuildingState());
            }
            else
            {
                Game.SetState(new gameIdleState());
            }
            Debug.Log("Offer 3 Selected");
        });
        
        
        //pause time
        Time.timeScale = 0;
        
            
        
    


    }

    public override void UpdateState(GameStateController Game)
    {
        // Implementation for updating the level-up state

    }

    public override void ExitState(GameStateController Game)
    {
        // Implementation for exiting the level-up state
        //close level-up UI
        //reset listeners on buttons to prevent multiple triggers
        levelUpUI.SetActive(false);
        offer1.onClick.RemoveAllListeners();
        offer2.onClick.RemoveAllListeners();
        offer3.onClick.RemoveAllListeners();
        Time.timeScale = 1;
        //resume time
    }
}