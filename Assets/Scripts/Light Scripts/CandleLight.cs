using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;

// ************************************************************************************
// Script to Simulate Candle Light, Including Movement and Intensity - Range Change
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

    [Tooltip("Enable Range FX.")]
    public bool range_enabled = false;

    public float max_movement = 1.0f;
    public float movement_speed = 0.1f;
    public float min_intensity = 1.0f;
    public float intensity_speed = 0.1f;
    public float min_range = 1.0f;
    public float range_speed = 0.1f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private Vector3 initial_position;                                   // Initial Worldspace Position

    private bool intensity_decreasing = true;                           // Whether Intensity is Decreasing
    private bool range_decreasing = true;                               // Whether Range is Decreasing
    private bool position_decreasing = true;                            // Whether Position is Decreasing

    private float initial_intensity;                                    // Initial Intensity
    private float initial_range;                                        // Initial Range
    private float lower_position;                                       // Lower Position Bound
    private float upper_position;                                       // Upper Position Bound

    // Use this for initialization
    void Start()
    {
        // Get Initial Values

        initial_position = transform.position;
        initial_intensity = GetComponent<HDAdditionalLightData>().intensity;
        initial_range = GetComponent<HDAdditionalLightData>().range;

        // Set Range of Movement

        if (x_enabled)
        {
            lower_position = initial_position.x - max_movement;
            upper_position = initial_position.x + max_movement;
        }
        else if (z_enabled)
        {
            lower_position = initial_position.z - max_movement;
            upper_position = initial_position.z + max_movement;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Add Delay Functionality

        // Process Intensity
        if (intensity_enabled)
        {
            float new_intensity = 0.0f;

            // Decreasing Intensity
            if (intensity_decreasing)
            {
                new_intensity = GetComponent<HDAdditionalLightData>().intensity - intensity_speed;

                // Edge Case
                if (new_intensity <= min_intensity)
                {
                    GetComponent<HDAdditionalLightData>().intensity = min_intensity;

                    intensity_decreasing = false;                                                       // Update Direction Flag
                }
                else
                {
                    GetComponent<HDAdditionalLightData>().intensity = new_intensity;                    // Update Intensity
                }
            }
            // Increasing Intensity
            else
            {
                new_intensity = GetComponent<HDAdditionalLightData>().intensity + intensity_speed;

                // Edge Case
                if (new_intensity >= initial_intensity)
                {
                    GetComponent<HDAdditionalLightData>().intensity = initial_intensity;

                    intensity_decreasing = true;                                                        // Update Direction Flag
                }
                else
                {
                    GetComponent<HDAdditionalLightData>().intensity = new_intensity;                    // Update Intensity
                }
            }
        }

        // Process Range
        if (range_enabled)
        {
            float new_range = 0.0f;

            // Decreasing Range
            if (range_decreasing)
            {
                new_range = GetComponent<HDAdditionalLightData>().range - range_speed;

                // Edge Case
                if (new_range <= min_range)
                {
                    GetComponent<HDAdditionalLightData>().range = min_range;                            // Update Range

                    range_decreasing = false;                                                           // Update Direction Flag
                }
                else
                {
                    GetComponent<HDAdditionalLightData>().range = new_range;                            // Update Range
                }
            }
            // Increasing Range
            else
            {
                new_range = GetComponent<HDAdditionalLightData>().range + range_speed;

                // Edge Case
                if (new_range >= initial_range)
                {
                    GetComponent<HDAdditionalLightData>().range = initial_range;                        // Update Range

                    range_decreasing = true;                                                            // Update Direction Flag
                }
                else
                {
                    GetComponent<HDAdditionalLightData>().range = new_range;                            // Update Range
                }
            }
        }

        // Process Movement
        if (movement_enabled)
        {
            // Process X-Axis
            if (x_enabled)
            {
                float new_position = 0.0f;

                // Decreasing Position
                if (position_decreasing)
                {
                    new_position = transform.position.x - movement_speed;

                    // Edge Case
                    if (new_position <= lower_position)
                    {
                        transform.position = new Vector3(lower_position, transform.position.y, transform.position.z);   // Update Position

                        position_decreasing = false;                                                                    // Update Direction Flag
                    }
                    else
                    {
                        transform.position = new Vector3(new_position, transform.position.y, transform.position.z);     // Update Position
                    }
                }
                // Increasing Position
                else
                {
                    new_position = transform.position.x + movement_speed;

                    // Edge Case
                    if (new_position >= upper_position)
                    {
                        transform.position = new Vector3(upper_position, transform.position.y, transform.position.z);   // Update Position

                        position_decreasing = true;                                                                     // Update Direction Flag
                    }
                    else
                    {
                        transform.position = new Vector3(new_position, transform.position.y, transform.position.z);     // Update Position
                    }
                }
            }

            // Process Z-Axis
            else if (z_enabled)
            {
                float new_position = 0.0f;

                // Decreasing Position
                if (position_decreasing)
                {
                    new_position = transform.position.z - movement_speed;

                    // Edge Case
                    if (new_position <= lower_position)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, lower_position);   // Update Position

                        position_decreasing = false;                                                                    // Update Direction Flag
                    }
                    else
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, new_position);     // Update Position
                    }
                }
                // Increasing Position
                else
                {
                    new_position = transform.position.z + movement_speed;

                    // Edge Case
                    if (new_position >= upper_position)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, upper_position);   // Update Position

                        position_decreasing = true;                                                                     // Update Direction Flag
                    }
                    else
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, new_position);     // Update Position
                    }
                }
            }
        }
    }
}