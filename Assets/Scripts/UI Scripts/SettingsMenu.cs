using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using System.Collections;

// ************************************************************************************
// Settings Menu
// ************************************************************************************

public class SettingsMenu : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public Toggle motionblur_toggle;                                    // Motion Blur Toggle
    public Toggle vsync_toggle;                                         // V-Sync Toggle

    public Button close_button;                                         // Button to Close This Sub-Menu

    [Tooltip("Array of Volumes.")]
    public GameObject[] volume_objects = new GameObject[3];

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Process Motion Blur Toggle
    private void processMotionblurChange()
    {
        // Iterate Through All Volumes to Process
        foreach (GameObject volume in volume_objects)
        {
            // Null Check
            if (volume)
            {
                Volume volume_comp = volume.GetComponent<Volume>();         // Get Volume Component

                MotionBlur mb;                                              // Declare Motion Blur Variable

                // Get Motion Blur Component
                if (volume_comp.profile.TryGet(out mb))
                {
                    mb.active = motionblur_toggle.isOn;                         // Toggle Motion Blur
                }
            }
        }
    }

    // Close This Sub-Menu
    private void closeMenu()
    {
        gameObject.SetActive(false);
    }

    // Process V-Sync Toggle
    private void processVsyncChange()
    {
        // TODO: Add Functionality?!
    }

    // Use this for initialization
    void Start()
    {
        // Set Value Change Listener for Toggle UI Elements

        motionblur_toggle.onValueChanged.AddListener(delegate
        {
            processMotionblurChange();
        });

        vsync_toggle.onValueChanged.AddListener(delegate
        {
            processVsyncChange();
        });

        // Add Listener to Button
        close_button.onClick.AddListener(() => closeMenu());
    }
}