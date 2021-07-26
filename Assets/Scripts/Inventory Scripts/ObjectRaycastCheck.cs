﻿using UnityEngine;
using System.Collections;

public class ObjectRaycastCheck : MonoBehaviour
{
    // ************************************************************************************
    // GameObject Parameters to be Used in Item Creation
    // ************************************************************************************

    public int item_id = -1;
    public float item_scale = 1.5f;
    public float delay = 0.5f;

    // Interaction Icon GameObject Variable
    public GameObject icon_object;

    // Camera Pointing to Trigger Flag
    public bool ray_trig = false;

    // Mouse Click Flag
    private bool was_clicked = false;

    // Counter Flag
    private bool counter_on = false;

    // Counter Value
    private float counter_value = 0.0f;

    // Player GameObject Variable
    private GameObject player_object;

    // Camera GameObject Variable
    private GameObject camera_object;

    // MeshFilter Component Object
    private MeshFilter mesh_f;

    // MeshRenderer Component Object
    private MeshRenderer mesh_r;

    // ************************************************************************************
    // Runtime Functions
    // ************************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        // Get Player GameObject
        player_object = GameObject.FindWithTag("Player");

        // Get Camera GameObject
        camera_object = GameObject.FindWithTag("MainCamera");

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

        // Check if Camera wasn't Found
        if (camera_object == null)
        {
            Debug.Log("No Camera Object Found!");

            // TODO: Add Exception Here!
        }

        // Check if Icon wasn't Found
        if (icon_object == null)
        {
            Debug.Log("No Icon Object Found!");

            // TODO: Add Exception Here!
        }

        // Check Item Parameters are Set
        if (item_id == -1)                          // -1 is Invalid ID
        {
            Debug.Log("Item Parameters Not Set!");

            // TODO: Add Exception Here!
        }

        // Get Item GameObject MeshFilter and Mesh Renderer Components

        mesh_f = this.GetComponent(typeof(MeshFilter)) as MeshFilter;
        mesh_r = this.GetComponent(typeof(MeshRenderer)) as MeshRenderer;

        // Check Above Components are Set
        if (mesh_f == null || mesh_r == null)
        {
            Debug.Log("Item Components Not Retrieved!");

            // TODO: Add Exception Here!
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ray_trig)
        {
            counter_on = true;
            counter_value = Time.time;

            icon_object.SetActive(true);
        }

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
            if (ray_trig && was_clicked)
            {
                // Disable UI Icon
                icon_object.SetActive(false);

                // Build Item in Inventory
                player_object.GetComponent<main_inventory>().buildItem(item_id, mesh_f, mesh_r, item_scale);

                // Call Knowledge Update Function from KnowledgeOnPickup Script
                this.gameObject.GetComponent<KnowledgeOnPickup>().increaseKnowledge();

                // Disable Picked-Up Item's GameObject
                this.gameObject.SetActive(false);
            }
        }

        if (Time.time - counter_value >= delay)
        {
            icon_object.SetActive(false);
            ray_trig = false;

            counter_on = false;
        }

        ray_trig = false;
    }
}