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

    [Header("GameObjects Control Section")]
    [Tooltip("GameObjects to be Disabled upon Entering Volume.")]
    public GameObject lod_to_disable;                                   // GameObject Parent LOD to Disable

    [Tooltip("GameObjects to Enable upon Entering Volume.")]
    public GameObject lod_to_enable;

    [Tooltip("HDRISky GameObject.")]
    public Volume sky_object;

    [Tooltip("HDRISky Night Lighting Value.")]
    public float night_sky_lighting_value = 0.0f;

    [Tooltip("Room Name for Analytics.")]
    public string room_name = "Old Modular Room";

    [Header("Emotion Recognition Response Section")]

    public int[] anger_color = new int[3];
    public int[] happiness_color = new int[3];
    public int[] sadness_color = new int[3];
    public int[] surprise_color = new int[3];
    public int[] default_color = { 255, 255, 255 };

    public float color_smoothness = 5.0f;
    public float color_interval = 0.0039f;
    public float tgt_error = 0.0039f;
    public float delay = 2.0f;                                          // Delay for Detection

    public bool color_change_on = false;                                // Color Change Flag

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                                   // Player GameObject

    private bool player_trigger = false;                                // Player In/Out of Volume
    private bool reverse_color = false;                                 // Flag that is Set to Reverse Color, and Vice Versa
    private bool detect_flag = true;                                    // Flag for Locking Emotion Detection

    private Volume volume;                                              // Volume Component
    private ColorAdjustments color_adjust;                              // Color Adjustment Object

    private bool[] emotion_detected = { false, false, false, false };   // Emotion Detected

    private float timer_start = 0.0f;                                   // Timer Start Value
    private float enter_time;                                           // Room Enter Time Value for Analytics

    private string model_emotion = "Unknown";                           // Emotion Detected by Model

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enableHDRILighting();                       // Enable HDRI Lighting

            enter_time = Time.time;                     // Set Enter Time for Analytics

            lod_to_disable.SetActive(false);            // Disable GameObjects
            //lod_to_enable.SetActive(true);              // Enable GameObjects

            // Add Information to Journal
            if (GetComponent<AddToJournalOnInteraction>() != null)
                GetComponent<AddToJournalOnInteraction>().addToJournal();

            player_trigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_trigger = false;

            player_object.GetComponent<RoomVolumeAnalytics>().addAnalytics(room_name, enter_time);  // Add Analytics on Exit

            Destroy(GameObject.FindWithTag("CassettePlayer"));

            disableHDRILighting();                      // Disable HDRI Lighting
        }
    }

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Set Emotion Flags

    private void setEmotionFlags()
    {
        if (String.Equals(model_emotion, "Happy"))
        {
            emotion_detected[1] = false;
            emotion_detected[2] = false;
            emotion_detected[3] = false;

            emotion_detected[0] = true;
        }
        else if (String.Equals(model_emotion, "Sad"))
        {
            emotion_detected[0] = false;
            emotion_detected[2] = false;
            emotion_detected[3] = false;

            emotion_detected[1] = true;
        }
        else if (String.Equals(model_emotion, "Angry"))
        {
            emotion_detected[0] = false;
            emotion_detected[1] = false;
            emotion_detected[3] = false;

            emotion_detected[2] = true;
        }
        else if (String.Equals(model_emotion, "Surprised"))
        {
            emotion_detected[0] = false;
            emotion_detected[1] = false;
            emotion_detected[2] = false;

            emotion_detected[3] = true;
        }
    }

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

        // Check Condition to Reverse Procedure

        if (checkErrorCondition(prev_color.r, convertColor(tgt_color[0])) && checkErrorCondition(prev_color.g, convertColor(tgt_color[1])) && checkErrorCondition(prev_color.b, convertColor(tgt_color[2])))
        {
            reverse_color = !reverse_color;
        }
    }

    // Enable HDRI Lighting
    private void enableHDRILighting()
    {
        HDRISky temp_sky;
        ColorAdjustments temp_color_adjustments;

        if (sky_object.profile.TryGet(out temp_sky))
        {
            temp_sky.desiredLuxValue.Override(night_sky_lighting_value);
        }

        if (sky_object.profile.TryGet(out temp_color_adjustments))
        {
            temp_color_adjustments.active = true;
        }
    }

    // Disable HDRI Lighting
    private void disableHDRILighting()
    {
        HDRISky temp_sky;
        ColorAdjustments temp_color_adjustments;

        if (sky_object.profile.TryGet(out temp_sky))
        {
            temp_sky.desiredLuxValue.Override(0.0f);
        }

        if (sky_object.profile.TryGet(out temp_color_adjustments))
        {
            temp_color_adjustments.active = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");   // Get Player GameObject

        // Get Volume Component
        volume = GetComponent<Volume>();

        //lod_to_enable.SetActive(false);                     // Initially Disable GameObjects

        // Get Color Adjustments Component
        
        if (volume.profile.TryGet(out color_adjust))
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        // Timer Section
        if (!detect_flag && (Time.time - timer_start >= delay))
        {
            detect_flag = true;             // Allow Detection
        }

        // TODO: Remove This as It's a Test Version

        if (Input.GetKeyDown(KeyCode.T))
        {
            emotion_detected[1] = false;
            emotion_detected[2] = false;

            emotion_detected[0] = true;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            emotion_detected[0] = false;
            emotion_detected[2] = false;

            emotion_detected[1] = true;
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            emotion_detected[0] = false;
            emotion_detected[1] = false;

            emotion_detected[2] = true;
        }

        if (color_change_on && detect_flag)
        {
            model_emotion = player_object.GetComponent<JSONReader>().readEmotion().Item1;   // Read Emotion from JSON

            setEmotionFlags();                                                              // Set Emotion Flags

            timer_start = Time.time;                                                        // Get Start Time for Timer

            detect_flag = false;                                                            // Reset Flag
        }

        if (player_trigger && emotion_detected[0])
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
        else if (player_trigger && emotion_detected[1])
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
        else if (player_trigger && emotion_detected[2])
        {
            if (!reverse_color)
            {
                smoothChange(anger_color);
                Debug.Log("Anger Color");
            }
            else
            {
                smoothChange(default_color);
                Debug.Log("Anger Color");
            }
        }
        else if (player_trigger && emotion_detected[3])
        {
            if (!reverse_color)
            {
                smoothChange(surprise_color);
                Debug.Log("Surprise Color");
            }
            else
            {
                smoothChange(default_color);
                Debug.Log("Surprise Color");
            }
        }
    }
}
