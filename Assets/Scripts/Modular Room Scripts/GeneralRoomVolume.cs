using UnityEngine;
using System.Collections;

// ************************************************************************************
// General Room - Volume Class Script for Analytics Purposes
// ************************************************************************************

[RequireComponent(typeof(BoxCollider))]
public class GeneralRoomVolume : MonoBehaviour
{
    [Tooltip("Room - Volume Name.")]
    public string room_name = "";

    [Tooltip("Whether to Initialize Data Storage after This Trigger Exit.")]
    public bool export_data_on_exit = false;

    private float enter_time;

    // Set Enter Time
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            enter_time = Time.time;
    }

    // Add to Analytics
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").GetComponent<RoomVolumeAnalytics>().addAnalytics(room_name, enter_time);  // Add Analytics on Exit

            // Export Data
            if (export_data_on_exit)
                GameObject.FindWithTag("Player").GetComponent<RoomVolumeAnalytics>().recordAnalytics();

            Destroy(gameObject);                // Destroy Trigger GameObject
        }
    }
}