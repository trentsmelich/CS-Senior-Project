
// Libraries
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//Author:Jia
//Description: This script tells the story to the player before the level starts, including displaying story text, handling user interactions, and managing transitions to the next state.

public class StoryState : GameState
{
    // Variables for the Story State
    private TextMeshProUGUI storyText;
    private string[] storyLines;
    private float textDisplaySpeed = 0.05f;
    private int index;

    public override void EnterState(GameStateController Game)
    {
        // Implementation for entering the story state
        // Get references to the story text and lines from the GameStateController
        storyText = Game.GetStoryText();
        storyLines = Game.GetStoryLines();

        // Show the story UI and hide the player UI, and pause the game time
        Game.SetStoryUI(true);
        Game.ShowPlayerUI(false);
        Time.timeScale = 0;

        // Clear the story text and start the dialogue
        storyText.text = "";
        StartDialogue(Game);
    }

    public override void UpdateState(GameStateController Game)
    {
        // Implementation for updating the story state
        //If space is pressed or left mouse is clicked, display the next line of the story or transition to the next state if all lines have been displayed

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            storyText.text = storyLines[index]; // do the full line of text 
            index++;

            if (index < storyLines.Length)
            {
                Game.StopAllCoroutines(); // stop the current typing coroutine if it's still running (if the player clicks before the line is fully displayed)
                storyText.text = ""; // clear the text to prepare for the next line
                Game.StartCoroutine(TypeLine()); // start the coroutine to type out the next line of the story
            }
            else
            {
                Game.SetState(new gameIdleState()); // transition to the next state (e.g., gameIdleState) after all story lines have been displayed
            }
        }
    }

    public override void ExitState(GameStateController Game)
    {
        // Implementation for exiting the story state
        storyText.text = "";
        Game.SetStoryUI(false);
        Game.ShowPlayerUI(true);
        Time.timeScale = 1;
    }

    void StartDialogue(GameStateController Game)
    {
        //start the dialogue by initializing the index and starting the coroutine to type out the first line of the story
        index = 0;
        Game.StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // Implementation for typing out the current line of the story with a delay between each character
        foreach (char c in storyLines[index])
        {
            storyText.text += c;
            yield return new WaitForSecondsRealtime(textDisplaySpeed);
        }

    }

}
