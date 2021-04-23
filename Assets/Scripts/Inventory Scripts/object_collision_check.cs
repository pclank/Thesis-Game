using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_collision_check : MonoBehaviour
{
    // ************************************************************************************
    // GameObject Parameters to be Used in Item Creation
    // ************************************************************************************

    public string item_name = null;
    public int item_id = -1;

    // Mouse Click Flag
    private bool was_clicked = false;

    // Camera Pointing to Trigger Flag
    private bool cam_trig = false;

    // Player GameObject Variable
    private GameObject player_object;

    // Interaction Icon GameObject Variable
    public GameObject icon_object;

    // MeshFilter Component Object
    private MeshFilter mesh_f;

    // MeshRenderer Component Object
    private MeshRenderer mesh_r;

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
        if (item_name == null || item_id == -1)         // -1 is Invalid ID
        {
            Debug.Log("Item Parameters not Set!");

            // TODO: Add Exception Here!
        }

        // Get Item GameObject MeshFilter and Mesh Renderer Components

        mesh_f = this.GetComponent(typeof(MeshFilter)) as MeshFilter;
        mesh_r = this.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
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
                player_object.GetComponent<main_inventory>().buildItem(item_name, item_id, mesh_f, mesh_r);

                // Disable Picked-Up Item's GameObject
                this.gameObject.SetActive(false);
            }
        }
    }
}
