using UnityEngine;
using System.Collections;

public class RotateHingePhysics : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public AK.Wwise.Event locked_event;                     // Wwise Event for Locked Door
    public AK.Wwise.Event unlock_event;                     // Wwise Event for Unlocking Door
    public AK.Wwise.Event handle_event;                     // Wwise Event for Using the Handle

    public GameObject icon_object;
    public GameObject handle;
    public GameObject ui_missing;                           // UI Item Missing Text GameObject
    public GameObject ui_unlocked;                          // UI Unlocked Text GameObject

    public float movement_speed = 2.0f;
    public float delay = 0.5f;

    [Tooltip("Indicates if Item (eg. a Key) is Required to Apply Force.")]
    public bool requires_item = false;
    [Tooltip("Requires Message from External Script to Apply Force.")]
    public bool requires_message = false;
    public bool use_force_direction = true;
    public bool ray_trig = false;

    [Tooltip("Indicates ID of Item Required.\nDoes Nothing if requires_item is False.")]
    public int required_item_id = 0;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    // Mouse Click Flag
    private bool was_clicked = false;

    // Interact Flag
    private bool interact = false;

    // Object is Locked/Unlocked
    private bool locked;

    // Timer Has Ran
    private bool t_ran = false;
    private bool counter_on = false;
    private bool missing_ui_on = false;
    private bool sfx_played = false;

    private float counter_value = 0.0f;

    // Time to Show Unlocked Text
    private float t_remaining = 2.0f;

    // Player GameObject
    private GameObject player_object;

    // Rigid Body of this GameObject
    private Rigidbody rigid_body;

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Unlock from External Script
    public void unlock()
    {
        locked = false;
    }

    // ************************************************************************************
    // Runtime Functions
    // ************************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        // Set Locked/Unlocked Variable
        if (requires_item || requires_message)
        {
            locked = true;
        }
        else
        {
            locked = false;
        }

        // Get Rigid Body
        rigid_body = GetComponent<Rigidbody>();

        // Get Player Object
        player_object = GameObject.FindWithTag("Player");

        // Check if Icon wasn't Found
        if (icon_object == null)
        {
            Debug.Log("No Icon Object Found!");

            // TODO: Add Exception Here!
        }

        // Check if Player Object wasn't Found
        if (player_object == null)
        {
            Debug.Log("No Player Object Found!");

            // TODO: Add Exception Here!
        }

        // Check if Rigid Body Component Doesn't Exist
        if (rigid_body == null)
        {
            Debug.Log("No Rigid Body!");

            // TODO: Add Exception Here!
        }

        // Check if Force Direction Object is Present
        if (use_force_direction && handle == null)
        {
            Debug.Log("No Direction Object Found!");

            // TODO: Add Exception Here!
        }

        icon_object.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ray_trig)
        {
            counter_on = true;
            counter_value = Time.time;

            if (!missing_ui_on)
            {
                icon_object.SetActive(true);
            }
        }

        // Reset in the Next Frame
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            was_clicked = false;

            interact = false;

            sfx_played = false;

            icon_object.SetActive(false);
        }

        // Check for Mouse Down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;
        }

        if (ray_trig && was_clicked && player_object.GetComponent<interaction_restriction>().getFreedom())
        {
            bool has_item = player_object.GetComponent<main_inventory>().startQuery(required_item_id);

            if ((requires_item && has_item) || (!requires_item && !locked))
            {
                interact = true;

                if (!sfx_played)
                {
                    handle_event.Post(gameObject);

                    sfx_played = true;
                }

                player_object.GetComponent<interaction_restriction>().setEntered(true);

                if (locked)
                {
                    locked = false;                                 // Unlock Object

                    unlock_event.Post(gameObject);                  // Play Unlock Sound

                    ui_unlocked.SetActive(true);                    // Show Unlocked Text
                }

                player_object.GetComponent<interaction_restriction>().setFreedom(true);
            }
            else if (requires_item && !has_item && !missing_ui_on)
            {
                locked_event.Post(gameObject);                  // Play Locked Door Sound

                ui_missing.SetActive(true);                     // Display UI Text that Item is Missing

                missing_ui_on = true;
            }
        }

        // Timer Section
        if (!locked && !t_ran)
        {
            // Time Remaining
            if (t_remaining > 0)
            {
                t_remaining -= Time.deltaTime;
            }
            // Time Expired
            else
            {
                t_ran = true;                                   // Lock Timer
                ui_unlocked.SetActive(false);                   // Disable UI Text

                player_object.GetComponent<interaction_restriction>().setEntered(false);
            }
        }

        if (counter_on && Time.time - counter_value >= delay)
        {
            icon_object.SetActive(false);
            ui_missing.SetActive(false);

            missing_ui_on = false;
            counter_on = false;

            player_object.GetComponent<interaction_restriction>().setEntered(false);
        }
    }

    void FixedUpdate()
    {
        // Check that Collider is the Player and was Pressed
        if (interact)
        {
            if (Input.GetAxis("Mouse Y") < 0)
            {
                float movement = movement_speed * Input.GetAxis("Mouse Y");
                Vector3 direction = movement * (player_object.transform.position - transform.position);

                if (use_force_direction)
                {
                    rigid_body.AddForceAtPosition(new Vector3(movement, 0, movement), handle.transform.position);
                }
                else
                {
                    rigid_body.AddForce(direction);
                }
            }
            else if (Input.GetAxis("Mouse Y") > 0)
            {
                float movement = movement_speed * Input.GetAxis("Mouse Y");
                Vector3 direction = movement * (player_object.transform.position - transform.position);

                if (use_force_direction)
                {
                    rigid_body.AddForceAtPosition(new Vector3(movement, 0, movement), handle.transform.position);
                }
                else
                {
                    rigid_body.AddForce(direction);
                }
            }
        }
    }
}