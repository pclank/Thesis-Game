using UnityEngine;
using System.Collections;

// ************************************************************************************
// Disables or Enables Rooms Based on Triggers
// ************************************************************************************

public class StateBasedOptimization : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("GameObject to Disable or Enable.")]
    public GameObject target_object;

    [Tooltip("When True, the target is Enabled. Else it's Disabled.")]
    public bool enable = true;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool has_run = false;                           // Whether Script Has Run

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (!has_run && other.CompareTag("Player"))
        {
            target_object.SetActive(enable);

            has_run = true;
        }
    }
}