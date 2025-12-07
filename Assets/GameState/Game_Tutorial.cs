using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game_Tutorial : MonoBehaviour
{
    public GameObject[] tutorialSteps;
    private int currentStep;
    //private const string PREF_TUTORIAL_DONE = "Tutorial_Completed";
    public GameObject GameTutorialObject;
    public GameObject PlayerUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If tutorial was already completed, skip showing
        /*if (PlayerPrefs.GetInt(PREF_TUTORIAL_DONE, 0) == 1)
        {
            // Set tutorial UI inactive
            GameTutorialObject.SetActive(false);
            // Not show the tutorial and ingnore rest the code
            return;
        }*/

        //Setups for the Game Tutorial
        Time.timeScale = 0; 
        currentStep = 0;
        GameTutorialObject.SetActive(true);
        PlayerUI.SetActive(false);
        ShowTutorialStep(currentStep);
    }

    // Update is called once per frame
    void Update()
    {
        // If the player clicked on the left mouse button, go to the next step
        if (Input.GetMouseButtonDown(0))
        {
            NextStep();
        }
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
        //PlayerPrefs.SetInt(PREF_TUTORIAL_DONE, 1);
        //PlayerPrefs.Save();

        // Disable tutorial UI
        GameTutorialObject.SetActive(false);

        // Set the showing steps gameobjects back to where it was

        // Put the time back to normal
        PlayerUI.SetActive(true);
        Time.timeScale = 1; 
    }
}
