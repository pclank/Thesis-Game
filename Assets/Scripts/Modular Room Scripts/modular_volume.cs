using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

public class modular_volume : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public float[] anger_color = new float[3];
    public float[] happiness_color = new float[3];
    public float[] sadness_color = new float[3];

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool player_trigger = false;    // Player In/Out of Volume
    private ColorAdjustments color_adjust;  // Color Adjustment Object

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_trigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_trigger = false;
        }
    }

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Change Color

    private void changeColor(float[] colors)
    {
        Color new_color = new Color(colors[0] / 255.0f, colors[1] / 255.0f, colors[2] / 255.0f, 1.0f);

        //var color_parameter = new ColorParameter(new_color);

        color_adjust.colorFilter.Override(new_color);
    }

    // TODO: Add Function to Calculate Smooth Change to Color.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
