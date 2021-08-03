using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.HighDefinition;
public class LightChainFX : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Light is Chained to Other Light")]
    public bool chained = false;                // Whether Light is Chained to Run Other Light's Script

    [Tooltip("Chained Light Call is Delayed")]
    public bool delayed = false;                // Whether Running Other Script is Delayed

    [Tooltip("Chained Call Delay")]
    public float delay = 1.0f;                  // Delay

    [Tooltip("Other Light GameObject")]
    public GameObject chained_light;            // Chained Light GameObject

    public bool light_on = true;                // Light State

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private float initial_intensity = 0.0f;     // Initial Light Intensity
    private float start_time = 0.0f;            // Timer Start Time

    private bool timer_enabled = false;         // Timer Enabled/Disabled

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Switch Function
    public void switchLight()
    {
        if (light_on)
        {
            gameObject.GetComponent<HDAdditionalLightData>().intensity = 0.0f;              // Turn Light Off
            light_on = false;
        }
        else if (!light_on)
        {
            gameObject.GetComponent<HDAdditionalLightData>().intensity = initial_intensity; // Turn Light On
            light_on = true;
        }

        if (chained && !delayed)
        {
            chained_light.GetComponent<LightChainFX>().switchLight();                       // Run Chained Light's Script
        }
        else if (chained && delayed)
        {
            start_time = Time.time;                                                         // Get Start Time

            timer_enabled = true;                                                           // Enable Timer
        }
    }

    // Use this for initialization
    void Start()
    {
        initial_intensity = gameObject.GetComponent<HDAdditionalLightData>().intensity; // Get Initial Intensity
    }

    // Update is called once per frame
    void Update()
    {
        // If Timer Functionality is Enabled, and Delay has Passed
        if (timer_enabled && Time.time - start_time >= delay)
        {
            chained_light.GetComponent<LightChainFX>().switchLight();                       // Run Chained Light's Script

            timer_enabled = false;                                                          // Disable Timer
        }
    }
}