using UnityEngine;
using System;
using System.Collections;

// ************************************************************************************
// Modular Corridor Control Script
// ************************************************************************************

public class ModularCorridor : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject door_wall_object;                         // Wall with Door GameObject
    public GameObject end_wall_object;                          // End Wall GameObject
    public GameObject door_trigger_object;                      // Door Trigger GameObject
    public GameObject end_trigger_object;                       // End Trigger GameObject

    [Tooltip("Minimun Distance Player Must Traverse into Corridor.")]
    public float player_min_distance = 1.0f;

    [Tooltip("End Wall Distance from Player.")]
    public float end_wall_distance = 1.0f;

    [Tooltip("Trigger Wall Distance from Player")]
    public float trigger_distance = 1.0f;

    [Tooltip("Box Size On X-Axis.")]
    public float box_distance = 2.0f;                           // Distance Denoting Player Box

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                           // Player GameObject

    private bool ray_trig = false;                              // Raycast Hit Flag
    private bool passed_min_distance = false;                   // Whether Player Has Passed Minimun Distance

    private uint state = 0;                                     // Current State of System

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Raycast Trigger Set Function
    public void setRayTrigger(bool flag)
    {
        ray_trig = flag;
    }

    // Spawn Corridor End Wall
    private void spawnEndWall()
    {
        Vector3 wall_position = end_wall_object.transform.position;                 // Set to Initial Position

        wall_position.x = player_object.transform.position.x - end_wall_distance;   // Set Further from Player

        end_wall_object.transform.position = wall_position;                         // Set Position
        end_wall_object.SetActive(true);                                            // Enable GameObject

        door_trigger_object.SetActive(false);                                       // Disable Door Wall Trigger
        end_trigger_object.SetActive(true);                                         // Enable End Wall Trigger

        state++;                                                                    // Increment State
    }

    // Spawn Corridor Door Wall
    private void spawnDoorWall()
    {
        Vector3 wall_position = end_wall_object.transform.position;                 // Set to End Wall Position

        wall_position.x += box_distance;                                            // Set Backwards to Form Box

        door_wall_object.transform.position = wall_position;                        // Set Position
        door_wall_object.SetActive(true);                                           // Enable GameObject

        end_trigger_object.SetActive(false);                                        // Disable End Wall Trigger

        state++;                                                                    // Increment State
    }

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");               // Get Player GameObject

        // Disable Triggers On Startup

        door_trigger_object.SetActive(false);
        end_trigger_object.SetActive(false);
    }

    // Update is called once per Fixed frame
    void FixedUpdate()
    {
        // Check if Player Has Passed Minimun Distance
        if (!passed_min_distance)
        {
            // Check Distance
            if (Math.Abs(player_object.transform.position.x - door_wall_object.transform.position.x) >= player_min_distance)
            {
                passed_min_distance = true;
                door_trigger_object.SetActive(true);
            }
        }
        // Check State to Destroy Control Script
        else if (state == 2)
        {
            Destroy(this);
        }
        // Check if Player is Triggering a Wall
        else if (passed_min_distance && ray_trig)
        {
            // Check if Player is Looking Towards Door Wall
            if (state == 0)
            {
                spawnEndWall();
            }
            // Check if Player is Looking Towards End Wall
            else if (state == 1)
            {
                spawnDoorWall();
            }
        }

        // Movement for Trigger Walls
        if (passed_min_distance)
        {
            Vector3 new_position = door_trigger_object.transform.position;
            new_position.x = player_object.transform.position.x + trigger_distance;
            door_trigger_object.transform.position = new_position;
            new_position.x -= (trigger_distance * 2);
            end_trigger_object.transform.position = new_position;
        }
    }
}