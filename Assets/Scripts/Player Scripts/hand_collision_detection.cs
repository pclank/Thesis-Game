using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand_collision_detection : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool free_to_pickup = true;                 // Player is Free to Pick Item Up

    private GameObject restrictive_object;              // GameObject Restricting Interaction

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        // If Collision with Non-Interactable Object is Detected

        if (!other.gameObject.CompareTag("Interactable"))
        {
            restrictive_object = other.gameObject;                  // Store GameObject
            free_to_pickup = false;                                 // Prevent Item Pick-Up
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If Collision with the Currently Restricting Object is Detected

        if (!other.gameObject.CompareTag("Interactable") && other.gameObject == restrictive_object)
        {
            restrictive_object = null;                              // Reset Restrictive Object
            free_to_pickup = true;                                  // Allow Item Pick-Up
        }
    }

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Function to Access Freedom Variable
    public bool isUnobstructed()
    {
        return free_to_pickup;
    }
}
