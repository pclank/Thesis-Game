using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPuzzle : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************



    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private int[] slot_list = new int[5] { 0, 0, 0, 0, 0 };         // Array Containing Item ID for Each Slot

    private int selected_slot;                                      // Slot Camera is Pointing At

    private GameObject player_object;                               // Player GameObject

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Set Slot as Usable

    public void setSlot(int used_slot)
    {
        selected_slot = used_slot;          // Set Slot Index

        player_object.GetComponent<main_inventory>().setSlotFlag(true, this.gameObject);   // Player Can Use Item on Slot
    }

    // Reset

    public void resetSlot()
    {
        player_object.GetComponent<main_inventory>().setSlotFlag(false, this.gameObject);  // Player Can't Use Item on Slot
    }

    // Set Item on Slot

    public void setItem(Item item)
    {
        slot_list[selected_slot] = item.getID();        // Set Item ID to Current Slot


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
