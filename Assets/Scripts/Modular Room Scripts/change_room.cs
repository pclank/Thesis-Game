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
    public GameObject associated_puzzle;                // Puzzle Associated with Changing Rooms

    public float rotation_limit_lower = -90.0f;         // Lower Rotation Limit
    public float rotation_limit_upper = 90.0f;          // Upper Rotation Limit

    public bool inventory_requirement = false;          // Flag that an Item Must be in Inventory for Room Change
    public bool puzzle_requirement = false;             // Flag that a Puzzle Must be Solved for Room Change

    public int req_item_id = 0;                         // The ID of the Item Required

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private MainPuzzle main_puzzle;                     // Code of Associated Puzzle

    private GameObject player_object;                   // Player GameObject
    private bool player_trig;                           // Player In/Out of Box Collider

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Function to Change Rooms

    private void changeRooms()
    {
        initial_room.SetActive(false);
        secondary_room.SetActive(true);
    }

    // Function to Check Conditions
    private void checkConditions()
    {
        float player_rotation = player_object.transform.eulerAngles.y;      // Get Player Rotation

        if (player_rotation >= rotation_limit_lower && player_rotation <= rotation_limit_upper)     // Rotation is Within Limits
        {
            player_trig = false;                                                                        // Reset Flag

            changeRooms();                                                                              // Switch Rooms
        }
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

        if (inventory_requirement)              // If Item is Required
        {
            if (req_item_id == 0)                   // Check that Item ID Has Been Set
            {
                Debug.Log("Required Item ID Not Set!");
            }
        }

        if (puzzle_requirement)                 // If Puzzle is Required
        {
            main_puzzle = associated_puzzle.GetComponent<MainPuzzle>(); // Set Associated Script
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player_trig)                        // Player in Collider
        {
            // No Inventory or Puzzle Requirement

            if (!inventory_requirement && !puzzle_requirement)
            {
                checkConditions();
            }
            
            // Inventory Requirement

            else if (inventory_requirement && player_object.GetComponent<main_inventory>().startQuery(req_item_id))
            {
                checkConditions();
            }

            // Puzzle Requirement

            else if (puzzle_requirement && main_puzzle.checkSolved())
            {
                checkConditions();
            }
        }
    }
}
