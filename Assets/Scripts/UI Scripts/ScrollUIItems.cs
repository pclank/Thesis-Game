using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

// ************************************************************************************
// Scrollbar Script for Inventory Item List
// ************************************************************************************

public class ScrollUIItems : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Scroll-Speed Smoothness.")]
    public float smoothness = 1.0f;                         // Scroll Smoothness Factor

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private VerticalLayoutGroup item_list;                  // Inventory Item List Vertical Layout Group

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Change Layout Padding Based on Scrollbar Position, On Scrollbar Event
    public void scrollList(float scrollbar_value)
    {
        item_list.padding.top = -(int)(smoothness * scrollbar_value * 1000);        // Calculate and Set New Padding
        item_list.SetLayoutVertical();                                              // Force-Update Layout
    }

    // Use this for initialization
    void Start()
    {
        item_list = GetComponent<VerticalLayoutGroup>();                            // Get Vertical Layout Group
    }
}