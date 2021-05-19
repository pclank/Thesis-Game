using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSlot : MonoBehaviour
{
    // Public Variable

    int slot_id = 0;

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        // Check that Collider is the MainCamera
        if (other.gameObject.CompareTag("MainCamera"))
        {
            this.GetComponentInParent<CoinPuzzle>().setSlot(slot_id);        // Set Selected Slot
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.GetComponentInParent<CoinPuzzle>().resetSlot();                // Reset Slot
    }
}
