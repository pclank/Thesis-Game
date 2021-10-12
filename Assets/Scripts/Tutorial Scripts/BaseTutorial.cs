﻿using UnityEngine;
using System.Collections;

// ************************************************************************************
// Basic Tutorial Script
// ************************************************************************************

public class BaseTutorial : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject main_camera;

    public GameObject ui_background;

    [Tooltip("Array of UI Elements to be Used by Tutorial.")]
    public GameObject[] ui_elements = new GameObject[4];

    [Tooltip("Whether Tutorials are Displayed for the Delay Value, rather than exiting on a button press.")]
    public bool use_timer = false;

    [Tooltip("Amount of Seconds to Run Each Tutorial.")]
    public float[] delay = {5.0f, 5.0f, 5.0f, 5.0f};

    [Tooltip("Keybind to Exit Tutorial.")]
    public KeyCode exit_key = KeyCode.Return;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                                   // Player GameObject

    private float timer_value = 0.0f;                                   // Timer Value

    private bool tutorial_active = false;                               // Whether a Tutorial is Currently Running
    private bool first_tutorial_run = false;                            // Whether First Tutorial Has Run

    private uint tutorial_index = 0;                                    // Index of Tutorial Running, Zero-Based

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (!first_tutorial_run && other.CompareTag("Player"))
        {
            first_tutorial_run = true;

            startTutorial(0);
        }
    }

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Start Tutorial from External Script
    public void initiateStart(uint tut_index)
    {
        startTutorial(tut_index);
    }
    
    // Start Tutorial
    private void startTutorial(uint tut_index)
    {
        tutorial_active = true;

        tutorial_index = tut_index;

        freezePlayer();                                     // Freeze Player Controller

        ui_background.SetActive(true);                      // Enable Background
        ui_elements[tutorial_index].SetActive(true);        // Display UI Element

        timer_value = Time.time;                            // Set Timer Value
    }

    // Exit Tutorial
    private void exitTutorial()
    {
        ui_background.SetActive(false);                     // Disable Background
        ui_elements[tutorial_index].SetActive(false);       // Disable UI Element

        unfreezePlayer();                                   // Unfreeze Player

        tutorial_active = false;
    }

    // Freeze Player
    private void freezePlayer()
    {
        player_object.GetComponent<FirstPersonMovement>().stop_flag = true;                     // Freeze Player Controller
        main_camera.GetComponent<FirstPersonLook>().stop_flag = true;                           // Freeze Camera Controller
    }

    // Unfreeze Player
    private void unfreezePlayer()
    {
        player_object.GetComponent<FirstPersonMovement>().stop_flag = false;                    // UnFreeze Player Controller
        main_camera.GetComponent<FirstPersonLook>().stop_flag = false;                          // UnFreeze Camera Controller
    }

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Input Check Section

        if (Input.GetKeyUp(exit_key))
        {
            exitTutorial();
        }

        // Timer Section

        if (use_timer && tutorial_active && (Time.time - timer_value) >= delay[tutorial_index])
        {
            exitTutorial();
        }
    }
}