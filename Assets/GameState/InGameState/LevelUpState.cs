using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpState : GameState
{
    private GameObject levelUpUI;

    
    public List<string> playerStats = new List<string> { "Health", "Speed", "Damage", "Attack Speed", "Profit Multiplier", "Experience Multiplier" };
    public override void EnterState(GameStateController Game)
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerStats stats = player.GetComponent<PlayerStats>();
        // Implementation for entering the level-up state
        //open level-up UI
        levelUpUI = Game.GetUpgradeScreen();
        levelUpUI.SetActive(true);
        
        List<string> selectedStats = playerStats.OrderBy(s => UnityEngine.Random.value).Take(3).ToList();
        List<float> statIncreases = new List<float>();
        //make each stat increase between 1 and 10
        for (int i = 0; i < 3; i++)
        {
            statIncreases.Add(UnityEngine.Random.Range(1f, 10f));
        }

        Button offer1 = levelUpUI.transform.Find("Offer_1").GetComponent<Button>();
        Button offer2 = levelUpUI.transform.Find("Offer_2").GetComponent<Button>();
        Button offer3 = levelUpUI.transform.Find("Offer_3").GetComponent<Button>();
        TMP_Text offer1text = offer1.GetComponentInChildren<TMP_Text>();
        TMP_Text offer2text = offer2.GetComponentInChildren<TMP_Text>();   
        TMP_Text offer3text = offer3.GetComponentInChildren<TMP_Text>();
        offer1text.text = $"{selectedStats[0]} +{statIncreases[0].ToString("F2")}";
        offer2text.text = $"{selectedStats[1]} +{statIncreases[1].ToString("F2")}";
        offer3text.text = $"{selectedStats[2]} +{statIncreases[2].ToString("F2")}";
        
        offer1.onClick.AddListener(() =>
        {
            //increase player stat

            stats.ModifyStat(selectedStats[0], statIncreases[0]);
            //exit level-up state
            Game.SetState(new gameIdleState());
            Debug.Log("Offer 1 Selected");
        });
        offer2.onClick.AddListener(() =>
        {
            //increase player stat

            stats.ModifyStat(selectedStats[1], statIncreases[1]);
            //exit level-up state
            Game.SetState(new gameIdleState());
            Debug.Log("Offer 2 Selected");
        });
        offer3.onClick.AddListener(() =>
        {
            //increase player stat

            stats.ModifyStat(selectedStats[2], statIncreases[2]);
            //exit level-up state
            Game.SetState(new gameIdleState());
            Debug.Log("Offer 3 Selected");
        });
        
        Time.timeScale = 0;
            
        //pause time
        

        // Set up level-up options
        //make random level-up options
        //randomly pick stats and pick number from 1-10 to increase by


    }

    public override void UpdateState(GameStateController Game)
    {
        // Implementation for updating the level-up state

    }

    public override void ExitState(GameStateController Game)
    {
        // Implementation for exiting the level-up state
        //close level-up UI
        levelUpUI.SetActive(false);
        Time.timeScale = 1;
        //resume time
    }
}