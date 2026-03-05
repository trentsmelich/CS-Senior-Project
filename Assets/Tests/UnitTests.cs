using NUnit.Framework;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class UnitTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void UnitTestsSimplePasses()
    {
        GameObject testObject = new GameObject("PlayerStats_Test");
        PlayerStats stats = testObject.AddComponent<PlayerStats>();

        stats.maxHealth = 100f;
        stats.currentHealth = 50f;

        stats.ModifyStat("Health", 20f);

        Assert.That(stats.maxHealth, Is.EqualTo(120f).Within(0.001f));
        Assert.That(stats.currentHealth, Is.EqualTo(60f).Within(0.001f));

        

        Object.DestroyImmediate(testObject);
    }


    [Test]
    public void UnitTestsMainMenuStateController()
    {
        GameObject mainMenuStateController = new GameObject("mainMenuStateController_Test");
        MainMenuStateController mmsc = mainMenuStateController.AddComponent<MainMenuStateController>();

        // create all panels using gameobject and apply their menu types per what their panel is
        GameObject mainMenuPanel = new GameObject("MainMenuPanel");
        GameObject levelSelectPanel = new GameObject("LevelSelectPanel");
        GameObject settingsPanel = new GameObject("SettingsPanel");
        GameObject unlocksPanel = new GameObject("UnlocksPanel");

        // set those panels to the main menu state controller
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        unlocksPanel.SetActive(false);

        mmsc.mainMenuPanel = mainMenuPanel;
        mmsc.levelSelectPanel = levelSelectPanel;
        mmsc.settingsPanel = settingsPanel;
        mmsc.unlocksPanel = unlocksPanel;

        // Test the state transition by calling the SetState function with different states and checking if the correct panel is active
        
        // ---Testing Main Menu State On
        // create and set the buttons for main menu panel
        GameObject playButton = new GameObject("Play_Button");
        GameObject settingsButton = new GameObject("Settings_Button");
        GameObject unlocksButton = new GameObject("Unlocks_Button");
        GameObject exitButton = new GameObject("Exit_Button");

        playButton.transform.SetParent(mainMenuPanel.transform);
        settingsButton.transform.SetParent(mainMenuPanel.transform);
        unlocksButton.transform.SetParent(mainMenuPanel.transform);
        exitButton.transform.SetParent(mainMenuPanel.transform);

        playButton.AddComponent<Button>();
        settingsButton.AddComponent<Button>();
        unlocksButton.AddComponent<Button>();
        exitButton.AddComponent<Button>();

        // Set to Main Menu State On
        mmsc.SetState(new MainMenu());
        // Check if the main menu panel is active and other panels are inactive
        Assert.IsTrue(mainMenuPanel.activeSelf);
        Assert.IsFalse(levelSelectPanel.activeSelf);
        Assert.IsFalse(settingsPanel.activeSelf);
        Assert.IsFalse(unlocksPanel.activeSelf);



        // ---Testing Settings State On
        // create and set the buttons for settings panel
        GameObject optionsXButton = new GameObject("Options_XButton");

        optionsXButton.transform.SetParent(settingsPanel.transform);

        optionsXButton.AddComponent<Button>();

        // Set to Settings State On
        mmsc.SetState(new Settings());
        // Check if the settings panel is active and other panels are inactive
        Assert.IsTrue(settingsPanel.activeSelf);
        Assert.IsFalse(mainMenuPanel.activeSelf);
        Assert.IsFalse(levelSelectPanel.activeSelf);
        Assert.IsFalse(unlocksPanel.activeSelf);



        // ---Testing Level Select State On
        // create and set the buttons for level select panel
        GameObject level1Button = new GameObject("Level_1_Button");
        GameObject level2Button = new GameObject("Level_2_Button");
        GameObject level3Button = new GameObject("Level_3_Button");
        GameObject mainMenuButton = new GameObject("MainMenu_Button");
        GameObject tutorialButton = new GameObject("Tutorial_Button");

        level1Button.transform.SetParent(levelSelectPanel.transform);
        level2Button.transform.SetParent(levelSelectPanel.transform);
        level3Button.transform.SetParent(levelSelectPanel.transform);
        mainMenuButton.transform.SetParent(levelSelectPanel.transform);
        tutorialButton.transform.SetParent(levelSelectPanel.transform);

        level1Button.AddComponent<Button>();
        level2Button.AddComponent<Button>();
        level3Button.AddComponent<Button>();
        mainMenuButton.AddComponent<Button>();
        tutorialButton.AddComponent<Button>(); 

        // Set to Level Select State On
        mmsc.SetState(new LevelSelect());

        // Check if the level select panel is active and other panels are inactive
        Assert.IsTrue(levelSelectPanel.activeSelf);
        Assert.IsFalse(mainMenuPanel.activeSelf);
        Assert.IsFalse(settingsPanel.activeSelf);
        Assert.IsFalse(unlocksPanel.activeSelf);



        // ---Testing Main Menu State Back On
        mmsc.SetState(new MainMenu());
        // Check if the main menu panel is active and other panels are inactive
        Assert.IsTrue(mainMenuPanel.activeSelf);
        Assert.IsFalse(levelSelectPanel.activeSelf);
        Assert.IsFalse(settingsPanel.activeSelf);
        Assert.IsFalse(unlocksPanel.activeSelf);


        // Making sure the testing game objects are destroyed after the test
        Object.DestroyImmediate(mainMenuStateController);
        Object.DestroyImmediate(mainMenuPanel);
        Object.DestroyImmediate(levelSelectPanel);
        Object.DestroyImmediate(settingsPanel);
        Object.DestroyImmediate(unlocksPanel);
    }


    [Test]
    public void WaveStateTest()
    {
        GameObject testObject = new GameObject("WaveState_Test");
        GameObject countdownObject = new GameObject("Countdown_Test");
        TextMeshProUGUI countdownText = countdownObject.AddComponent<TextMeshProUGUI>();

        GameObject enemyPrefab = new GameObject("EnemyPrefab_Test");
        enemyPrefab.AddComponent<EnemyHealth>();
        enemyPrefab.AddComponent<EnemyAI>();

        float minSpawnRadius = 5f;
        float maxSpawnRadius = 7f;

        WavesState waveState = new WavesState(
            testObject.transform,
            20,
            10f,
            new[] { enemyPrefab },
            new[] { enemyPrefab },
            minSpawnRadius,
            maxSpawnRadius,
            0f,
            countdownText
        );

        MethodInfo spawnWaveMethod = typeof(WavesState).GetMethod("SpawnWave", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.That(spawnWaveMethod, Is.Not.Null, "SpawnWave method not found.");

        IEnumerator spawnWaveEnumerator = (IEnumerator)spawnWaveMethod.Invoke(waveState, null);
        while (spawnWaveEnumerator.MoveNext())
        {
        }

        Assert.That(EnemyHealth.GetNumEnemies(), Is.EqualTo(20), "Expected 20 enemies to be spawned.");

        var spawnedEnemies = Object.FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);
    
        foreach (EnemyHealth enemy in spawnedEnemies)
        {
            if (enemy.gameObject == enemyPrefab) continue;

            float distance = Vector3.Distance(enemy.transform.position, testObject.transform.position);
            Assert.That(distance, Is.InRange(minSpawnRadius, maxSpawnRadius), "Enemy spawned at distance " + distance + " which is outside the expected range.");
        }
    

        foreach (EnemyHealth spawnedEnemy in Object.FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None))
        {
            Object.DestroyImmediate(spawnedEnemy.gameObject);
        }

        Object.DestroyImmediate(enemyPrefab);
        Object.DestroyImmediate(countdownObject);
        Object.DestroyImmediate(testObject);
        EnemyHealth.resetEnemyCounts();
    }
}