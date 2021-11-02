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

    [Tooltip("Flame GameObject.")]
    public GameObject flame_object;

    [Tooltip("Enable Random Startup Delay.")]
    public bool rand_delay_on = true;

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

    [Tooltip("Synchronize Intensity with Movement.")]
    public bool sync = false;

    [Tooltip("Number of Movement Cycles Before Delay.")]
    public uint cycles = 1;

    [Tooltip("Movement Delay.")]
    public float movement_delay = 2.0f;
    [Tooltip("Intensity Delay.")]
    public float intensity_delay = 2.0f;
    [Tooltip("Range Delay.")]
    public float range_delay = 2.0f;

    [Tooltip("Center Position Error.")]
    public float center_error = 0.01f;

    public float max_movement = 1.0f;
    public float movement_speed = 0.1f;
    public float min_intensity = 1.0f;
    public float intensity_speed = 0.1f;
    public float min_range = 1.0f;
    public float range_speed = 0.1f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private Material new_flame;                                         // New Material for Flame

    private Vector3 initial_position;                                   // Initial Worldspace Position

    private bool intensity_decreasing = true;                           // Whether Intensity is Decreasing
    private bool range_decreasing = true;                               // Whether Range is Decreasing
    private bool position_decreasing = true;                            // Whether Position is Decreasing
    private bool movement_t_on = false;                                 // Whether Movement Timer is On
    private bool intensity_t_on = false;                                // Whether Intensity Timer is On
    private bool range_t_on = false;                                    // Whether Range Timer is On
    private bool sync_done = false;                                     // Whether Synchronized Intensity Processing was Performed in Previous Frame

    private float initial_intensity;                                    // Initial Intensity
    private float initial_range;                                        // Initial Range
    private float lower_position;                                       // Lower Position Bound
    private float upper_position;                                       // Upper Position Bound

    private float movement_t = 0.0f;                                    // Movement Timer Value
    private float intensity_t = 0.0f;                                   // Intensity Timer Value
    private float range_t = 0.0f;                                       // Range Timer Value

    private float random_delayed_start;                                 // Random Delayed Start

    private uint movement_state = 0;                                    // Used to Count State of Movement Cycle

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Change Flame Transparency
    private void changeFlameTransparency(float new_alpha)
    {
        new_flame = flame_object.GetComponent<MeshRenderer>().material;
        new_flame.color = new Color(new_flame.color.r, new_flame.color.g, new_flame.color.b, new_alpha);
    }

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

        // Setup Startup Delay Parameters
        if (rand_delay_on)
        {
            movement_t = Time.time;
            movement_t_on = true;

            random_delayed_start = Random.value * 10.0f;            // Extend Value
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Process Intensity
        if (intensity_enabled && (!intensity_t_on || (intensity_t_on && (Time.time - intensity_t >= intensity_delay))))
        {
            float new_intensity = 0.0f;

            intensity_t_on = false;                                                             // Reset Flag

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
                    intensity_t_on = true;                                                              // Enable Delay

                    intensity_t = Time.time;                                                            // Get Timer Value
                }
                else
                {
                    GetComponent<HDAdditionalLightData>().intensity = new_intensity;                    // Update Intensity
                }
            }
        }

        // Process Range
        if (range_enabled && (!range_t_on || (range_t_on && (Time.time - range_t >= range_delay))))
        {
            float new_range = 0.0f;

            range_t_on = false;                                                                 // Reset Flag

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
                    range_t_on = true;                                                                  // Enable Delay

                    range_t = Time.time;                                                                // Get Timer Value
                }
                else
                {
                    GetComponent<HDAdditionalLightData>().range = new_range;                            // Update Range
                }
            }
        }

        // Process Movement
        if (movement_enabled && (!movement_t_on || (movement_t_on && (Time.time - movement_t >= movement_delay)) || (rand_delay_on && (Time.time - movement_t >= random_delayed_start))))
        {
            // Disable Startup Delay on First Run
            if (rand_delay_on)
            {
                rand_delay_on = false;
            }

            movement_t_on = false;                                                              // Reset Flag

            // Check for Intensity - Movement Synchronization Mode
            if (sync && !sync_done)
            {
                changeFlameTransparency(0.2f);
                GetComponent<HDAdditionalLightData>().intensity = min_intensity;

                sync_done = true;
            }

            // Process X-Axis
            if (x_enabled)
            {
                float new_position = 0.0f;

                // Decreasing Position
                if (position_decreasing)
                {
                    new_position = transform.position.x - movement_speed;

                    // Center Case
                    if (movement_state == (cycles * 2) && new_position <= (initial_position.x + center_error))
                    {
                        transform.position = initial_position;                                                          // Update Position

                        // Check for Intensity - Movement Synchronization Mode
                        if (sync)
                        {
                            changeFlameTransparency(1.0f);
                            GetComponent<HDAdditionalLightData>().intensity = initial_intensity;

                            sync_done = false;
                        }

                        movement_state = 0;                                                                             // Reset State

                        movement_t_on = true;                                                                           // Enable Delay

                        movement_t = Time.time;                                                                         // Get Timer Value
                    }
                    // Edge Case
                    else if (new_position <= lower_position)
                    {
                        transform.position = new Vector3(lower_position, transform.position.y, transform.position.z);   // Update Position

                        movement_state++;                                                                               // Update State

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

                        movement_state++;                                                                               // Update State

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

                    // Center Case
                    if (movement_state == (cycles * 2) && new_position <= (initial_position.z + center_error))
                    {
                        transform.position = initial_position;                                                          // Update Position

                        movement_state = 0;                                                                             // Reset State

                        movement_t_on = true;                                                                           // Enable Delay

                        movement_t = Time.time;                                                                         // Get Timer Value
                    }
                    // Edge Case
                    else if (new_position <= lower_position)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, lower_position);   // Update Position

                        movement_state++;                                                                               // Update State

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

                        movement_state++;                                                                               // Update State

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