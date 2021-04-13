using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_rotator_parent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // If the Trigger is the Player
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInChildren<door_rotator>().OnPlEnter(other);
        }
    }

    // Player Exits Interaction Position
    private void OnTriggerExit(Collider other)
    {
        // If the Trigger is the Player
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInChildren<door_rotator>().OnPlExit(other);
        }
    }
}
