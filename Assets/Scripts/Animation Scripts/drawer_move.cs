using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawer_move : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject icon_object;

    public float z_limit = 2.0f;
    public float z_limit_upper = 0.0f;
    public float movement_speed = 2.0f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    // Mouse Click Flag
    private bool was_clicked = false;

    // Camera Pointing to Trigger Flag
    private bool cam_trig = false;

    // Player GameObject Variable
    private GameObject player_object;

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

            icon_object.SetActive(false);
        }
    }

    // ************************************************************************************
    // Runtime Functions
    // ************************************************************************************

    // Start is called before the first frame update
    void Start()
    {
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

        icon_object.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Reset in the Next Frame
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            was_clicked = false;
        }

        // Check for Mouse Down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;
        }

        // Check that Collider is the Player and was Pressed
        if (cam_trig && was_clicked)
        {
            if (Input.GetAxis("Mouse Y") <= 0 && transform.localPosition.z < z_limit)
            {
                float movement = -movement_speed * Input.GetAxis("Mouse Y");
                transform.Translate(new Vector3(0, 0, movement) * Time.deltaTime);
            }
            
            else if (Input.GetAxis("Mouse Y") > 0 && transform.localPosition.z > z_limit_upper)
            {
                float movement = -movement_speed * Input.GetAxis("Mouse Y");
                transform.Translate(new Vector3(0, 0, movement) * Time.deltaTime);
            }
        }
    }
}
