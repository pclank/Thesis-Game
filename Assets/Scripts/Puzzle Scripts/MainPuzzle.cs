using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPuzzle : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************



    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private int[] slot_list = new int[5] { 0, 0, 0, 0, 0 };         // Array Containing Item ID for Each Slot

    private GameObject selected_slot;                               // Slot Camera is Pointing At
    private GameObject player_object;                               // Player GameObject

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Set Slot as Usable

    public void setSlot(GameObject used_slot)
    {
        selected_slot = used_slot;          // Set Slot Index

        player_object.GetComponent<main_inventory>().setSlotFlag(true, used_slot);          // Player Can Use Item on Slot
    }

    // Reset

    public void resetSlot()
    {
        player_object.GetComponent<main_inventory>().setSlotFlag(false, this.gameObject);   // Player Can't Use Item on Slot
    }

    // Set Item on Slot

    public void setItem(int item_id)
    {
        slot_list[selected_slot.GetComponent<ItemSlot>().slot_id] = item_id;                // Set Item ID to Current Slot
    }

    // Start is called before the first frame update
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");           // Set Player GameObject
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
