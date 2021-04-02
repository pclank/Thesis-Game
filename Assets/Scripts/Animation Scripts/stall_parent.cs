using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stall_parent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // If the Trigger is the Player
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInChildren<stall_door>().OnPlEnter(other);
        }
    }

    // Player Exits Interaction Position
    private void OnTriggerExit(Collider other)
    {
        // If the Trigger is the Player
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInChildren<stall_door>().OnPlExit(other);
        }
    }
}
