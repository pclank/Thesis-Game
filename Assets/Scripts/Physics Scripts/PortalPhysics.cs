using UnityEngine;
using System.Collections;

public class PortalPhysics : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Target Portal for Teleportation.")]
    public GameObject target_portal;    // Target Portal for Teleport

    [Tooltip("Requires Item to Go Through or Not.")]
    public bool item_required = false;  // Requires Item to Go Through or Not

    [Tooltip("Item ID for Required Item.")]
    public int item_id = 0;             // Required Item ID

    [Tooltip("Vector Added to Target Position.")]
    public Vector3 extend = new Vector3(0, 0, 0);

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;   // Player GameObject

    private bool locked;                // Portal is Locked or Not

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Teleport Player to Target Portal
    private void teleport()
    {
        Vector3 target_location = target_portal.transform.position + extend;    // Calculate New Position Coordinates

        player_object.transform.position = target_location;                     // Teleport Player
    }

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        // If Player is Trigger
        if (other.gameObject.CompareTag("Player"))
        {
            teleport();                 // Initiate Teleport
        }
    }

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");   // Get Player GameObject

        // Set Locked or Not

        if (item_required)
        {
            locked = true;
        }
        else
        {
            locked = false;
        }

        // Check that Target GameObject is a Portal

        if (!target_portal.CompareTag("Portal"))
        {
            Debug.Log("GameObject isn't Portal!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}