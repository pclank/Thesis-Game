using UnityEngine;
using System.Collections;

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

    [Tooltip("Camera for Padlock Interaction.")]
    public Camera padlock_camera;

    [Tooltip("Code Solution.")]
    public int[] code_solution;

    [Tooltip("Rotation per Input.")]
    public float base_rotation = 0.1f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                                   // Player GameObject
    private GameObject camera_object;                                   // MainCamera GameObject

    private int[] code_inputs = new int[3];                             // Code Inputs by Player

    private bool last_was_clockwise = false;                            // Whether Last Rotation was Clockwise
    private bool interaction = false;                                   // Whether Player is interaction Padlock

    private int current_value = 0;                                      // Value the Needle is Pointing at
    private int current_input = 0;                                      // Current Input Array Index

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Interact with Padlock
    private void interact()
    {
        interaction = true;                                                         // Set Interaction Flag On

        player_object.GetComponent<FirstPersonMovement>().stop_flag = true;         // Freeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = true;             // Freeze Camera Controller

        player_object.GetComponent<SwitchCameras>().switchCamera(padlock_camera);   // Switch Cameras

        padlock_camera.GetComponent<InteractionRaycasting>().cast_flag = true;      // Allow Raycasting from Keypad Camera

        input_ui.SetActive(true);                                                   // Enable UI
    }

    // Exit Interaction with Keypad
    private void exitInteraction()
    {
        interaction = false;                                                        // Set Interaction Flag Off

        player_object.GetComponent<FirstPersonMovement>().stop_flag = false;        // Unfreeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = false;            // Unfreeze Camera Controller

        player_object.GetComponent<SwitchCameras>().resetDefaultCamera();           // Switch Camera

        padlock_camera.GetComponent<InteractionRaycasting>().cast_flag = false;     // Disallow Raycasting from Keypad Camera

        input_ui.SetActive(false);                                                  // Disable UI
    }

    // Rotate Clockwise
    private void rotateClockwise()
    {
        gameObject.transform.Rotate(base_rotation, 0.0f, 0.0f);

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
            // Edge Case
            if (current_input == 2)
                System.Array.Copy(code_inputs, 1, code_inputs, 0, code_inputs.Length - 1);  // Left Shift Array
            else
                current_input++;
        }

        code_inputs[current_input] = current_value;                         // Set Value

        last_was_clockwise = true;
    }

    // Rotate Counter-Clockwise
    private void rotateCounterClockwise()
    {
        gameObject.transform.Rotate(-base_rotation, 0.0f, 0.0f);

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
            // Edge Case
            if (current_input == 2)
                System.Array.Copy(code_inputs, 1, code_inputs, 0, code_inputs.Length - 1);  // Left Shift Array
            else
                current_input++;
        }

        code_inputs[current_input] = current_value;                         // Set Value

        last_was_clockwise = false;
    }

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");       // Get Player GameObject
        camera_object = GameObject.FindWithTag("MainCamera");   // Get Main Camera GameObject
    }

    // Update is called once per fixed frame
    void FixedUpdate()
    {
        // When Player is interaction Padlock
        if (interaction)
        {
            if (Input.GetKeyUp(clockwise_button))
                rotateClockwise();
            else if (Input.GetKeyUp(counter_clockwise_button))
                rotateCounterClockwise();
        }
    }
}