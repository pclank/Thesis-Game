using UnityEngine;
using System.Collections;

// ************************************************************************************
// Keypad Interaction Code
// ************************************************************************************

public class Keypad : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject press_ui;                         // Press Button UI GameObject
    public GameObject tgt_door;                         // Target Door to Open GameObject
    public GameObject fail_indicator;                   // Wrong Password Indicator GameObject
    public GameObject success_indicator;                // Correct Password Indicator GameObject

    public float delay = 0.5f;                          // Ray Hit Update Delay

    public bool ray_trig = false;                       // Ray Hits

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool was_clicked = false;                   // Was Clicked Flag
    private bool counter_on = false;                    // Counter On

    private float counter_value = 0.0f;                 // Counter Value

    private int key_pressed;                            // ID of Pressed Key

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Set ID of Pressed Key
    public void setKeyPressed(int key_id)
    {
        key_pressed = key_id;
    }

    // Process Button Press
    private void processPress()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ray_trig)
        {
            counter_on = true;
            counter_value = Time.time;

            press_ui.SetActive(true);
        }

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
            if (ray_trig && was_clicked)
            {
                processPress();
            }
        }

        if (counter_on && Time.time - counter_value >= delay)
        {
            press_ui.SetActive(false);

            counter_on = false;
        }
    }
}