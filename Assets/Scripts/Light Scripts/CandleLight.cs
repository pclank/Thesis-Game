using UnityEngine;
using System.Collections;

// ************************************************************************************
// Script to Simulate Candle Light, Including Movement and Intensity - Radius Change
// ************************************************************************************

public class CandleLight : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Enable Movement FX.")]
    public bool movement_enabled = true;

    [Tooltip("Enable Movement on X-Axis.")]
    public bool x_enabled = false;

    [Tooltip("Enable Movement on Z-Axis.")]
    public bool z_enabled = false;

    [Tooltip("Enable Intensity FX.")]
    public bool intensity_enabled = false;

    [Tooltip("Enable Radius FX.")]
    public bool radius_enabled = false;

    public float max_movement = 1.0f;
    public float movement_speed = 0.1f;
    public float min_intensity = 1.0f;
    public float min_radius = 1.0f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private Vector3 initial_position;                                   // Initial Worldspace Position

    private float initial_intensity;                                    // Initial Intensity
    private float initial_radius;                                       // Initial Radius

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}