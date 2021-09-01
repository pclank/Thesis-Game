using System;
using System.Linq;
using UnityEngine;

public class MainPuzzle : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject use_ui;                                       // Use UI Element
    public GameObject portal_gameobject;                            // Portal to Appear GameObject

    public int[] solution_list;                                     // List Containing Solution
    public int[] valid_list;                                        // List of Valid Items

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private int[] slot_list = new int[5] { 0, 0, 0, 0, 0 };         // Array Containing Item ID for Each Slot

    private GameObject selected_slot;                               // Slot Camera is Pointing At
    private GameObject player_object;                               // Player GameObject

    private bool solved = false;                                    // Whether Puzzle Has Been Solved

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Check if Item is Valid

    public bool checkValid(int i_id)
    {
        return Array.Exists(valid_list, element => element == i_id);
    }

    // Set Slot as Usable

    public void setSlot(GameObject used_slot)
    {
        // Check if Puzzle has Already Been Solved
        if (!solved)
        {
            selected_slot = used_slot;          // Set Slot Index

            player_object.GetComponent<main_inventory>().setSlotFlag(true, used_slot);          // Player Can Use Item on Slot

            use_ui.SetActive(true);             // Enable UI Element
        }
    }

    // Reset

    public void resetSlot()
    {
        // Check if Puzzle has Already Been Solved
        if (!solved)
        {
            player_object.GetComponent<main_inventory>().setSlotFlag(false, this.gameObject);   // Player Can't Use Item on Slot

            use_ui.SetActive(false);            // Disable UI Element
        }
    }

    // Set Item on Slot

    public void setItem(int item_id)
    {
        slot_list[selected_slot.GetComponent<ItemSlot>().slot_id] = item_id;                // Set Item ID to Current Slot

        // Check If Slots are Filled According to Solution
        if (slot_list.SequenceEqual(solution_list))
        {
            Debug.Log("Puzzle Solved!");

            openAfterSolution();                        // Act

            use_ui.SetActive(false);                    // Disable UI Element

            solved = true;                              // Set Puzzle as Solved
        }
    }

    // Unset Item from Slot

    public void unsetItem()
    {
        slot_list[selected_slot.GetComponent<ItemSlot>().slot_id] = 0;                      // Unset
    }

    // Called from Other Objects to Check if Puzzle is Solved

    public bool checkSolved()
    {
        return solved;
    }

    // Do Something on Solution

    private void openAfterSolution()
    {
        portal_gameobject.GetComponent<PortalPhysics>().activatePortal();
    }

    // Start is called before the first frame update
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");           // Set Player GameObject
    }
}
