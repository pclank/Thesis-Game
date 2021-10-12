using UnityEngine;
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

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                                   // Player GameObject

    private bool first_tutorial_ran = false;                            // Whether First Tutorial Has Ran

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (!first_tutorial_ran && other.CompareTag("Player"))
        {
            first_tutorial_ran = true;

            startTutorial();
        }
    }

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************
    
    // Start Base Tutorial
    private void startTutorial()
    {
        freezePlayer();                         // Freeze Player Controller


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

    }
}