using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_door_physics : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject icon_object;
    public GameObject handle;

    public float movement_speed = 2.0f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    // Mouse Click Flag
    private bool was_clicked = false;

    // Camera Pointing to Trigger Flag
    private bool cam_trig = false;

    // Interact Flag
    private bool interact = false;

    // Player GameObject
    private GameObject player_object;

    // Rigid Body of this GameObject
    private Rigidbody rigid_body;

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    // Detect Collision with Camera
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            cam_trig = true;

            icon_object.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            cam_trig = false;
            was_clicked = false;
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

            icon_object.SetActive(false);
        }

        // Check for Mouse Down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;
        }

        if (cam_trig && was_clicked)
        {
            interact = true;
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

                rigid_body.AddForceAtPosition(new Vector3(movement, 0, movement), handle.transform.position);
                //rigid_body.AddForceAtPosition(direction.normalized, handle.transform.position);
            }
            else if (Input.GetAxis("Mouse Y") > 0)
            {
                float movement = movement_speed * Input.GetAxis("Mouse Y");
                Vector3 direction = movement * (player_object.transform.position - transform.position);

                rigid_body.AddForceAtPosition(new Vector3(movement, 0, movement), handle.transform.position);
                //rigid_body.AddForceAtPosition(direction.normalized, handle.transform.position);
            }
        }
    }
}
