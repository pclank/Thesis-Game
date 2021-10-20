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

    public GameObject camera_object;
    public GameObject menu_ui;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;

    private bool menu_enabled = false;                          // Whether Pause Menu is Enabled

    // Use this for initialization
    void Start()
    {
        // Get Player GameObject
        player_object = GameObject.FindWithTag("Player");

        // Disable Pause Menu on Start
        menu_ui.SetActive(false);

        // Add Button Listeners

        resume_button.onClick.AddListener(() => buttonCallBack(resume_button));
        show_tutorials_button.onClick.AddListener(() => buttonCallBack(show_tutorials_button));
        settings_button.onClick.AddListener(() => buttonCallBack(settings_button));
        exit_button.onClick.AddListener(() => buttonCallBack(exit_button));
    }

    // Update is called once per frame
    void Update()
    {
        // Check for Pause Menu Enable

        if (!menu_enabled && Input.GetKeyUp(KeyCode.Escape))
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = true;         // Freeze Camera Controller

            Cursor.lockState = CursorLockMode.None;                                 // Unlock Cursor
            Cursor.visible = true;                                                  // Make Cursor Visible

            menu_ui.SetActive(true);                                                // Enable Pause Menu GameObject

            menu_enabled = true;                                                    // Set Menu as Enabled
        }

        // Check for Pause Menu Disable

        else if (menu_enabled && Input.GetKeyUp(KeyCode.Escape))
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = false;    // Unfreeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = false;        // Unfreeze Camera Controller

            Cursor.lockState = CursorLockMode.Locked;                               // Lock Cursor to Center
            Cursor.visible = false;                                                 // Hide Cursor

            menu_ui.SetActive(false);                                               // Disable Pause Menu GameObject

            menu_enabled = false;                                                   // Set Menu as Disabled
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
        }

        // Show Tutorials Button Pressed
        else if (but_pressed == show_tutorials_button)
        {
            // TODO: Add Functionality!
        }

        // Settings Button Pressed
        else if (but_pressed == settings_button)
        {
            // TODO: Add Functionality!
        }

        // Exit Game Button Pressed
        else if (but_pressed == exit_button)
        {
            Application.Quit();                                                     // Quit Application
        }
    }
}