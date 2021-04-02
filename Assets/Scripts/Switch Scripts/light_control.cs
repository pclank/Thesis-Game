using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class light_control : MonoBehaviour
{
    // Initial Light Intensity in On Setting
    public float initial_intensity = 1200.0f;

    // Boolean Denoting Manual Light Entry
    public bool manual_intensity = false;

    // Function Called to Turn Lights On/Off

    public void switchLight()
    {
        // Get Component List
        Component[] light_list = gameObject.GetComponentsInChildren<HDAdditionalLightData>();

        foreach (HDAdditionalLightData light in light_list)
        {
            // Light is Off
            if (light.intensity != initial_intensity)
            {
                light.intensity = initial_intensity;
            }
            // Light is On
            else
            {
                light.intensity = 0.0f;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // If Manual Setting is False
        if (!manual_intensity)
        {
            initial_intensity = gameObject.GetComponentInChildren<HDAdditionalLightData>().intensity;   // Get Initial Light Intensity
        }
    }
}
