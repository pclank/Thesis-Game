using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change_room : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject initial_room;                     // Initial Room GameObject
    public GameObject secondary_room;                   // Secondary Room GameObject

    public float rotation_limit_lower = -90.0f;         // Lower Rotation Limit
    public float rotation_limit_upper = 90.0f;          // Upper Rotation Limit

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                   // Player GameObject
    private bool player_trig;                           // Player In/Out of Box Collider

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    private void changeRooms()
    {
        initial_room.SetActive(false);
        secondary_room.SetActive(true);
    }

    // ************************************************************************************
    // Trigger Detection
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_trig = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_trig = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");

        if (player_object == null)
        {
            Debug.Log("Player GameObject Not Found!");
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (player_trig)                        // Player in Collider
        {
            float player_rotation = player_object.transform.eulerAngles.y;      // Get Player Rotation

            if (player_rotation >= rotation_limit_lower && player_rotation <= rotation_limit_upper)     // Rotation is Within Limits
            {
                player_trig = false;                                                                        // Reset Flag

                changeRooms();                                                                              // Switch Rooms
            }
        }
    }
}
