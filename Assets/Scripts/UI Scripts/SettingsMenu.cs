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

    public Toggle motionblur_toggle;
    public Toggle vsync_toggle;

    [Tooltip("Array of Volumes.")]
    public GameObject[] volume_objects = new GameObject[3];

    public GameObject menu_ui;

    public Camera main_camera;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool menu_enabled = false;                          // Whether Pause Menu is Enabled

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Open Settings Menu from other Script
    public void openSettingsMenu()
    {
        menu_ui.SetActive(true);

        menu_enabled = true;
    }

    // Process Motion Blur Toggle
    private void processMotionblurChange()
    {
        // Iterate Through All Volumes to Process
        foreach (GameObject volume in volume_objects)
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