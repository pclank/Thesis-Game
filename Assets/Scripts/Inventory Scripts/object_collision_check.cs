using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_collision_check : MonoBehaviour
{
    // ******************************************************
    // GameObject Parameters to be Used in Item Creation
    // ******************************************************

    public string item_name;
    public int item_id;

    // Mouse Click Flag
    private bool was_clicked = false;

    // Camera Pointing to Trigger Flag
    private bool cam_trig = false;

    // Player GameObject Variable
    private GameObject player_object;

    // Interaction Icon GameObject Variable
    public GameObject icon_object;

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

            icon_object.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get Player GameObject
        player_object = GameObject.FindWithTag("Player");

        // Get Icon GameObject
        if (icon_object == null)
        {
            icon_object = GameObject.Find("Pick Up - Text");
        }

        icon_object.SetActive(false);

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

        // Check Item Parameters are Set
        if (item_name == null || item_id == null)
        {
            Debug.Log("Item Parameters not Set!");

            // TODO: Add Exception Here!
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Reset in the Next Frame
        if (was_clicked)
        {
            was_clicked = false;
        }

        // Check for Mouse Down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;

            // Check that Collider is the Player and was Pressed
            if (cam_trig && was_clicked)
            {
                // Disable UI Icon
                icon_object.SetActive(false);

                // Build Item in Inventory
                player_object.GetComponent<main_inventory>().buildItem(item_name, item_id);

                // Disable Picked-Up Item's GameObject
                this.gameObject.SetActive(false);
            }
        }
    }
}
