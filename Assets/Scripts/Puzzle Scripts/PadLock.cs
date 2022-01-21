using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

// ************************************************************************************
// Padlock Control Script
// ************************************************************************************

public class PadLock : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Header("Button Options")]
    [Tooltip("Clockwise KeyCode.")]
    public KeyCode clockwise_button = KeyCode.D;

    [Tooltip("Counter-Clockwise KeyCode.")]
    public KeyCode counter_clockwise_button = KeyCode.A;

    [Header("Misc Options")]
    [Tooltip("Input UI GameObject.")]
    public GameObject input_ui;

    [Tooltip("Current Value UI GameObject.")]
    public GameObject value_ui;

    [Tooltip("Control Layout UI.")]
    public GameObject control_ui;

    [Tooltip("Tick SFX.")]
    public AK.Wwise.Event tick_sfx_event;

    [Tooltip("Get Animated GameObject.")]
    public GameObject lock_object;

    [Tooltip("Box Top GameObject.")]
    public GameObject box_top;

    [Tooltip("Key GameObject.")]
    public GameObject key_object;

    [Tooltip("Camera for Padlock Interaction.")]
    public Camera padlock_camera;

    [Tooltip("Code Solution.")]
    public int[] code_solution;

    [Tooltip("Max Value.")]
    public int max_value = 99;

    [Tooltip("Rotation per Input.")]
    public float base_rotation = 0.1f;

    [Tooltip("Rotation Delay When Button is Held Down.")]
    public float rotation_delay = 0.2f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                                   // Player GameObject
    private GameObject camera_object;                                   // MainCamera GameObject

    private int[] code_inputs = new int[3];                             // Code Inputs by Player

    private float rotation_timer_value = 0.0f;                          // Rotation Timer Value

    private bool ray_trig = false;                                      // Raycast Flag
    private bool last_was_clockwise = false;                            // Whether Last Rotation was Clockwise
    private bool interaction = false;                                   // Whether Player is interaction Padlock
    private bool clockwise_pressed = false;                             // Whether the Clockwise Button is Held Down
    private bool counter_clockwise_pressed = false;                     // Whether the Counter - Clockwise Button is Held Down
    private bool rotation_timer_on = false;                             // Whether Rotation Timer is On

    private int current_value = 0;                                      // Value the Needle is Pointing at
    private int current_input = 0;                                      // Current Input Array Index

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Function to Externally Set the Raycast Trigger Flag
    public void setRaycast(bool flag)
    {
        if (!player_object.GetComponent<main_inventory>().isInventoryOpen())
            ray_trig = flag;

        if (!interaction)
            player_object.GetComponent<AuxiliaryUI>().controlUI(5, flag);
        else
            player_object.GetComponent<AuxiliaryUI>().controlUI(5, false);
    }

    // Get Raycast Trigger Flag
    public bool getRaycast()
    {
        return ray_trig;
    }

    // Disable Code
    public void disableCode()
    {
        box_top.GetComponent<RotateHingePhysics>().unlock();                        // Unlock Box
        gameObject.tag = "Untagged";                                                // Untag Object

        key_object.SetActive(true);                                                 // Enable Key GameObject

        setRaycast(false);

        Destroy(this);
    }

    // Process Input
    private void processInput()
    {
        // Clockwise Input
        if (Input.GetKeyDown(clockwise_button))
            clockwise_pressed = true;
        else if (Input.GetKeyUp(clockwise_button))
            clockwise_pressed = false;

        // Counter - Clockwise Input
        if (Input.GetKeyDown(counter_clockwise_button))
            counter_clockwise_pressed = true;
        else if (Input.GetKeyUp(counter_clockwise_button))
            counter_clockwise_pressed = false;

        // Process Both Pressed
        if (clockwise_pressed && counter_clockwise_pressed)
        {
            clockwise_pressed = false;
            counter_clockwise_pressed = false;
        }
        // Process Both Not Pressed
        else if (!clockwise_pressed && !counter_clockwise_pressed)
        {
            rotation_timer_on = false;
        }
    }

    // Interact with Padlock
    private void interact()
    {
        interaction = true;                                                         // Set Interaction Flag On

        player_object.GetComponent<FirstPersonMovement>().stop_flag = true;         // Freeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = true;             // Freeze Camera Controller

        player_object.GetComponent<SwitchCameras>().switchCamera(padlock_camera);   // Switch Cameras

        input_ui.SetActive(true);                                                   // Enable UI
        value_ui.SetActive(true);                                                   // Enable UI
        control_ui.SetActive(true);                                                 // Enable UI
    }

    // Exit Interaction with Keypad
    private void exitInteraction()
    {
        interaction = false;                                                        // Set Interaction Flag Off

        player_object.GetComponent<FirstPersonMovement>().stop_flag = false;        // Unfreeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = false;            // Unfreeze Camera Controller

        player_object.GetComponent<SwitchCameras>().resetDefaultCamera();           // Switch Camera

        input_ui.SetActive(false);                                                  // Disable UI
        value_ui.SetActive(false);                                                  // Disable UI
        control_ui.SetActive(false);                                                // Disable UI
    }

    // Rotate Clockwise
    private void rotateClockwise()
    {
        tick_sfx_event.Post(gameObject);                                            // Trigger SFX Event

        gameObject.transform.Rotate(0.0f, base_rotation, 0.0f);

        int prev_value = current_value;                                             // Store Previous Value

        // Process Edge Case
        if (current_value == 0)
        {
            current_value = 39;
        }
        else
            current_value--;

        // Process Code Inputs
        if (!last_was_clockwise)
        {
            code_inputs[current_input] = prev_value;                            // Set Value

            // Edge Case
            if (current_input == 2)
            {
                System.Array.Copy(code_inputs, 1, code_inputs, 0, code_inputs.Length - 1);  // Left Shift Array

                checkSolution();                                                            // Check Solution
            }
            else if (current_input < 2)
                current_input++;
        }
        else if (last_was_clockwise && current_input == 2)
        {
            code_inputs[current_input] = current_value;

            checkSolution();                                                    // Check Solution
        }

        last_was_clockwise = true;

        value_ui.GetComponent<Text>().text = current_value.ToString();      // Update UI Value

        // Timer Section
        if (!rotation_timer_on)
        {
            rotation_timer_on = true;
        }

        rotation_timer_value = Time.time;                                   // Update Timer Value

        Debug.Log(string.Join(".", code_inputs));
    }

    // Rotate Counter-Clockwise
    private void rotateCounterClockwise()
    {
        tick_sfx_event.Post(gameObject);                                            // Trigger SFX Event

        gameObject.transform.Rotate(0.0f, -base_rotation, 0.0f);

        int prev_value = current_value;                                             // Store Previous Value

        // Process Edge Case
        if (current_value == 39)
        {
            current_value = 0;
        }
        else
            current_value++;

        // Process Code Inputs
        if (last_was_clockwise)
        {
            code_inputs[current_input] = prev_value;                            // Set Value

            // Edge Case
            if (current_input == 2)
            {
                System.Array.Copy(code_inputs, 1, code_inputs, 0, code_inputs.Length - 1);  // Left Shift Array

                checkSolution();                                                            // Check Solution
            }
            else if (current_input < 2)
                current_input++;
        }
        else if (!last_was_clockwise && current_input == 2)
        {
            code_inputs[current_input] = current_value;

            checkSolution();                                                    // Check Solution
        }

        last_was_clockwise = false;

        value_ui.GetComponent<Text>().text = current_value.ToString();      // Update UI Value

        // Timer Section
        if (!rotation_timer_on)
        {
            rotation_timer_on = true;
        }

        rotation_timer_value = Time.time;                                   // Update Timer Value

        Debug.Log(string.Join(".", code_inputs));
    }

    // Check Solution
    private void checkSolution()
    {
        if (code_inputs.SequenceEqual(code_solution))
        {
            Debug.Log("Solution Correct!");

            player_object.GetComponent<PuzzleAnalytics>().addAnalytics("PadLock Puzzle");   // Add to Analytics

            camera_object.GetComponent<InteractionRaycasting>().disableHit();

            unlockBox();

            exitInteraction();
        }
    }

    // Unlock Box
    private void unlockBox()
    {
        GetComponent<AnimationQueueing>().startQueue(gameObject);   // Start Animation Queue
    }

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");       // Get Player GameObject
        camera_object = GameObject.FindWithTag("MainCamera");   // Get Main Camera GameObject

        key_object.SetActive(false);                            // Disable Key on Start
    }

    // Update is called once per frame
    void Update()
    {
        // When Player is interacting with the Padlock
        if (interaction)
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;         // Freeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = true;             // Freeze Camera Controller

            processInput();

            if ((clockwise_pressed && rotation_timer_on && Time.time - rotation_timer_value >= rotation_delay) || (clockwise_pressed && !rotation_timer_on))
                rotateClockwise();
            else if ((counter_clockwise_pressed && rotation_timer_on && Time.time - rotation_timer_value >= rotation_delay) || (counter_clockwise_pressed && !rotation_timer_on))
                rotateCounterClockwise();
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                exitInteraction();
            }
        }
        // Catch Start of Interaction
        else if (ray_trig && !interaction && Input.GetKeyUp(KeyCode.Mouse0))
        {
            interact();
        }
    }
}