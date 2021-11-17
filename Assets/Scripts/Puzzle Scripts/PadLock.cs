using UnityEngine;
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

    [Tooltip("Camera for Padlock Interaction.")]
    public Camera padlock_camera;

    [Tooltip("Code Solution.")]
    public int[] code_solution;

    [Tooltip("Max Value.")]
    public int max_value = 99;

    [Tooltip("Rotation per Input.")]
    public float base_rotation = 0.1f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                                   // Player GameObject
    private GameObject camera_object;                                   // MainCamera GameObject

    private int[] code_inputs = new int[3];                             // Code Inputs by Player

    private bool ray_trig = false;                                      // Raycast Flag
    private bool last_was_clockwise = false;                            // Whether Last Rotation was Clockwise
    private bool interaction = false;                                   // Whether Player is interaction Padlock

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

        player_object.GetComponent<AuxiliaryUI>().controlUI(1, flag);
    }

    // Get Raycast Trigger Flag
    public bool getRaycast()
    {
        return ray_trig;
    }

    // Interact with Padlock
    private void interact()
    {
        interaction = true;                                                         // Set Interaction Flag On

        player_object.GetComponent<FirstPersonMovement>().stop_flag = true;         // Freeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = true;             // Freeze Camera Controller

        player_object.GetComponent<SwitchCameras>().switchCamera(padlock_camera);   // Switch Cameras

        input_ui.SetActive(true);                                                   // Enable UI
    }

    // Exit Interaction with Keypad
    private void exitInteraction()
    {
        interaction = false;                                                        // Set Interaction Flag Off

        player_object.GetComponent<FirstPersonMovement>().stop_flag = false;        // Unfreeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = false;            // Unfreeze Camera Controller

        player_object.GetComponent<SwitchCameras>().resetDefaultCamera();           // Switch Camera

        input_ui.SetActive(false);                                                  // Disable UI
    }

    // Rotate Clockwise
    private void rotateClockwise()
    {
        gameObject.transform.Rotate(0.0f, base_rotation, 0.0f);

        int prev_value = current_value;                                             // Store Previous Value

        // Process Edge Case
        if (current_value == 0)
        {
            current_value = 99;
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
            else
                current_input++;
        }
        else if (last_was_clockwise && current_input == 2)
        {
            code_inputs[current_input] = current_value;

            checkSolution();                                                    // Check Solution
        }

        last_was_clockwise = true;

        Debug.Log(string.Join(".", code_inputs));
    }

    // Rotate Counter-Clockwise
    private void rotateCounterClockwise()
    {
        gameObject.transform.Rotate(0.0f, -base_rotation, 0.0f);

        int prev_value = current_value;                                             // Store Previous Value

        // Process Edge Case
        if (current_value == 99)
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
            else
                current_input++;
        }
        else if (!last_was_clockwise && current_input == 2)
        {
            code_inputs[current_input] = current_value;

            checkSolution();                                                    // Check Solution
        }

        last_was_clockwise = false;

        Debug.Log(string.Join(".", code_inputs));
    }

    // Check Solution
    private void checkSolution()
    {
        if (code_inputs.SequenceEqual(code_solution))
        {
            Debug.Log("Solution Correct!");

            camera_object.GetComponent<InteractionRaycasting>().disableHit();

            unlockBox();

            exitInteraction();

            //Destroy(this);
        }
    }

    // Unlock Box
    private void unlockBox()
    {
        // TODO: Write Code!
    }

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");       // Get Player GameObject
        camera_object = GameObject.FindWithTag("MainCamera");   // Get Main Camera GameObject
    }

    // Update is called once per frame
    void Update()
    {
        // When Player is interaction Padlock
        if (interaction)
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;         // Freeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = true;             // Freeze Camera Controller

            if (Input.GetKeyDown(clockwise_button))
                rotateClockwise();
            else if (Input.GetKeyDown(counter_clockwise_button))
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