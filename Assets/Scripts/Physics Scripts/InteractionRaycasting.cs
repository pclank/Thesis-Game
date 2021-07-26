using UnityEngine;
using System.Collections;

// ************************************************************************************
// Raycasting for Proper Object Interaction
// ************************************************************************************

public class InteractionRaycasting : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************


    public float max_distance = 10.0f;          // Distance for Raycasting
    public int excluded_layer = 8;              // Layer to Exclude from Raycasting

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private int layer_mask;                     // Layer Mask for Raycasting

    // Use this for initialization
    void Start()
    {
        layer_mask = 1 << excluded_layer;           // Bit Shift to Get Layer Bitmask
        layer_mask = ~layer_mask;                   // Invert Bitmask
    }

    // Update Native Player Loop
    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, max_distance, layer_mask, QueryTriggerInteraction.Collide))  // Generate Ray and Trigger Colliders
        {
            if (hit.transform.gameObject.CompareTag("Interactable"))
            {
                hit.transform.gameObject.GetComponent<ObjectRaycastCheck>().ray_trig = true;
            }

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
    }
}