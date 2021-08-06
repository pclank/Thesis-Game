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
    public GameObject interact_ui;                              // Interact with Keypad UI GameObject
    public GameObject tgt_door;                                 // Target Door to Open GameObject
    public GameObject fail_indicator;                           // Wrong Password Indicator GameObject
    public GameObject success_indicator;                        // Correct Password Indicator GameObject

    public float delay = 0.5f;                                  // Ray Hit Update Delay

    public bool ray_trig = false;                               // Ray Hit Button
    public bool ray_keypad = false;                             // Ray Hit Keypad

    [Tooltip("Correct Sequence.")]
    public int[] correct_sequence = new int[] {-1, -1, -1, -1}; // Correct Number Sequence

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                           // Player GameObject
    private GameObject camera_object;                           // Main Camera GameObject

    private bool was_clicked = false;                           // Was Clicked Flag
    private bool counter_on = false;                            // Counter On
    private bool interaction = false;                           // Interacting with Keypad

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

    // Interact with Keypad
    private void interact()
    {
        interaction = true;                                                     // Set Interaction Flag On

        player_object.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = true;         // Freeze Camera Controller

        player_object.GetComponent<SwitchCameras>().switchCamera();             // Switch Cameras
    }

    // Exit Interaction with Keypad
    private void exitInteraction()
    {
        interaction = false;                                                    // Set Interaction Flag Off

        player_object.GetComponent<FirstPersonMovement>().stop_flag = false;    // Unfreeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = false;        // Unfreeze Camera Controller

        player_object.GetComponent<SwitchCameras>().resetDefaultCamera();       // Switch Camera
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
        sequence = new int[] { 0, 0, 0, 0 };                                // Set Sequence to All Zeros

        keys_in_sequence = 0;                                               // Reset Keys Pressed Counter

        fail_indicator.GetComponent<LightEmissionFX>().startIndicator();    // Start Indication
    }

    // Confirm Sequence
    private void confirmSequence()
    {
        // Check Sequence
        if (keys_in_sequence == 4 && sequence.Equals(correct_sequence))
        {
            tgt_door.GetComponent<BasicStartAnimation>().startAnimation();      // Open Door

            success_indicator.GetComponent<LightEmissionFX>().startIndicator(); // Start Indication
        }
        else
        {
            resetSequence();                                                    // Reset Sequence

            fail_indicator.GetComponent<LightEmissionFX>().startIndicator();    // Start Indication
        }
    }

    // First Frame Only
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");       // Get Player GameObject
        camera_object = GameObject.FindWithTag("MainCamera");   // Get Main Camera GameObject
    }

    // Update is called once per frame
    void Update()
    {
        // Key Press UI Functionality
        if (interaction && ray_trig)
        {
            counter_on = true;
            counter_value = Time.time;

            press_ui.SetActive(true);
        }
        // Keypad Interaction UI Functionality
        else if (!interaction && ray_keypad)
        {
            counter_on = true;
            counter_value = Time.time;

            interact_ui.SetActive(true);
        }

        // Reset in the Next Frame
        if (was_clicked)
        {
            was_clicked = false;
        }

        // Check for Mouse Down and No Interaction
        if (!interaction && Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;

            // Check that Collider is a Key and was Pressed
            if ((ray_trig || ray_keypad) && was_clicked)
            {
                interact();             // Start Interaction
            }
        }
        // Check for Mouse Down and Interaction
        else if (interaction && Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;

            // Check that Collider is a Key and was Pressed
            if (ray_trig && was_clicked)
            {
                processPress();         // Process Press
            }
        }
        // Check for Interaction Exit on Right Mouse Click
        else if (interaction && Input.GetKeyDown(KeyCode.Mouse1))
        {
            exitInteraction();          // Exit Interaction
        }

        if (counter_on && Time.time - counter_value >= delay)
        {
            press_ui.SetActive(false);
            interact_ui.SetActive(false);

            counter_on = false;
        }
    }
}