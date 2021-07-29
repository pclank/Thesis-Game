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
    private bool hit_flag = false;              // Flag Denoting Hit in Previous Update
    private GameObject hit_gameobject;          // Hit GameObject

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Disable GameObject Hit Variable
    private void disableHit()
    {
        if (hit_gameobject.CompareTag("Interactable"))
        {
            hit_gameobject.GetComponent<ObjectRaycastCheck>().ray_trig = false;
        }
        else if (hit_gameobject.CompareTag("Door"))
        {
            hit_gameobject.GetComponent<RotateHingePhysics>().ray_trig = false;
        }
        else if (hit_gameobject.CompareTag("Drawer"))
        {
            hit_gameobject.GetComponent<MoveDrawerPhysics>().ray_trig = false;
        }
    }

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
            // If a Different GameObject was Hit in the Next Update, Disable the Previous GameObject's Variable

            if (hit_flag && hit.transform.gameObject != hit_gameobject)
            {
                disableHit();

                hit_flag = false;
            }

            if (hit.transform.gameObject.CompareTag("Interactable"))
            {
                hit_flag = true;

                hit_gameobject = hit.transform.gameObject;

                hit_gameobject.GetComponent<ObjectRaycastCheck>().ray_trig = true;
            }
            else if (hit.transform.gameObject.CompareTag("Door"))
            {
                hit_flag = true;

                hit_gameobject = hit.transform.gameObject;

                hit_gameobject.GetComponent<RotateHingePhysics>().ray_trig = true;
            }
            else if (hit.transform.gameObject.CompareTag("Drawer"))
            {
                hit_flag = true;

                hit_gameobject = hit.transform.gameObject;

                hit_gameobject.GetComponent<MoveDrawerPhysics>().ray_trig = true;
            }

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
        else
        {
            if (hit_flag)
            {
                disableHit();
            }

            hit_flag = false;
        }
    }
}