using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    // Public Variable

    public int slot_id = 0;

    private GameObject item_object;

    private MainPuzzle main_puzzle;

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    public void placeItem(Item item)
    {
        GameObject new_go = new GameObject(item.getName());                                             // Create GameObject Object Using Constructor

        new_go.AddComponent<MeshFilter>(item.getMeshFilter());                                          // Add MeshFilter
        Material mat = item.getMaterial();                                                              // Get Material from Renderer
        var mr = new_go.AddComponent<MeshRenderer>(item.getMeshRenderer());                             // Add MeshRenderer
        mr.material = mat;                                                                              // Assign Material

        new_go.transform.position = this.gameObject.transform.position;                                 // Place in Slot

        float scale = item.getScale();                                                                  // Get Scale
        new_go.transform.localScale = new Vector3(scale, scale, scale);                                 // Set Scale

        main_puzzle.setItem(item.getID());                                                              // Set Item ID on Puzzle List
    }

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        // Check that Collider is the MainCamera
        if (other.gameObject.CompareTag("MainCamera"))
        {
            main_puzzle.setSlot(this.gameObject);        // Set Selected Slot
        }
    }

    private void OnTriggerExit(Collider other)
    {
        main_puzzle.resetSlot();                // Reset Slot
    }

    void Start()
    {
        main_puzzle = this.GetComponentInParent<MainPuzzle>();              // Set Parent Script
    }
}
