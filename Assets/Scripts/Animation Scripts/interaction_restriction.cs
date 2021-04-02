using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class interaction_restriction : MonoBehaviour
{
    // Expected Collider Tag
    public string col_tag = "Stall";    // TODO: Possibly Add a List of Strings to Check From

    private bool was_clicked = false;

    // On Trigger Enter
    private void OnTriggerStay(Collider other)
    {
        // First Mouse0 Down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;
        }

        Debug.Log("Trigger Detected");

        // Check if Collider Matches Expected Tag
        if (String.Equals(other.tag, col_tag) && was_clicked)
        {
            gameObject.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
            gameObject.GetComponentInChildren<FirstPersonLook>().stop_flag = true;  // Freeze Camera
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse0 Up
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            was_clicked = false;

            gameObject.GetComponent<FirstPersonMovement>().stop_flag = false;   // Unfreeze Player Controller
            gameObject.GetComponentInChildren<FirstPersonLook>().stop_flag = false; // Unfreeze Camera
        }
    }
}
