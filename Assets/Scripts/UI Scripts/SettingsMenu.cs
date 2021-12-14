using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using System;

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

    public Slider subtitle_font_size_slider;                            // Subtitle Font Size Slider

    public Text subtitle_font_size;                                     // Subtitle Font Size Text

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

    // Process Subtitle Font Size
    private void processSubtitleFontSizeChange()
    {
        try
        {
            subtitle_font_size.text = subtitle_font_size_slider.value.ToString();

            GameObject.FindWithTag("Player").GetComponent<SubtitleControl>().changeSubtitleFont((int)subtitle_font_size_slider.value);
        }
        catch (Exception)
        {
            Debug.LogError("Invalid Font Size Input!");

            throw;
        }
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

        subtitle_font_size_slider.onValueChanged.AddListener(delegate
        {
            processSubtitleFontSizeChange();
        });
    }
}