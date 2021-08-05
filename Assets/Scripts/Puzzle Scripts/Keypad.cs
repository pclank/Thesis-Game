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

    public GameObject press_ui;                                 // Press Button UI GameObject
    public GameObject tgt_door;                                 // Target Door to Open GameObject
    public GameObject fail_indicator;                           // Wrong Password Indicator GameObject
    public GameObject success_indicator;                        // Correct Password Indicator GameObject

    public float delay = 0.5f;                                  // Ray Hit Update Delay

    public bool ray_trig = false;                               // Ray Hits

    [Tooltip("Correct Sequence.")]
    public int[] correct_sequence = new int[] {-1, -1, -1, -1}; // Correct Number Sequence

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool was_clicked = false;                           // Was Clicked Flag
    private bool counter_on = false;                            // Counter On

    private float counter_value = 0.0f;                         // Counter Value

    private int key_pressed;                                    // ID of Pressed Key
    private int keys_in_sequence = 0;                           // Counts Number of Keys Pressed in Current Sequence
    private int[] sequence = new int[] {0, 0, 0, 0};            // Current Sequence

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
        // Reset Button Pressed
        if (key_pressed == 10)
        {
            resetSequence();            // Reset Sequence
        }
        // Confirm Button Pressed
        else if (key_pressed == 11)
        {
            confirmSequence();          // Confirm Sequence
        }
        // Number Button Pressed
        else if (key_pressed >= 0 && key_pressed <= 9)
        {
            addNumber(key_pressed);     // Add Number
        }    
    }

    // Add Number to Sequence
    private void addNumber(int number)
    {
        // Check Whether the Number of Keys Pressed in the Current Sequence is Smaller than 4
        if (keys_in_sequence == 4)
        {
            resetSequence();                        // Reset Sequence
        }
        else
        {
            sequence[keys_in_sequence] = number;    // Set Number to Sequence

            keys_in_sequence++;                     // Increment Keys Pressed
        }
    }

    // Reset Sequence
    private void resetSequence()
    {
        sequence = new int[] { 0, 0, 0, 0 };    // Set Sequence to All Zeros

        keys_in_sequence = 0;                   // Reset Keys Pressed Counter

        // TODO: Add Code for Lighting Reset Indicator
    }

    // Confirm Sequence
    private void confirmSequence()
    {
        // Check Sequence
        if (keys_in_sequence == 4 && sequence.Equals(correct_sequence))
        {
            tgt_door.GetComponent<BasicStartAnimation>().startAnimation();  // Open Door

            // TODO: Add Code for Lighting Success Indicator
        }
        else
        {
            resetSequence();                    // Reset Sequence

            // TODO: Add Code for Lighting Reset Indicator
        }
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