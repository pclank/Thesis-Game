using UnityEngine;
using System.Collections;

public class RotatingRoomPlayerDetect : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject rotating_object;

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rotating_object.GetComponent<RotatingRooms>().player_trigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rotating_object.GetComponent<RotatingRooms>().resetTimer();

            rotating_object.GetComponent<RotatingRooms>().player_trigger = false;
        }
    }
}