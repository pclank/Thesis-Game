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

    public Toggle examine_on_pickup_toggle;                             // Examine On Pickup Toggle
    public Toggle motionblur_toggle;                                    // Motion Blur Toggle
    public Toggle vsync_toggle;                                         // V-Sync Toggle
    public Toggle subtitle_toggle;                                      // Subtitle Toggle

    [Tooltip("Array of Volumes.")]
    public Volume[] volume_objects = new Volume[4];

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Process Examine On Pickup Toggle
    private void processExamineOnPickupChange()
    {
        GameObject.FindWithTag("Player").GetComponent<main_inventory>().display_on_pickup = examine_on_pickup_toggle.isOn;  // Toggle Display On Pickup
    }

    // Process Motion Blur Toggle
    private void processMotionblurChange()
    {
        // Iterate Through All Volumes to Process
        foreach (Volume volume in volume_objects)
        {
            // Null Check
            if (volume)
            {
                MotionBlur mb;                                              // Declare Motion Blur Variable

                // Get Motion Blur Component
                if (volume.profile.TryGet(out mb))
                {
                    mb.active = motionblur_toggle.isOn;                         // Toggle Motion Blur
                }
            }
        }
    }

    // Process V-Sync Toggle
    private void processVsyncChange()
    {
        // TODO: Add Functionality?!
    }

    // Process Subtitle Toggle
    private void processSubtitleChange()
    {
        GameObject.FindWithTag("Player").GetComponent<SubtitleControl>().changeSubtitlesOn(subtitle_toggle.isOn);
    }

    // Use this for initialization
    void Start()
    {
        // Set Value Change Listener for Toggle UI Elements

        examine_on_pickup_toggle.onValueChanged.AddListener(delegate
        {
            processExamineOnPickupChange();
        });

        motionblur_toggle.onValueChanged.AddListener(delegate
        {
            processMotionblurChange();
        });

        vsync_toggle.onValueChanged.AddListener(delegate
        {
            processVsyncChange();
        });

        subtitle_toggle.onValueChanged.AddListener(delegate
        {
            processSubtitleChange();
        });
    }
}