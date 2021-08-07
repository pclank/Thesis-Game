using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public int slot_id = 0;

    public bool ray_trig = false;   // Raycasting Hit Flag

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject item_object;

    private Item slot_item;

    private MainPuzzle main_puzzle;

    private bool prev_hit = false;      // Hit was Detected in Previous Update

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Place Item in Slot

    public bool placeItem(Item item)
    {
        bool validation = main_puzzle.checkValid(item.getID());

        // Check that Item Is Valid for Placement in Slot
        if (validation)
        {
            GameObject new_go = new GameObject("temp_gameobject");                                          // Create GameObject Object Using Constructor

            new_go.AddComponent<MeshFilter>(item.getMeshFilter());                                          // Add MeshFilter
            Material mat = item.getMaterial();                                                              // Get Material from Renderer
            var mr = new_go.AddComponent<MeshRenderer>(item.getMeshRenderer());                             // Add MeshRenderer
            mr.material = mat;                                                                              // Assign Material

            new_go.transform.position = this.gameObject.transform.position;                                 // Place in Slot

            float scale = item.getScale();                                                                  // Get Scale
            new_go.transform.localScale = new Vector3(scale, scale, scale);                                 // Set Scale

            new_go.transform.Rotate(90, 90, 0, Space.Self);                                                 // Rotate Object

            main_puzzle.setItem(item.getID());                                                              // Set Item ID on Puzzle List

            item_object = new_go;                                                                           // Set Object
            slot_item = item;                                                                               // Set Item
        }

        return validation;                                                                                  // Return Validation Back to main_inventory
    }

    // Remove Item

    public Item removeItem()
    {
        // Check that Slot isn't Empty

        if (item_object != null)
        {
            main_puzzle.unsetItem();        // Unset Item

            Destroy(item_object);           // Destroy Object

            return slot_item;
        }
        else
        {
            return null;
        }
    }

    void Start()
    {
        main_puzzle = this.GetComponentInParent<MainPuzzle>();              // Set Parent Script
    }

    void Update()
    {
        if (!prev_hit && ray_trig)
        {
            main_puzzle.setSlot(this.gameObject);   // Set Selected Slot

            prev_hit = true;
        }
        else if (prev_hit && !ray_trig)
        {
            main_puzzle.resetSlot();                // Reset Slot

            prev_hit = false;
        }
    }
}