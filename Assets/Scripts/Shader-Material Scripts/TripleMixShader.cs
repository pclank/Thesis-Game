using UnityEngine;
using System;
using System.Collections;

// ************************************************************************************
// Mix Shader Control Code for Three Materials
// ************************************************************************************

public class TripleMixShader : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Prediction Certainty Constraint.")]
    public float target_certainty = 70.0f;                      // Target Emotion Certainty Constraint
    public float transition_speed = 0.2f;                       // Speed of Transition
    public float delay = 10.0f;                                 // Prediction Interval

    [Header("Runtime Options")]
    [Tooltip("Don't Reverse to Original Material.")]
    public bool forwards_only_mode = false;

    [Tooltip("Development Mode Index.")]
    public int dev_index = 0;

    [Tooltip("Enables Development Mode.")]
    public bool dev_mode = false;                               // Development Mode

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                           // Player GameObject

    private float timer_value = 0.0f;                           // Timer Value

    private int emotion_detected_index = 0;                     // Index of Emotion Detected

    private bool timer_on = false;                              // Whether Timer is Enabled
    private bool forwards = true;                               // Whether Blending is Increasing
    private bool emotion_detected = false;                      // Whether Target Emotion has been Detected

    private Material mat;

    // Use this for initialization
    void Start()
    {
        mat = gameObject.GetComponent<MeshRenderer>().material;

        player_object = GameObject.FindWithTag("Player");

        // *****************************************************************************************
        // Initial Emotion Prediction
        // *****************************************************************************************

        if (!dev_mode)
        {
            Tuple<int, float> prediction = player_object.GetComponent<JSONReader>().readEmotionIndex();

            // Check for Target Emotion and Certainty Constraint Satisfaction
            if (prediction.Item2 >= target_certainty || dev_mode)
            {
                emotion_detected_index = prediction.Item1;

                emotion_detected = true;                            // Set Emotion as Detected
            }

            timer_value = Time.time;                            // Update Timer Value
            timer_on = true;                                    // Enable Timer Functionality
        }
        else if (dev_mode)
            emotion_detected_index = dev_index;
    }

    // Update is called once per frame
    void Update()
    {
        // Timer - Prediction Section

        if (!dev_mode && timer_on && (Time.time - timer_value) >= delay)
        {
            //emotion_detected = false;                           // Reset Flag

            Tuple<int, float> prediction = player_object.GetComponent<JSONReader>().readEmotionIndex();

            // Check for Target Emotion and Certainty Constraint Satisfaction
            if (prediction.Item2 >= target_certainty || dev_mode)
            {
                emotion_detected_index = prediction.Item1;

                emotion_detected = true;                            // Set Emotion as Detected
            }

            timer_value = Time.time;                            // Update Timer Value
        }

        // Shader Mixing Section

        if (emotion_detected || dev_mode)
        {
            float next_state = gameObject.GetComponent<MeshRenderer>().material.GetFloat("BlendOpacity");
            float sec_next_state = gameObject.GetComponent<MeshRenderer>().material.GetFloat("SecBlendOpacity");

            // Happiness Detected
            if (emotion_detected_index == 0)
            {
                // Reduce First Blend to Zero
                if (next_state > 0.0f)
                {
                    next_state -= transition_speed;

                    // Edge Case
                    if (next_state <= 0.0f)
                    {
                        next_state = 0.0f;

                        // Forwards Only Mode
                        if (forwards_only_mode)
                        {
                            emotion_detected = false;

                            Destroy(this);
                        }
                    }
                }

                // Reduce Second Blend to Zero
                if (sec_next_state > 0.0f)
                {
                    sec_next_state -= transition_speed;

                    // Edge Case
                    if (sec_next_state <= 0.0f)
                        sec_next_state = 0.0f;
                }
            }

            // Sadness Emotion
            else if (emotion_detected_index == 1)
            {
                // Increase First Blend to One
                if (next_state < 1.0f)
                {
                    next_state += transition_speed;

                    // Edge Case
                    if (next_state >= 1.0f)
                        next_state = 1.0f;
                }

                // Reduce Second Blend to Zero
                if (sec_next_state > 0.0f)
                {
                    sec_next_state -= transition_speed;

                    // Edge Case
                    if (sec_next_state <= 0.0f)
                        sec_next_state = 0.0f;
                }
            }

            // Anger Emotion
            else if (emotion_detected_index == 2 || emotion_detected_index == 3)
            {
                // Reduce First Blend to Zero
                if (next_state > 0.0f)
                {
                    next_state -= transition_speed;

                    // Edge Case
                    if (next_state <= 0.0f)
                        next_state = 0.0f;
                }

                // Increase Second Blend to One
                if (sec_next_state < 1.0f)
                {
                    sec_next_state += transition_speed;

                    // Edge Case
                    if (sec_next_state >= 1.0f)
                        sec_next_state = 1.0f;
                }
            }

            gameObject.GetComponent<MeshRenderer>().material.SetFloat("BlendOpacity", next_state);
            gameObject.GetComponent<MeshRenderer>().material.SetFloat("SecBlendOpacity", sec_next_state);
        }
    }
}