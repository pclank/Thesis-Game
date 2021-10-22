using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

// ************************************************************************************
// Tutorials Menu
// ************************************************************************************

public class TutorialsMenu : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Array of Buttons that Start Tutorials.")]
    public Button[] tutorial_buttons = new Button[6];                           // Array of Tutorial Buttons

    public GameObject base_tutorial_object;                                     // Base Tutorial Object

    [Tooltip("Array of Tutorial IDs Respective to the Buttons.")]
    public uint[] tutorial_ids = new uint[6];                                   // Array of Tutorial IDs Respective to the Buttons

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************



    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Build Tutorial Page
    public void buildTutorialPage(uint tutorials_completed)
    {
        // Build Buttons for the Acquired Tutorials
        for (int i = 0; i < tutorials_completed; i++)
        {
            tutorial_buttons[i].gameObject.SetActive(true);                         // Enable Button
        }
    }

    // Button Callback Function
    private void buttonStartTutorial(Button but_pressed)
    {
        base_tutorial_object.GetComponent<BaseTutorial>().initiateStartfromMenu((uint)Array.IndexOf(tutorial_buttons, but_pressed));    // Get Index of Button and Run Tutorial Script
    }

    // Use this for initialization
    void Start()
    {
        // Add Listeners to Buttons
        
        foreach (Button but in tutorial_buttons)
        {
            but.onClick.AddListener(() => buttonStartTutorial(but));

            but.gameObject.SetActive(false);                                            // Disable Button on Start
        }
    }
}