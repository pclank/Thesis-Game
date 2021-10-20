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

            menu_enabled = true;                                                    // Set Menu as Enabled
        }

        // Check for Pause Menu Disable

        else if (menu_enabled && Input.GetKeyUp(KeyCode.Escape))
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = false;    // Unfreeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = false;        // Unfreeze Camera Controller

            menu_enabled = false;                                                   // Set Menu as Disabled
        }
    }

    // Button CallBack Function

    private void buttonCallBack(Button but_pressed)
    {
        // Resume Button Pressed
        if (but_pressed == resume_button)
        {
            
        }

        // Show Tutorials Button Pressed
        else if (but_pressed == show_tutorials_button)
        {
            
        }

        // Settings Button Pressed
        else if (but_pressed == settings_button)
        {

        }

        // Exit Game Button Pressed
        else if (but_pressed == exit_button)
        {
            Application.Quit();                                                     // Quit Application
        }
    }
}