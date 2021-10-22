﻿using UnityEngine;
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

    [Tooltip("Array of Volumes.")]
    public Volume[] volume_objects = new Volume[3];

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
    }
}