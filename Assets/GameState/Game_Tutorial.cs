using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game_Tutorial : MonoBehaviour
{
    public GameObject[] tutorialSteps;
    public Button[] nextButtons;
    private int currentStep;
    private const string PREF_TUTORIAL_DONE = "Tutorial_Completed";
    public GameObject GameTutorialObject;
    public GameObject PlayerUI;
    private AudioSource buttonSFX;

    //String Variables for texts of different steps
    private string stepOneText;
    private string stepTwoText;
    private string stepThreeText;
    private string stepFourText;
    private string stepFiveText;
    private string stepSixText;
    private string stepSevenText;
    private string stepEightText;
    private string stepNineText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If tutorial was already completed, skip showing in level 1
        if (PlayerPrefs.GetInt(PREF_TUTORIAL_DONE, 0) == 1)
        {
            // Set tutorial UI inactive
            GameTutorialObject.SetActive(false);
            // Not show the tutorial and ingnore rest the code
            return;
        }

        // Assign every button the same functionality of next step
        foreach (Button nextButton in nextButtons)
        {
            nextButton.onClick.AddListener(NextFunctionality);
        }

        //Set all the texts for the tutorial
        SetTexts();

        //Setups for the Game Tutorial
        Time.timeScale = 0; 
        currentStep = 0;
        GameTutorialObject.SetActive(true);
        PlayerUI.SetActive(false);
        buttonSFX = GameObject.Find("SFX/Tutorial_ClickSFX").GetComponent<AudioSource>();
        ShowTutorialStep(currentStep);
    }

    private void NextFunctionality()
    {
        buttonSFX.Play();
        NextStep();
    }

    private void SetTexts()
    {
        
        //Setting the step texts
        stepOneText = "Welcome to Cycle Of The Dead!" + "\n\n" + "This is a quick tutorial of the game" + "\n\n"
        + "This is a 2D Top Down defensive survival video game with a roguelike aspect. The main goal is to defeat as much enemies as you can and survival as long as possible. " 
        + "During the game, you can choose to buy different items from the shop or getting a upgrade offer to help you get stronger as you process the game.";

        stepTwoText = "Player Control" + "\n\n"
        + "* Up: W" + "\n" 
        + "* Down: S" + "\n" 
        + "* Left: A" + "\n" 
        + "* Right: D" + "\n" 
        + "* Attack: Left Mouse" + "\n";

        stepThreeText = "Player UI" + "\n\n"
        + "* Coin Counter: display the total amount of coins you collected." + "\n"
        + "* Enemy Counter: display the total number of enemies you have killed." + "\n"
        + "* Timer: display the total time have passed (Min/Sec)." + "\n"
        + "* Wave Timer: display the wave count down and enemies left." + "\n"
        + "* XP Bar: display the current/total experience. " + "\n"
        + "* Health Bar: display the current/total health." + "\n";

        stepFourText = "Paused Menu" + "\n\n"
        + "* Access with ESC Key." + "\n"
        + "* Resume: go back to the current game." + "\n"
        + "* Options: changed the Music/SFX settings of the game." + "\n"
        + "* Quit: go back to the main menu." + "\n";

        stepFiveText = "Upgrade Offer" + "\n\n"
        + "* Everytime when the player level up by getting enough XP, an upgrade Offer Screen will pop up. "  + "\n"
        + "* The Upgrade Offer screen will offer the player 3 different random upgrade offers, and the player is able to pick one of the three. "  + "\n"
        + "* That three offers include speed, damage, health, attack speed, profit multipllier, etc."  + "\n";

        stepSixText = "Shop" + "\n\n"
        + "* Access with F Key" + "\n"
        + "* Shop allows player to buy different items with coins." + "\n"
        + "* Damage, Farm, and Stat buttons help the player to filter different type of items." + "\n"
        + "* Player is able to buy towers for better defense, farms for generating coins, and stats for modify the player's stats. " + "\n"
        + "* The dark item buttons shows the item is locked, but will be unlocked after the requirements. " + "\n"
        + "* A red message will display if the player don't have enough coins for the item." + "\n";
        
        stepSevenText = "Game Over Screen" + "\n\n"
        + "* When the player died in the game, a Game Over screen will display for showing the player has lose the game." + "\n"
        + "* Player will see the total time and the total enemies killed." + "\n"
        + "* Player will able to choose go back to main menu or restart the level." + "\n";

        stepEightText = "Enemy Waves" + "\n\n"
        + "* The longer the player survives, the more enemies the next waves." + "\n"
        + "* There will be a boss enemy in a certain numbers of waves." + "\n"
        + "* Each enemy will drop a coin after the player killed them." + "\n";

        stepNineText = "The End of the Tutorial" + "\n\n"
        + "Congratulations!" + "\n"
        + "You have finished the tutorial!" + "\n"
        + "Good Luck!" + "\n";

        // Set the texts to the game object texts
        GameTutorialObject.transform.Find("Step1_Introduction/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = stepOneText;
        GameTutorialObject.transform.Find("Step2_PlayerControl/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = stepTwoText;
        GameTutorialObject.transform.Find("Step3_PlayerUI/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = stepThreeText;
        GameTutorialObject.transform.Find("Step4_Pause/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = stepFourText;
        GameTutorialObject.transform.Find("Step5_Upgrade/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = stepFiveText;
        GameTutorialObject.transform.Find("Step6_Shop/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = stepSixText;
        GameTutorialObject.transform.Find("Step7_GameOver/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = stepSevenText;
        GameTutorialObject.transform.Find("Step8_Waves/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = stepEightText;
        GameTutorialObject.transform.Find("Step9_EndOfTutorial/Paragraph_Text").GetComponent<TextMeshProUGUI>().text = stepNineText;

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

        // Disable tutorial UI
        GameTutorialObject.SetActive(false);
        // Put the time back to normal
        PlayerUI.SetActive(true);
        Time.timeScale = 1; 
    }
}
