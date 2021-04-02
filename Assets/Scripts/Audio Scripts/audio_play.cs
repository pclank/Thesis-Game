using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_play : MonoBehaviour
{
    // Mouse Click Flag
    private bool was_clicked = false;

    // Camera Pointing to Switch Flag
    private bool cam_trig = false;

    // Catch Trigger
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

    // Update is called once per frame
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

            // Check that Collider is the Player and was Pressed
            if (cam_trig && was_clicked)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }
}
