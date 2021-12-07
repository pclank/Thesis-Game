using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// ************************************************************************************
// Mirror Puzzle Control Script
// ************************************************************************************
public class MirrorPuzzle : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Header("Wwise Events")]
    [Tooltip("Wwise Point Audio Event.")]
    public AK.Wwise.Event point_sfx_event;
    [Tooltip("Wwise Touch Audio Event.")]
    public AK.Wwise.Event touch_sfx_event;

    [Header("Various")]
    [Tooltip("GameObject to Enable, if enable_gameobject is True.")]
    public GameObject target_gameobject;

    [Tooltip("Starts Disabled.")]
    public bool starts_disabled = false;

    [Tooltip("Enables a GameObject upon Entering Portal.")]
    public bool enable_gameobject = false;

    [Tooltip("Time Needed to Point Before Portal Appears in Mirror.")]
    public float time_needed = 3.0f;

    [Tooltip("Activation Speed.")]
    public float activation_speed = 0.005f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;       // Player GameObject
    private GameObject camera_object;       // Camera GameObject
    private GameObject fx_ui;               // FX UI Element
    private GameObject target_portal;       // Target Portal for Teleport

    private bool ray_trig = false;
    private bool tape_is_played = false;    // Whether Tape Has Been Played
    private bool active;                    // Portal is Active
    private bool active_ran = false;        // Activation FX is Finished
    private bool engaged = false;           // Teleportation Engaged
    private bool reverse = false;           // Reverse Image Fading
    private bool timer_enabled = false;     // Whether Timer is Enabled

    private float initial_scale;            // Initial Scale of Portal
    private float current_scale = 0.0f;     // Current Scale During FX
    private float timer_value;              // Timer Value

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Set Raycast
    public void setRaycast(bool flag)
    {
        ray_trig = flag;

        // Control Timer
        if (flag && tape_is_played && !active && !timer_enabled)
        {
            timer_value = Time.time;
            
            timer_enabled = true;
        }
        else if (!flag)
        {
            timer_enabled = false;
        }

        // Control UI
        if (active_ran)
        {
            player_object.GetComponent<AuxiliaryUI>().controlUI(3, flag);
        }
    }

    // Set Tape Has Been Played
    public void setTapeIsPlayed()
    {
        if (!tape_is_played)
        {
            tape_is_played = true;

            // Add Information to Journal
            if (GetComponent<AddToJournalOnInteraction>() != null)
                GetComponent<AddToJournalOnInteraction>().addToJournal();
        }
    }

    // Activate Portal
    private void activatePortal()
    {
        point_sfx_event.Post(gameObject);

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

        player_object.transform.position = target_portal.transform.position;    // Teleport Player
    }


    // When GameObject Becomes Active
    void Awake()
    {
        player_object = GameObject.FindWithTag("Player");       // Get Player GameObject
        camera_object = GameObject.FindWithTag("MainCamera");   // Get Camera GameObject

        initial_scale = transform.localScale.x;                 // Get Local Scale

        fx_ui = player_object.GetComponent<AuxiliaryUI>().ui_objects[4];    // Get FX UI GameObject
        target_portal = GameObject.FindWithTag("MirrorPortal");             // Get Target Portal GameObject

        // If Set, Initialize GameObject State as Disabled
        if (enable_gameobject)
        {
            target_gameobject.SetActive(false);
        }

        // Check that Target GameObject is a Portal

        if (!target_portal.CompareTag("MirrorPortal"))
        {
            Debug.Log("GameObject isn't Portal!");
        }

        if (starts_disabled)
        {
            transform.localScale = new Vector3(0, 0, 0);        // Hide Portal

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
        // Timer Section to Activate Portal in Mirror
        if (timer_enabled && (Time.time - timer_value >= time_needed))
        {
            activatePortal();

            timer_enabled = false;
        }

        // Mouse Click - Touch Detection
        if (active && ray_trig && Input.GetKeyUp(KeyCode.Mouse0))
        {
            engaged = true;

            fx_ui.SetActive(true);
        }

        // Portal Enabling FX
        if (starts_disabled && active && !active_ran)
        {
            current_scale += activation_speed;

            if (current_scale >= initial_scale)
            {
                current_scale = initial_scale;
                
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