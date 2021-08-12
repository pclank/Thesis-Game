using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;

public class LightIntensityFX : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Number of Quick Change Cycles, before Reset Cycle.")]
    public int n_quick_cycles = 3;              // Number of Quick Cycles

    [Tooltip("Minimum Intensity During Quick Cycles.")]
    public float minimum_intensity = 0.0f;      // Minimum Light Intensity During Quick Cycles
    [Tooltip("Intensity Step for Quick Cycles.")]
    public float quick_step = 0.1f;             // Quick Cycle Step
    [Tooltip("Intensity Step for Reset Cycle.")]
    public float reset_step = 0.05f;            // Reset Cycle Step

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private int cnt = 0;                        // Counts Full Cycles
    
    private float starting_intensity;           // Light Intensity at Start Time

    private bool reverse_cycle = false;         // Flag to Reverse Quick Cycle

    // Use this for initialization
    void Start()
    {
        starting_intensity = gameObject.GetComponent<HDAdditionalLightData>().intensity;    // Get Starting Intensity
    }

    // Update is called once per frame
    void Update()
    {
        float new_intensity;

        // Quick Cycles
        if (cnt < n_quick_cycles)
        {
            // Increase
            if (reverse_cycle)
            {
                new_intensity = gameObject.GetComponent<HDAdditionalLightData>().intensity + quick_step;  // Calculate New Intensity

                // Check for Reverse
                if (new_intensity >= starting_intensity)
                {
                    new_intensity = starting_intensity;

                    reverse_cycle = false;                  // Reverse Off

                    cnt++;                                  // Increment Cycles
                }
            }
            // Decrease
            else
            {
                new_intensity = gameObject.GetComponent<HDAdditionalLightData>().intensity - quick_step;  // Calculate New Intensity

                // Check for Reverse
                if (new_intensity <= minimum_intensity)
                {
                    new_intensity = minimum_intensity;

                    reverse_cycle = true;                   // Reverse On
                }
            }
        }
        // Reset Cycle
        else
        {
            // Increase
            if (reverse_cycle)
            {
                new_intensity = gameObject.GetComponent<HDAdditionalLightData>().intensity + reset_step;  // Calculate New Intensity

                // Check for Reverse
                if (new_intensity >= starting_intensity)
                {
                    new_intensity = starting_intensity;

                    reverse_cycle = false;                  // Reverse Off

                    cnt = 0;                                // Reset Cycles
                }
            }
            // Decrease
            else
            {
                new_intensity = gameObject.GetComponent<HDAdditionalLightData>().intensity - reset_step;  // Calculate New Intensity

                // Check for Reverse
                if (new_intensity <= minimum_intensity)
                {
                    new_intensity = minimum_intensity;

                    reverse_cycle = true;                   // Reverse On
                }
            }
        }

        gameObject.GetComponent<HDAdditionalLightData>().intensity = new_intensity;                     // Update Intensity
    }
}