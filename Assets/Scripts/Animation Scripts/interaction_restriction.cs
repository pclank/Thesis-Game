using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class interaction_restriction : MonoBehaviour
{
    // Expected Collider Tag
    private string col_tag = "Door";    // TODO: Possibly Add a List of Strings to Check From

    private bool is_free = true;        // Player is free to Interact with Object

    private bool was_clicked = false;
    private bool entered = false;

    // Manually Set Entered Flag
    public void setEntered(bool flag)
    {
        entered = flag;
    }

    // On Trigger Enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(col_tag))
        {
            entered = true;
        }
    }

    // On Trigger Exit
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(col_tag))
        {
            entered = false;
        }
    }

    // Change Freedom of Interaction

    public void setFreedom(bool flag)
    {
        is_free = flag;
    }

    // Get Freedom

    public bool getFreedom()
    {
        return is_free;
    }

    // Update is called once per frame
    void Update()
    {
        // First Mouse0 Down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;
        }

        // Mouse0 Up
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            was_clicked = false;

            gameObject.GetComponent<FirstPersonMovement>().stop_flag = false;   // Unfreeze Player Controller
            gameObject.GetComponentInChildren<FirstPersonLook>().stop_flag = false; // Unfreeze Camera
        }

        // Check if Collider Matches Expected Tag
        if (entered && was_clicked)
        {
            gameObject.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
            gameObject.GetComponentInChildren<FirstPersonLook>().stop_flag = true;  // Freeze Camera
        }
    }
}
