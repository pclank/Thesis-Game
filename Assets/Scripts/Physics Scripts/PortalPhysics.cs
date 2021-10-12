using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PortalPhysics : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Target Portal for Teleportation.")]
    public GameObject target_portal;    // Target Portal for Teleport

    [Tooltip("GameObject to Enable, if enable_gameobject is True.")]
    public GameObject target_gameobject;

    [Tooltip("FX UI Element.")]
    public GameObject fx_ui;            // FX UI Element

    [Tooltip("Starts Disabled.")]
    public bool starts_disabled = false;

    [Tooltip("Enables a GameObject upon Entering Portal.")]
    public bool enable_gameobject = false;

    [Tooltip("Requires Item to Go Through or Not.")]
    public bool item_required = false;  // Requires Item to Go Through or Not

    [Tooltip("Item ID for Required Item.")]
    public int item_id = 0;             // Required Item ID

    [Tooltip("Vector Added to Target Position.")]
    public Vector3 extend = new Vector3(0, 0, 0);

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;   // Player GameObject
    private GameObject camera_object;   // Camera GameObject

    private bool locked;                // Portal is Locked or Not
    private bool active;                // Portal is Active
    private bool active_ran = false;    // Activation FX is Finished
    private bool engaged = false;       // Teleportation Engaged
    private bool reverse = false;       // Reverse Image Fading

    private float initial_scale;        // Initial Scale of Portal
    private float current_scale = 0.0f; // Current Scale During FX

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Activate Portal
    public void activatePortal()
    {
        GetComponent<AudioSource>().Play();

        active = true;
    }

    // Teleport Player to Target Portal
    private void teleport()
    {
        // If Set, Enable GameObject on Teleport
        if (enable_gameobject)
        {
            target_gameobject.SetActive(true);
        }

        Vector3 target_location = target_portal.transform.position + extend;    // Calculate New Position Coordinates

        player_object.transform.position = target_location;                     // Teleport Player
    }

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        // If Player is Trigger
        if (other.gameObject.CompareTag("Player"))
        {
            engaged = true;

            fx_ui.SetActive(true);
        }
    }

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");       // Get Player GameObject
        camera_object = GameObject.FindWithTag("MainCamera");   // Get Camera GameObject

        initial_scale = transform.localScale.x;                 // Get Local Scale

        // If Set, Initialize GameObject State as Disabled
        if (enable_gameobject)
        {
            target_gameobject.SetActive(false);
        }

        // Set Locked or Not

        if (item_required)
        {
            locked = true;
        }
        else
        {
            locked = false;
        }

        // Check that Target GameObject is a Portal

        if (!target_portal.CompareTag("Portal"))
        {
            Debug.Log("GameObject isn't Portal!");
        }

        if (starts_disabled)
        {
            transform.localScale = new Vector3(0, 0, 0);    // Hide Portal

            active = false;
        }
        else
        {
            activatePortal();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Portal Enabling FX
        if (starts_disabled && active && !active_ran)
        {
            current_scale += 0.005f;

            if (current_scale >= initial_scale)
            {
                current_scale = initial_scale;

                //GetComponent<AudioSource>().Stop();

                active_ran = true;
            }

            transform.localScale = new Vector3(current_scale, current_scale, current_scale);
        }

        // Process FX
        if (engaged && active)
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = true;         // Freeze Camera Controller

            if (fx_ui.GetComponent<Image>().color.a < 1.0f)
            {
                float temp_alpha = fx_ui.GetComponent<Image>().color.a + 0.1f;

                if (temp_alpha > 1.0f)
                {
                    temp_alpha = 1.0f;

                    reverse = true;         // Set for Reversal
                    engaged = false;        // Reset Flag
                }

                fx_ui.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, temp_alpha);

                if (!engaged)
                {
                    teleport();             // Initiate Teleport
                }
            }
        }
        else if (active && reverse)
        {
            if (fx_ui.GetComponent<Image>().color.a > 0.0f)
            {
                float temp_alpha = fx_ui.GetComponent<Image>().color.a - 0.1f;

                if (temp_alpha < 0.0f)
                {
                    temp_alpha = 0.0f;

                    reverse = false;        // Set for Reversal
                    engaged = false;        // Reset Flag
                }

                fx_ui.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, temp_alpha);

                if (!reverse)
                {
                    fx_ui.SetActive(false); // Disable UI Element

                    player_object.GetComponent<FirstPersonMovement>().stop_flag = false;     // Unfreeze Player Controller
                    camera_object.GetComponent<FirstPersonLook>().stop_flag = false;         // Unfreeze Camera Controller
                }
            }
        }
    }
}