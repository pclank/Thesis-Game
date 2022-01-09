using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// ************************************************************************************
// Pause Menu
// ************************************************************************************

public class PauseMenu : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public Button resume_button;
    public Button show_tutorials_button;
    public Button settings_button;
    public Button exit_button;
    public Button close_tutorials_menu_button;                  // Button to Close Tutorials Menu
    public Button close_settings_menu_button;                   // Button to Close Settings Menu

    public GameObject camera_object;
    public GameObject base_tutorial_object;                     // Base Tutorial Object
    public GameObject menu_ui;
    public GameObject settings_ui;
    public GameObject tutorials_ui;                             // Tutorials Menu GameObject

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;

    private bool menu_enabled = false;                          // Whether Pause Menu is Enabled

    // Pause Game
    private void pauseGame()
    {
        Time.timeScale = 0;
    }

    // Unpause Game
    private void unpauseGame()
    {
        Time.timeScale = 1;
    }

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");           // Get Player GameObject

        // Disable Menus on Start

        menu_ui.SetActive(false);
        settings_ui.SetActive(false);
        tutorials_ui.SetActive(false);

        // Add Button Listeners

        resume_button.onClick.AddListener(() => buttonCallBack(resume_button));
        show_tutorials_button.onClick.AddListener(() => buttonCallBack(show_tutorials_button));
        settings_button.onClick.AddListener(() => buttonCallBack(settings_button));
        exit_button.onClick.AddListener(() => buttonCallBack(exit_button));
        close_tutorials_menu_button.onClick.AddListener(() => buttonCallBack(close_tutorials_menu_button));
        close_settings_menu_button.onClick.AddListener(() => buttonCallBack(close_settings_menu_button));
    }

    // Update is called once per frame
    void Update()
    {
        // Check for Pause Menu Enable

        if (!menu_enabled && Input.GetKeyUp(KeyCode.Escape) && !player_object.GetComponent<main_inventory>().isInventoryOpen() && !player_object.GetComponent<MainJournal>().isJournalOpen())
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = true;         // Freeze Camera Controller

            Cursor.lockState = CursorLockMode.None;                                 // Unlock Cursor
            Cursor.visible = true;                                                  // Make Cursor Visible

            menu_ui.SetActive(true);                                                // Enable Pause Menu GameObject

            menu_enabled = true;                                                    // Set Menu as Enabled

            pauseGame();
        }

        // Check for Pause Menu Disable

        else if (menu_enabled && Input.GetKeyUp(KeyCode.Escape))
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = false;    // Unfreeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = false;        // Unfreeze Camera Controller

            Cursor.lockState = CursorLockMode.Locked;                               // Lock Cursor to Center
            Cursor.visible = false;                                                 // Hide Cursor

            tutorials_ui.SetActive(false);                                          // Disable Tutorials Menu GameObject
            settings_ui.SetActive(false);                                           // Disable Settings Menu GameObject
            menu_ui.SetActive(false);                                               // Disable Pause Menu GameObject

            GameObject.FindWithTag("Player").GetComponent<SubtitleControl>().hidePlaceholder(); // Show Subtitles PLACEHOLDER

            menu_enabled = false;                                                   // Set Menu as Disabled

            unpauseGame();
        }

        // Weird Mouse Click Bug Magic Fix

        if (menu_enabled)
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = true;         // Freeze Camera Controller
        }
    }

    // Button CallBack Function

    private void buttonCallBack(Button but_pressed)
    {
        // Resume Button Pressed
        if (but_pressed == resume_button)
        {
            Cursor.lockState = CursorLockMode.Locked;                               // Lock Cursor to Center
            Cursor.visible = false;                                                 // Hide Cursor

            menu_ui.SetActive(false);                                               // Disable Pause Menu GameObject

            menu_enabled = false;                                                   // Set Menu as Disabled

            unpauseGame();
        }

        // Show Tutorials Button Pressed
        else if (but_pressed == show_tutorials_button)
        {
            tutorials_ui.SetActive(true);

            gameObject.GetComponent<TutorialsMenu>().buildTutorialPage(base_tutorial_object.GetComponent<BaseTutorial>().getTutorialsCompleted());      // Build Tutorial Page Based on Tutorials Completed
        }

        // Settings Button Pressed
        else if (but_pressed == settings_button)
        {
            settings_ui.SetActive(true);

            GameObject.FindWithTag("Player").GetComponent<SubtitleControl>().showPlaceholder(); // Show Subtitles PLACEHOLDER
        }

        // Exit Game Button Pressed
        else if (but_pressed == exit_button)
        {
            unpauseGame();

            Application.Quit();                                                     // Quit Application
        }

        // Sub-Menu Close Button
        else if (but_pressed == close_tutorials_menu_button || but_pressed == close_settings_menu_button)
        {
            tutorials_ui.SetActive(false);                                          // Disable Tutorials Menu
            settings_ui.SetActive(false);                                           // Disable Settings Menu

            GameObject.FindWithTag("Player").GetComponent<SubtitleControl>().hidePlaceholder(); // Disable Subtitles PLACEHOLDER
        }
    }
}