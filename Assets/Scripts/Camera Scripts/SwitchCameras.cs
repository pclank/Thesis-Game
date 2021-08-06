using UnityEngine;
using System.Collections;

// ************************************************************************************
// Switch Between Cameras
// ************************************************************************************

public class SwitchCameras : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public Camera new_camera;

    // ************************************************************************************
    // Private Cameras
    // ************************************************************************************

    private Camera default_camera;

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    public void switchCamera()
    {
        default_camera.enabled = false;     // Disable Current Camera

        new_camera.enabled = true;          // Enable New Camera
    }

    public void resetDefaultCamera()
    {
        new_camera.enabled = false;         // Disable Current Camera

        default_camera.enabled = true;      // Enable Default Camera
    }

    // First Frame Only
    void Start()
    {
        default_camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();   // Get Default Camera
    }
}