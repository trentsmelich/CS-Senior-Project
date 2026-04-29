// Libraries
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Author:Jia
//Description: This script manages the in game tutorial, guiding players through the game's mechanics and UI.
public class TutorialState : GameState
{
    // Declare lists for steps, playerPrefab, UI, SFX, and display game objects
    private GameObject[] tutorialSteps;
    private Button nextButton;
    private Button backButton;
    private int currentStep;
    private const string PREF_TUTORIAL_DONE = "Tutorial_Completed";
    private GameObject GameTutorialObject;
    private AudioSource buttonSFX;

    //String Variables for texts of different steps
    private string step1Text;
    private string step2Text;
    private string step3Text;
    private string step4Text;
    private string step5Text;
    private string step6Text;
    private string step7Text;
    private string step8Text;
    private string step9Text;
    private string step10Text;
    private string step11Text;
    private string step12Text;
    private string step13Text;
    private string step14Text;
    private string step15Text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void EnterState(GameStateController Game)
    {
        // Setting up the tutorial variables and other UI
        tutorialSteps = Game.GetTutorialSteps();
        nextButton = Game.GetTutorialNextButton();
        backButton = Game.GetTutorialBackButton();
        GameTutorialObject = Game.GetGameTutorialObject();
        nextButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        // If tutorial was already completed, skip showing in level 1
        if (PlayerPrefs.GetInt(PREF_TUTORIAL_DONE, 0) == 1)
        {
            // Set tutorial UI inactive
            GameTutorialObject.SetActive(false);
            // Not show the tutorial and ingnore rest the code
            return;
        }

        // Assign every button the same functionality of next step

        nextButton.onClick.AddListener(NextFunctionality);
        backButton.onClick.AddListener(BackFunctionality);
    

        //Set all the texts for the tutorial
        SetTexts();

        //Setups for the Game Tutorial
        Time.timeScale = 0; 
        currentStep = 0;
        GameTutorialObject.SetActive(true);
        Game.ShowPlayerUI(false);
        buttonSFX = GameObject.Find("SFX/Tutorial_ClickSFX").GetComponent<AudioSource>();
        ShowTutorialStep(currentStep);
    }

    public override void UpdateState(GameStateController Game)
    {
        // Implementation for updating the game over state
    }

    private void BackFunctionality()
    {
        // Play button SFX and go to previous step
        buttonSFX.Play();
        BackStep();
    }

    private void NextFunctionality()
    {
        // Play button SFX and go to next step
        buttonSFX.Play();
        NextStep();
    }

