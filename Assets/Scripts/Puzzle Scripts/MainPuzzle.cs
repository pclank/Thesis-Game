using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPuzzle : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject use_ui;                                       // Use UI Element

    public int[] solution_list;                                     // List Containing Solution

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

        use_ui.SetActive(true);             // Enable UI Element
    }

    // Reset

    public void resetSlot()
    {
        player_object.GetComponent<main_inventory>().setSlotFlag(false, this.gameObject);   // Player Can't Use Item on Slot

        use_ui.SetActive(false);            // Disable UI Element
    }

    // Set Item on Slot

    public void setItem(int item_id)
    {
        slot_list[selected_slot.GetComponent<ItemSlot>().slot_id] = item_id;                // Set Item ID to Current Slot

        // Check If Slots are Filled According to Solution
        if (slot_list.Equals(solution_list))
        {
            openAfterSolution();                        // Act
        }
    }

    // Unset Item from Slot

    public void unsetItem()
    {
        slot_list[selected_slot.GetComponent<ItemSlot>().slot_id] = 0;                      // Unset
    }

    // Do Something on Solution

    private void openAfterSolution()
    {
        // TODO: Add Code!
    }

    // Start is called before the first frame update
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");           // Set Player GameObject
    }
}
