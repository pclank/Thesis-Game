using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_rotator : MonoBehaviour
{
    // Get Collider from Parent
    private Collider inter_col;

    // Interaction Boolean
    private bool inter_flag = false;

    // Mouse Moved Boolean
    private bool was_clicked = false;

    // Mouse Vertical Translation
    private float mouse_translation = 0.0f;

    // Smoothing Factor
    public float speed_factor = 40.0f;

    // Player Inside Interaction Position
    public void OnPlEnter(Collider other)
    {
        // If the Trigger is the Player
        if (other.gameObject.CompareTag("Player"))
        {
            inter_flag = true;
        }
    }

    // Player Exits Interaction Position
    public void OnPlExit(Collider other)
    {
        // If the Trigger is the Player
        if (other.gameObject.CompareTag("Player"))
        {
            inter_flag = false;
        }
    }

    // On Start
    void Start()
    {
        inter_col = GetComponentInParent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // First Mouse0 Down

        if (Input.GetKeyDown(KeyCode.Mouse0) && inter_flag)
        {
            was_clicked = true;
        }

        // Door Moving

        if (was_clicked && inter_flag)
        {
            mouse_translation = Input.GetAxis("Mouse Y");       // Get Mouse Translation

            float rotation_factor = -mouse_translation * Time.deltaTime * speed_factor;
            float new_rotation = rotation_factor + transform.localRotation.eulerAngles.y;

            if (mouse_translation < 0 && new_rotation < 90)         // Door Closing - Mouse Towards Player
            {
                transform.Rotate(0.0f, rotation_factor, 0.0f, Space.Self);
            }
            else if (mouse_translation > 0 && new_rotation > 0)     // Door Opening - Mouse Away Player
            {
                transform.Rotate(0.0f, rotation_factor, 0.0f, Space.Self);
            }
        }

        // Door Idle

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            was_clicked = false;
        }
    }
}