    private void SetTexts()
    {
        
        //Setting the step texts from the start to the end of the tutorial
        step1Text = "Welcome to Cycle Of The Dead!" + "\n\n" + "This is a quick tutorial of the game" + "\n\n"
        + "This is a 2D Top Down defensive survival video game with a roguelike aspects. The main goal is to defeat as much enemies as you can and survival as long as possible. " 
        + "During the game, you can choose to buy different items from the shop and getting other different type of upgrades to help you get stronger as you process the game.";

        step2Text = "Story" + "\n\n"
        + "* Start from the beginning of the game, you will be introduced to the story and the world you are in." + "\n"
        + "* If you want to see the story, either press the space bar or the left mouse button to continue." + "\n"
        + "* If you want to skip the story, press the green skip button in the right down corner." + "\n";

        step3Text = "Player Control" + "\n\n"
        + "* Up: W" + "\n" 
        + "* Down: S" + "\n" 
        + "* Left: A" + "\n" 
        + "* Right: D" + "\n" 
        + "* Attack: Left Mouse" + "\n";

        step4Text = "Player UI" + "\n\n"
        + "* Coin Counter: display the total amount of coins you collected." + "\n"
        + "* Enemy Counter: display the total number of enemies you have killed." + "\n"
        + "* Timer: display the total time that has passed (Min/Sec)" + "\n"
        + "* Wave Timer: displays the wave countdown and the number of enemies left." + "\n"
        + "* XP Bar: display the current/total experience. " + "\n"
        + "* Health Bar: display the current/total health." + "\n";

        step5Text = "Paused Menu" + "\n\n"
        + "* Access by pressing ESC Key." + "\n"
        + "* Resume: go back to the current game." + "\n"
        + "* Options: change the Music/SFX and keybinds settings of the game." + "\n"
        + "* Quit: go back to the main menu." + "\n";

        step6Text = "Upgrade Offer" + "\n\n"
        + "* Every time oncet he player level up by getting enough XP, an upgrade Offer Screen will display. "  + "\n"
        + "* The Upgrade Offer screen will offer the player 3 different random upgrade offers, and the player is able to pick one of the three. "  + "\n"
        + "* That three offers include speed, damage, health, attack speed, profit multipllier, etc."  + "\n";

        step7Text = "Shop" + "\n\n"
        + "* Access by pressing F Key" + "\n"
        + "* Shop allows player to buy different items using coins." + "\n"
        + "* Damage, Farm, and Stat buttons help the player to filter different type of items." + "\n"
        + "* Player is able to buy towers for better defense, farms for generating coins, and stats for modify the player's stats. " + "\n"
        + "* The dark item buttons shows the item is locked, but will be unlocked after the requirements. " + "\n"
        + "* A red message will display if the player don't have enough coins to buy the item." + "\n";

        step8Text = "Building State" + "\n\n"
        + "* The player will enter the building state after the buy button is pressed when they have enough coins." + "\n"
        + "* During the building state, use left mouse button to place the building and right mouse button to cancel placing." + "\n"
        + "* If the player successfully place the building, the coins will be deducted and the building will be placed. " + "\n"
        + "* If the player cancel the placing, the building will be canceled and the coins will be given back." + "\n"
        + "* The player will back to the main game after placing or canceling the building." + "\n";

        step9Text = "Destroy State" + "\n\n"
        + "* The player will enter the destroy state after pressing the B key on the keyboard." + "\n"
        + "* During the destroy state, use right mouse button to destroy the building on top of the building you want to destroy." + "\n"
        + "* After the player successfully destroy the building, 80% of the coins will be given back and the building will be removed." + "\n"
        + "* Press the B key on the keyboard to exit the destroy state and return to the shop." + "\n";

        step10Text = "Enemy Waves" + "\n\n"
        + "* The more waves that the player survives, the more enemies spawn and stronger in the next waves." + "\n"
        + "* There will be a boss enemy in a certain numbers of waves (except boss level)." + "\n"
        + "* Each enemy will drop a coin, gain experience points, and a small chance to drop power up items such as health, speed, or other attributes after the enemy is killed." + "\n";

        step11Text = "Power Ups" + "\n\n"
        + "* Power ups are special items that can be collected by the player to enhance their abilities." + "\n"
        + "* CoolDown Power Up: increase the attack speed." + "\n"
        + "* Magnet Power Up: increase the attraction range for nearby coins" + "\n"
        + "* Poison Power Up: add a poison effect to the player's attack, dealing damage over time." + "\n"
        + "* Health Power Up: restore a portion of the player's health." + "\n"
        + "* Shield Power Up: provide temporary invincibility to the player." + "\n"
        + "* Speed Power Up: increase the player's movement speed." + "\n";

        step12Text = "Game Over Screen" + "\n\n"
        + "* After the player dies in the game, a game over screen will display for showing the player has lost the game." + "\n"
        + "* Player will see the total time and the total enemies killed." + "\n"
        + "* Player will able to choose go back to main menu or restart the level." + "\n";

        step13Text = "Score Update" + "\n\n"
        + "* After the player died and the game is over, the player's longest survival time and highest kill count will be updated." + "\n"
        + "* The player can check the longest survival time and highest kill count in the level select of the main menu." + "\n"
        + "* The game will only update the longest survival time and highest kill count than the previous record." + "\n";

        step14Text = "Unlocks" + "\n\n"
        + "* The locked buildings will be shown as dark buttons and cannot be accessed by the player until they are unlocked." + "\n"
        + "* As the player progresses through the game, they will unlock new buildings by completing certain requirements such as killing a certain number of enemies, place the same building number of times, etc." + "\n"
        + "* If the player meets the requirements, the player can see it in unlock screen from the main menu after the game over." + "\n"
        + "* To see the unlocked buildings, you can view it by pressing the unlocks button in the main menu." + "\n";

        step15Text = "Congratulations!" + "\n\n"
        + "You have finished the tutorial!" + "\n"
        + "Good Luck!" + "\n";

        // Set the texts to the game object texts
        GameTutorialObject.transform.Find("Step1_Introduction/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step1Text;
        GameTutorialObject.transform.Find("Step2_Story/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step2Text;
        GameTutorialObject.transform.Find("Step3_PlayerControl/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step3Text;
        GameTutorialObject.transform.Find("Step4_PlayerUI/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step4Text;
        GameTutorialObject.transform.Find("Step5_Pause/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step5Text;
        GameTutorialObject.transform.Find("Step6_Upgrade/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step6Text;
        GameTutorialObject.transform.Find("Step7_Shop/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step7Text;
        GameTutorialObject.transform.Find("Step8_BuildingState/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step8Text;
        GameTutorialObject.transform.Find("Step9_DestroyState/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step9Text;
        GameTutorialObject.transform.Find("Step10_Waves/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step10Text;
        GameTutorialObject.transform.Find("Step11_PowerUps/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step11Text;
        GameTutorialObject.transform.Find("Step12_GameOver/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step12Text;
        GameTutorialObject.transform.Find("Step13_ScoreUpdate/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step13Text;
        GameTutorialObject.transform.Find("Step14_Unlocks/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step14Text;
        GameTutorialObject.transform.Find("Step15_EndOfTutorial/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = step15Text;
    }

    private void BackStep()
    {
        // Move to the previous step
        currentStep--;
        if (currentStep < 0)
        {
            currentStep = 0;
        }
        ShowTutorialStep(currentStep);
    }

    private void NextStep()
    {
        // Move to the next step
        currentStep++;
        // Ends the tutorial when finished all teh tutorialSteps
        if (currentStep >= tutorialSteps.Length)
        {
            Tutorial_Completed();
            return;
        }
        ShowTutorialStep(currentStep);
        
    }

    private void ShowTutorialStep(int currentStepIndex)
    {
        // Set everything false
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            tutorialSteps[i].SetActive(false);
        }

        // Except that one gameobject needs to be show
        tutorialSteps[currentStepIndex].SetActive(true);
    }

    private void Tutorial_Completed()
    {
        // Finish the tutorial
        PlayerPrefs.SetInt(PREF_TUTORIAL_DONE, 1);
        PlayerPrefs.Save();
    }

    public override void ExitState(GameStateController Game)
    {
        // Disable tutorial UI
        GameTutorialObject.SetActive(false);
        // Put the time back to normal
        Game.ShowPlayerUI(true);
        Time.timeScale = 1; 
    }
}
