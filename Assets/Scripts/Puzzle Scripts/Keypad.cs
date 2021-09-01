using UnityEngine;
using System.Collections;
using System.Linq;

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
    public Camera keypad_camera;                                // Keypad Camera

    public AudioClip success_clip;                              // Success SFX
    public AudioClip fail_clip;                                 // Failure SFX
    public AudioClip key_clip;                                  // Key Press SFX

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
    private GameObject hit_object;                              // Hit GameObject

    private AudioSource audio_source;                           // SFX Source

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

    // Set Hit GameObject

    public void setHitObject(GameObject hit_obj)
    {
        hit_object = hit_obj;
    }

    // Interact with Keypad
    private void interact()
    {
        interaction = true;                                                         // Set Interaction Flag On

        player_object.GetComponent<FirstPersonMovement>().stop_flag = true;         // Freeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = true;             // Freeze Camera Controller

        player_object.GetComponent<SwitchCameras>().switchCamera(keypad_camera);    // Switch Cameras

        keypad_camera.GetComponent<InteractionRaycasting>().cast_flag = true;       // Allow Raycasting from Keypad Camera

        Cursor.lockState = CursorLockMode.None;                                     // Unlock Cursor
        Cursor.visible = true;                                                      // Make Cursor Visible
    }

    // Exit Interaction with Keypad
    private void exitInteraction()
    {
        interaction = false;                                                        // Set Interaction Flag Off
        ray_trig = false;                                                           // Set Raycast Flag Off
        ray_keypad = false;                                                         // Set Raycast Flag Off
        counter_on = false;                                                         // Set Counter Flag Off

        counter_value = 0.0f;                                                       // Reset Counter Value

        player_object.GetComponent<FirstPersonMovement>().stop_flag = false;        // Unfreeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = false;            // Unfreeze Camera Controller

        player_object.GetComponent<SwitchCameras>().resetDefaultCamera();           // Switch Camera

        keypad_camera.GetComponent<InteractionRaycasting>().cast_flag = false;      // Disallow Raycasting from Keypad Camera

        Cursor.lockState = CursorLockMode.Locked;                                   // Lock Cursor to Center
        Cursor.visible = false;                                                     // Hide Cursor
    }

    // Process Button Press
    private void processPress()
    {
        // Play Animation
        if (hit_object != null)
        {
            hit_object.GetComponent<BasicStartAnimation>().startAnimation();
        }

        // Reset Button Pressed
        if (key_pressed == 10)
        {
            resetSequence();                // Reset Sequence
        }
        // Confirm Button Pressed
        else if (key_pressed == 11)
        {
            confirmSequence();              // Confirm Sequence
        }
        // Number Button Pressed
        else if (key_pressed >= 0 && key_pressed <= 9)
        {
            audio_source.clip = key_clip;   // Set to Success Clip
            audio_source.Play();            // Play Audio

            addNumber(key_pressed);         // Add Number
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

        audio_source.clip = fail_clip;                                      // Set to Failure Clip
        audio_source.Play();                                                // Play Audio

        fail_indicator.GetComponent<LightEmissionFX>().startIndicator();    // Start Indication
    }

    // Confirm Sequence
    private void confirmSequence()
    {
        // Check Sequence
        if (keys_in_sequence == 4 && sequence.SequenceEqual(correct_sequence))
        {
            tgt_door.GetComponent<BasicStartAnimation>().startAnimation();      // Open Door

            audio_source.clip = success_clip;                                   // Set to Success Clip
            audio_source.Play();                                                // Play Audio

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

        audio_source = GetComponent<AudioSource>();             // Get Audio Source
    }

    // Update is called once per frame
    void Update()
    {
        // Key Press UI Functionality
        if (interaction && ray_trig)
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;         // Freeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = true;             // Freeze Camera Controller

            counter_on = true;
            counter_value = Time.time;

            press_ui.SetActive(true);
        }
        else if (interaction && !ray_trig)
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;         // Freeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = true;             // Freeze Camera Controller

            counter_on = false;

            press_ui.SetActive(false);
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

                interact_ui.SetActive(false);
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

            counter_value = 0.0f;

            counter_on = false;
        }
    }
}