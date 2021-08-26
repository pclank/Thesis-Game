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

    public Camera keypad_camera;                // Keypad Camera

    public float max_distance = 10.0f;          // Distance for Raycasting
    public int excluded_layer = 8;              // Layer to Exclude from Raycasting

    public bool cast_flag = true;               // Flag Allowing Raycasting

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
        if (gameObject.CompareTag("MainCamera"))
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
            else if (hit_gameobject.CompareTag("Slot"))
            {
                hit_gameobject.GetComponent<ItemSlot>().ray_trig = false;
            }
            else if (hit_gameobject.CompareTag("Keypad"))
            {
                hit_gameobject.GetComponent<Keypad>().ray_keypad = false;
                hit_gameobject.GetComponent<Keypad>().ray_trig = false;
            }
        }
        else if (!gameObject.CompareTag("MainCamera") && hit_gameobject.CompareTag("KeypadButton"))
        {
            hit_gameobject.GetComponentInParent<Keypad>().ray_trig = false;

            hit_gameobject.GetComponentInParent<Keypad>().setKeyPressed(-1);
        }
    }

    // Use this for initialization
    void Start()
    {
        layer_mask = 1 << excluded_layer;           // Bit Shift to Get Layer Bitmask
        layer_mask = layer_mask ^ (1 << 5);         // Exclude UI Layer
        layer_mask = ~layer_mask;                   // Invert Bitmask

        Cursor.visible = false;                     // Disable Cursor
    }

    // Update Native Player Loop
    void FixedUpdate()
    {
        RaycastHit hit;

        // Camera Based Raycast
        if (gameObject.CompareTag("MainCamera") && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, max_distance, layer_mask, QueryTriggerInteraction.Collide))  // Generate Ray and Trigger Colliders
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
            else if (hit.transform.gameObject.CompareTag("Slot"))
            {
                hit_flag = true;

                hit_gameobject = hit.transform.gameObject;

                hit_gameobject.GetComponent<ItemSlot>().ray_trig = true;
            }
            else if (hit.transform.gameObject.CompareTag("Keypad"))
            {
                hit_flag = true;

                hit_gameobject = hit.transform.gameObject;

                hit_gameobject.GetComponent<Keypad>().ray_keypad = true;
            }
            else if (hit.transform.gameObject.CompareTag("KeypadButton"))
            {
                hit_flag = true;

                hit_gameobject = hit.transform.gameObject;

                hit_gameobject.GetComponentInParent<Keypad>().ray_trig = true;

                hit_gameobject.GetComponentInParent<Keypad>().setKeyPressed(hit_gameobject.GetComponent<KeypadButton>().key_id);
            }

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
        // Cursor Based Raycast
        else if (!gameObject.CompareTag("MainCamera") && cast_flag && Physics.Raycast(keypad_camera.ScreenPointToRay(Input.mousePosition), out hit, max_distance, layer_mask, QueryTriggerInteraction.Collide))
        {
            // If a Different GameObject was Hit in the Next Update, Disable the Previous GameObject's Variable

            if (hit_flag && hit.transform.gameObject != hit_gameobject)
            {
                disableHit();

                hit_flag = false;
            }

            if (hit.transform.gameObject.CompareTag("KeypadButton"))
            {
                hit_flag = true;

                hit_gameobject = hit.transform.gameObject;

                hit_gameobject.GetComponentInParent<Keypad>().ray_trig = true;

                hit_gameobject.GetComponentInParent<Keypad>().setHitObject(hit_gameobject);

                hit_gameobject.GetComponentInParent<Keypad>().setKeyPressed(hit_gameobject.GetComponent<KeypadButton>().key_id);
            }

            Debug.DrawRay(keypad_camera.ScreenPointToRay(Input.mousePosition).origin, hit.point, Color.yellow);
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