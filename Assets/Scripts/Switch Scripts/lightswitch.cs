using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightswitch : MonoBehaviour
{
    // Mouse Click Flag
    private bool was_clicked = false;

    // Camera Pointing to Switch Flag
    private bool cam_trig = false;

    // Switch Position
    public int pos = 0;

    // Declare Animator
    private Animator switch_anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            cam_trig = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            cam_trig = false;
        }
    }

    // Only First Frame

    void Start()
    {
        switch_anim = gameObject.GetComponentInChildren<Animator>();
    }

    // Once per Frame

    void Update()
    {
        // Reset in the Next Frame
        if (was_clicked)
        {
            was_clicked = false;
        }

        // Check for Mouse Down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;

            // Declare Component List
            Component[] light_list;

            // If the Trigger was the Player and the Mouse0 was Pressed
            if (cam_trig && was_clicked)
            {
                // If Switch is in Position 0
                if (pos == 0)
                {
                    switch_anim.Play("Base Layer.SwitchDown", 0, 0);    // Play Animation
                    gameObject.GetComponent<AudioSource>().Play();      // Play Sound FX

                    pos = 1;                                            // Update Switch Position
                }
                else
                {
                    switch_anim.Play("Base Layer.SwitchUp", 0, 0);      // Play Animation
                    gameObject.GetComponent<AudioSource>().Play();      // Play Sound FX

                    pos = 0;                                            // Update Switch Position
                }

                light_list = gameObject.GetComponentsInChildren<light_control>();   // Get All Lights

                // Check if List is Empty   // TODO: Possibly Add Exception Here
                if (light_list != null)
                {
                    foreach (light_control light in light_list)
                    {
                        light.switchLight();                                            // Run Function in Component
                    }
                }
            }
        }
    }
}
