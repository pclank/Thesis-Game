using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Rendering.HighDefinition;

public class LightColorChange : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public Color anger_color = Color.white;
    public Color happiness_color = Color.white;
    public Color sadness_color = Color.white;

    public float color_smoothness = 5.0f;
    public float color_interval = 0.0039f;
    public float tgt_error = 0.0039f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private Color default_color;                                // Default Color

    private bool reverse_color = false;                         // Flag that is Set to Reverse Color, and Vice Versa
    private bool[] emotion_detected = { false, false, false };  // Emotion Detected

    private HDAdditionalLightData light_data;                   // HDAdditionalLightData Component of Object

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

    // Overriden Change Color, Using Float RGB Values in Color Type

    private void changeColor(Color new_color)
    {
        light_data.color = new_color;
    }

    // Smoothly Change Color

    private void smoothChange(Color tgt_color)
    {
        Color prev_color = light_data.color;                    // Get Previous Color
        Color new_color = prev_color;                           // Set Colors as the Same in the Beginning

        // Red Value

        if (checkErrorCondition(prev_color.r, tgt_color.r))
        {
            new_color.r = tgt_color.r;
        }
        else if (prev_color.r < tgt_color.r)
        {
            new_color.r += color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }
        else if (prev_color.r > tgt_color.r)
        {
            new_color.r -= color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }

        // Green Value

        if (checkErrorCondition(prev_color.g, tgt_color.g))
        {
            new_color.g = tgt_color.g;
        }
        else if (prev_color.g < tgt_color.g)
        {
            new_color.g += color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }
        else if (prev_color.g > tgt_color.g)
        {
            new_color.g -= color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }

        // Blue Value

        if (checkErrorCondition(prev_color.b, tgt_color.b))
        {
            new_color.b = tgt_color.b;
        }
        else if (prev_color.b < tgt_color.b)
        {
            new_color.b += color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }
        else if (prev_color.b > tgt_color.b)
        {
            new_color.b -= color_interval * Time.deltaTime * color_smoothness;  // Update Color Value
        }

        // Apply Color
        changeColor(new_color);

        // Check Condition to Reverse Procedure

        if (checkErrorCondition(prev_color.r, tgt_color.r) && checkErrorCondition(prev_color.g, tgt_color.g) && checkErrorCondition(prev_color.b, tgt_color.b))
        {
            reverse_color = !reverse_color;
        }
    }

    // Use this for initialization
    void Start()
    {
        light_data = gameObject.GetComponent<HDAdditionalLightData>();  // Get Light Component

        default_color = light_data.color;                               // Get Default Color
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Remove This as It's a Test Version

        if (Input.GetKeyDown(KeyCode.G))
        {
            emotion_detected[1] = false;
            emotion_detected[2] = false;

            emotion_detected[0] = true;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            emotion_detected[0] = false;
            emotion_detected[2] = false;

            emotion_detected[1] = true;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            emotion_detected[0] = false;
            emotion_detected[1] = false;

            emotion_detected[2] = true;
        }

        if (emotion_detected[0])
        {
            if (!reverse_color)
            {
                smoothChange(happiness_color);
                Debug.Log("Happiness Color!");
            }
            else
            {
                smoothChange(default_color);
                Debug.Log("Happiness Color!");
            }
        }
        else if (emotion_detected[1])
        {
            if (!reverse_color)
            {
                smoothChange(sadness_color);
                Debug.Log("Sadness Color");
            }
            else
            {
                smoothChange(default_color);
                Debug.Log("Sadness Color");
            }
        }
        else if (emotion_detected[2])
        {
            if (!reverse_color)
            {
                smoothChange(anger_color);
                Debug.Log("Anger Color");
            }
            else
            {
                smoothChange(default_color);
                Debug.Log("Sadness Color");
            }
        }
    }
}