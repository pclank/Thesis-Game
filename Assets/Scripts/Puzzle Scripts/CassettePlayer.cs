using UnityEngine;
using System.Collections;

// ************************************************************************************
// Cassette Player Class
// ************************************************************************************

public class CassettePlayer : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    // TODO: Add Wwise Object for Tape Audio Clip

    [Tooltip("Play Tape UI GameObject.")]
    public GameObject play_ui;

    [Tooltip("\"No Tape Placed\" UI GameObject.")]
    public GameObject no_tape_ui;

    [Tooltip("Timer Delay for UI Messages.")]
    public float delay = 2.0f;

    [Tooltip("Item ID of Expected Tape.")]
    public int item_id = -1;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool tape_in = false;                                                   // Whether a Tape is Placed in Player
    private bool ray_trig = false;                                                  // Raycast Trigger Flag
    private bool timer_on = false;                                                  // Timer On Flag

    private float timer_value = 0.0f;                                               // Timer Value

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Call Externally, and Place Tape in Player
    public bool placeInPlayer()
    {
        if (!tape_in)
        {
            tape_in = true;

            return true;
        }
        else
        {
            return false;
        }
    }

    // Function to Externally Set the Raycast Trigger Flag
    public void setRaycast(bool flag)
    {
        ray_trig = flag;
    }

    // Play Tape Audio Clip
    private void playTape()
    {
        // Check Whether a Tape is Placed
        if (tape_in)
        {
            // TODO: Play Wwise Audio
        }
        else
        {
            no_tape_ui.SetActive(true);

            timer_value = Time.time;
            timer_on = true;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Timer Section
        if (timer_on && (Time.time - timer_value >= delay))
        {
            no_tape_ui.SetActive(false);

            timer_on = false;
        }
    }
}