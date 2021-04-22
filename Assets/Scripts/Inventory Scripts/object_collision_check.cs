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

    // GameObject Variable
    private GameObject player_object;

    // Detect Collision with Camera
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            cam_trig = true;

            // TODO: Enable UI Icon!
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            cam_trig = false;

            // TODO: Disable UI Icon!
        }
    }

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
                // TODO: Disable UI Icon

                // Build Item in Inventory
                player_object.GetComponent<main_inventory>().buildItem(item_name, item_id);

                // Disable Picked-Up Item's GameObject
                this.gameObject.SetActive(false);
            }
        }
    }
}
