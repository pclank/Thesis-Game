using UnityEngine;
using System.Collections;

// ************************************************************************************
// Automatically Remove Decoration GameObjects that Clips through Modular Walls
// ************************************************************************************

public class ClearDecoration : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Tag to Look for.")]
    public string tgt_tag = "Decoration";

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        // Check GameObject Tag
        if (other.gameObject.CompareTag(tgt_tag))
        {
            Destroy(other.gameObject);                  // Destroy GameObject
        }
    }
}