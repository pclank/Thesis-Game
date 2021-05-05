using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawer_move : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject icon_object;

    public float limit = 2.0f;
    public float movement_speed = 2.0f;

    // Limit Choices

    public bool x_limit = false;
    public bool y_limit = false;
    public bool z_limit = false;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    // Mouse Click Flag
    private bool was_clicked = false;

    // Camera Pointing to Trigger Flag
    private bool cam_trig = false;

    // Interact Flag
    private bool interact = false;

    // Player GameObject Variable
    private GameObject player_object;

    // Rigid Body of this GameObject
    private Rigidbody rigid_body;

    // Initial Global Position
    private Vector3 initial_position;

    // Global Position Limit
    private Vector3 position_limit;

    // Initial Drag
    private float initial_drag;

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    // Detect Collision with Camera
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera") && other.isTrigger)
        {
            cam_trig = true;

            icon_object.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera") && other.isTrigger)
        {
            cam_trig = false;
            was_clicked = false;

            icon_object.SetActive(false);
        }
    }

    // ************************************************************************************
    // Runtime Functions
    // ************************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        // Get Rigid Body
        rigid_body = GetComponent<Rigidbody>();

        // Get Initial Drag
        initial_drag = rigid_body.drag;

        // Get Initial Global Position
        initial_position = transform.position;

        // Get Position Limit
        position_limit = initial_position - new Vector3(limit, limit, limit);

        // Get Player GameObject
        player_object = GameObject.FindWithTag("Player");

        // Check if Player wasn't Found
        if (player_object == null)
        {
            Debug.Log("No Player Object Found!");

            // TODO: Add Exception Here!
        }

        // Check if Icon wasn't Found
        if (icon_object == null)
        {
            Debug.Log("No Icon Object Found!");

            // TODO: Add Exception Here!
        }

        // Check if Rigid Body Component Doesn't Exist
        if (rigid_body == null)
        {
            Debug.Log("No Rigid Body!");

            // TODO: Add Exception Here!
        }

        icon_object.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Reset in the Next Frame
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            was_clicked = false;

            interact = false;
        }

        // Check for Mouse Down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;
        }

        if (cam_trig && was_clicked && player_object.GetComponent<interaction_restriction>().getFreedom())
        {
            interact = true;

            player_object.GetComponent<interaction_restriction>().setFreedom(true);
        }
    }

    void FixedUpdate()
    {
        

        // Check that Collider is the Player and was Pressed
        if (interact)
        {
            if (x_limit)
            {
                if (Input.GetAxis("Mouse Y") < 0 && transform.position.x > position_limit.x)
                {
                    float movement = movement_speed * Input.GetAxis("Mouse Y");
                    rigid_body.AddForce(new Vector3(movement, 0, 0), ForceMode.Impulse);
                }

                else if (Input.GetAxis("Mouse Y") > 0 && transform.position.x < initial_position.x)
                {
                    float movement = movement_speed * Input.GetAxis("Mouse Y");
                    rigid_body.AddForce(new Vector3(movement, 0, 0), ForceMode.Impulse);
                }
            }

            else if (y_limit)
            {
                if (Input.GetAxis("Mouse Y") < 0 && transform.position.y > position_limit.y)
                {
                    float movement = movement_speed * Input.GetAxis("Mouse Y");
                    rigid_body.AddForce(new Vector3(0, movement, 0), ForceMode.Impulse);
                }

                else if (Input.GetAxis("Mouse Y") > 0 && transform.position.y < initial_position.y)
                {
                    float movement = movement_speed * Input.GetAxis("Mouse Y");
                    rigid_body.AddForce(new Vector3(0, movement, 0), ForceMode.Impulse);
                }
            }

            else if (z_limit)
            {
                if (Input.GetAxis("Mouse Y") < 0 && transform.position.z > position_limit.z)
                {
                    float movement = movement_speed * Input.GetAxis("Mouse Y");
                    rigid_body.AddForce(new Vector3(0, 0, movement), ForceMode.Impulse);
                }

                else if (Input.GetAxis("Mouse Y") > 0 && transform.position.z < initial_position.z)
                {
                    float movement = movement_speed * Input.GetAxis("Mouse Y");
                    rigid_body.AddForce(new Vector3(0, 0, movement), ForceMode.Impulse);
                }
            }
        }
    }
}
