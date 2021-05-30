using System;
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

    public int[] anger_color = new int[3];
    public int[] happiness_color = new int[3];
    public int[] sadness_color = new int[3];

    public float color_smoothness = 5.0f;
    public float color_interval = 0.0039f;
    public float tgt_error = 0.0039f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool player_trigger = false;                        // Player In/Out of Volume

    private Volume volume;                                      // Volume Component
    private ColorAdjustments color_adjust;                      // Color Adjustment Object

    private bool[] emotion_detected = { false, false, false };  // Emotion Detected

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

    // Helper Function to Convert Color RGB Value to Float Value

    private float convertColor(int init_value)
    {
        return init_value / 255.0f;
    }

    // Helper Function to Check If Error Condition is Met

    private bool checkErrorCondition(float current_color, float tgt_color)
    {
        if (Math.Abs(current_color - tgt_color) <= tgt_error)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Change Color

    private void changeColor(int[] colors)
    {
        Color new_color = new Color(convertColor(colors[0]), convertColor(colors[1]), convertColor(colors[2]), 1.0f);  // Build Color

        //var color_parameter = new ColorParameter(new_color);

        color_adjust.colorFilter.Override(new_color);                                                   // Apply Color to Filter
    }

    // Overriden Change Color, Using Float RGB Values in Color Type

    private void changeColor(Color new_color)
    {
        color_adjust.colorFilter.Override(new_color);
    }

    // Smoothly Change Color

    private void smoothChange(int[] tgt_color)
    {
        Color prev_color = color_adjust.colorFilter.value;      // Get Previous Color
        Color new_color = prev_color;                           // Set Colors as the Same in the Beginning

        // Red Value

        if (checkErrorCondition(prev_color.r, convertColor(tgt_color[0])))
        {
            new_color.r = convertColor(tgt_color[0]);
        }
        else if (prev_color.r < convertColor(tgt_color[0]))
        {
            new_color.r += color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }
        else if (prev_color.r > convertColor(tgt_color[0]))
        {
            new_color.r -= color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }

        // Green Value

        if (checkErrorCondition(prev_color.g, convertColor(tgt_color[1])))
        {
            new_color.g = convertColor(tgt_color[1]);
        }
        else if (prev_color.g < convertColor(tgt_color[1]))
        {
            new_color.g += color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }
        else if (prev_color.g > convertColor(tgt_color[1]))
        {
            new_color.g -= color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }

        // Blue Value

        if (checkErrorCondition(prev_color.b, convertColor(tgt_color[2])))
        {
            new_color.b = convertColor(tgt_color[2]);
        }
        else if (prev_color.b < convertColor(tgt_color[2]))
        {
            new_color.b += color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }
        else if (prev_color.b > convertColor(tgt_color[2]))
        {
            new_color.b -= color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }

        // Apply Color
        changeColor(new_color);

        // TODO: Add Way to Reverse Procedure to White Color
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get Volume Component
        volume = GetComponent<Volume>();

        // Get Color Adjustments Component
        
        if (volume.profile.TryGet(out color_adjust))
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Remove This as It's a Test Version

        if (Input.GetKeyDown(KeyCode.G))
        {
            emotion_detected[0] = true;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            emotion_detected[1] = true;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            emotion_detected[2] = true;
        }

        if (emotion_detected[0])
        {
            //changeColor(happiness_color);
            smoothChange(happiness_color);
            Debug.Log("Happiness Color!");
        }
        else if (emotion_detected[1])
        {
            //changeColor(sadness_color);
            smoothChange(sadness_color);
            Debug.Log("Sadness Color");
        }
        else if (emotion_detected[2])
        {
            //changeColor(anger_color);
            smoothChange(anger_color);
            Debug.Log("Anger Color");
        }
    }
}
