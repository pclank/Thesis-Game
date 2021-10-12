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

    [Tooltip("Array of UI Elements to be Used by Tutorial.")]
    public GameObject[] ui_elements = new GameObject[4];

    [Tooltip("Amount of Seconds to Run Each Tutorial.")]
    public float[] delay = new float[4];

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                                   // Player GameObject

    private float timer_value = 0.0f;                                   // Timer Value

    private bool tutorial_active = false;                               // Whether a Tutorial is Currently Running
    private bool first_tutorial_ran = false;                            // Whether First Tutorial Has Ran

    private uint tutorial_index = 0;                                    // Index of Tutorial Running, Zero-Based

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (!first_tutorial_ran && other.CompareTag("Player"))
        {
            first_tutorial_ran = true;

            startTutorial(0);
        }
    }

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************
    
    // Start Tutorial
    private void startTutorial(uint tut_index)
    {
        tutorial_active = true;

        tutorial_index = tut_index;

        freezePlayer();                                     // Freeze Player Controller

        ui_elements[tutorial_index].SetActive(true);        // Display UI Element

        timer_value = Time.time;                            // Set Timer Value
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
        // Timer Section

        if (tutorial_active && (Time.time - timer_value) >= delay[tutorial_index])
        {
            ui_elements[tutorial_index].SetActive(false);           // Disable UI Element
            unfreezePlayer();                                       // Unfreeze Player

            tutorial_active = false;
        }
    }
}