// Libraries
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
//Author:Jia
//Description: This script manages the keybinding and UI

public class KeybindUI : MonoBehaviour
{
    public TextMeshProUGUI keyText;
    public GameObject keybindSetting;
    public GameObject keybindOverlayScreen;
    public string keyName = "";
    private bool isListening = false;
    private Button keybindButton;

    private void Start()
    {
        // Find the buttons for each keybind and set their listeners
        keybindButton = keybindSetting.transform.Find("Rebinding_" + keyName).GetComponent<Button>();
        keybindButton.onClick.RemoveAllListeners();

        keybindButton.onClick.AddListener(() =>
        {
            StartRebind(keyName);
        });
        
        // Set the keyName based on the keybindType and load the current keybind
        keyText.text = "Press any key for " + keyName + "...";
        LoadKeybind();
    }

    void Update()
    {
        // Listen for key input when rebinding
        if (isListening)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    SetKey(keyName, key);
                    isListening = false;
                    break;
                }
            }
        }
    }

    // Called by UI Button
    public void StartRebind(string newKeyName)
    {
        // Show the overlay screen and start listening for key input
        keyName = newKeyName;

        keybindOverlayScreen.SetActive(true);
        isListening = true;

        keybindOverlayScreen.transform.Find("Instruction_Text").GetComponent<TextMeshProUGUI>().text = "Press any key for " + keyName + "...";
        
    }


    void SetKey(string keyName, KeyCode newKey)
    {
        // Save the new keybind to PlayerPrefs
        PlayerPrefs.SetString(keyName, newKey.ToString());
        PlayerPrefs.Save();

        LoadKeybind();
        keybindOverlayScreen.SetActive(false);
    }

    void LoadKeybind()
    {
        // Load the keybind from PlayerPrefs and update the UI text
        string key = "";
        if (keyName == "Pause")
        {
            key = PlayerPrefs.GetString("Pause", KeyCode.Escape.ToString());
        }
        else if (keyName == "Shop")
        {
            key = PlayerPrefs.GetString("Shop", KeyCode.F.ToString());
        }
        else if (keyName == "Destroy")
        {
            key = PlayerPrefs.GetString("Destroy", KeyCode.B.ToString());
        }
        else if (keyName == "Attack")
        {
            key = PlayerPrefs.GetString("Attack", KeyCode.Mouse0.ToString());
        }
        else if (keyName == "MoveUp")
        {
            key = PlayerPrefs.GetString("MoveUp", KeyCode.W.ToString());
        }
        else if (keyName == "MoveDown")
        {
            key = PlayerPrefs.GetString("MoveDown", KeyCode.S.ToString());
        }
        else if (keyName == "MoveLeft")
        {
            key = PlayerPrefs.GetString("MoveLeft", KeyCode.A.ToString());
        }
        else if (keyName == "MoveRight")
        {
            key = PlayerPrefs.GetString("MoveRight", KeyCode.D.ToString());
        }

        keyText.text = key;
    }
}