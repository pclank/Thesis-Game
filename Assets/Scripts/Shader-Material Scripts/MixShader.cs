using UnityEngine;
using System;
using System.Collections;

public class MixShader : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Emotion that Triggers the Mix Shader Functionality.")]
    public string target_emotion;                               // Target Emotion to Look for

    [Tooltip("Prediction Certainty Constraint.")]
    public float target_certainty = 70.0f;                      // Target Emotion Certainty Constraint
    public float transition_speed = 0.2f;                       // Speed of Transition
    public float delay = 10.0f;                                 // Prediction Interval

    [Header("Runtime Options")]
    [Tooltip("Get Emotion from Code.")]
    public bool manual_emotion = false;

    [Tooltip("Don't Reverse to Original Material.")]
    public bool forwards_only_mode = true;

    [Tooltip("Enables Development Mode.")]
    public bool dev_mode = false;                               // Development Mode

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                           // Player GameObject

    private float timer_value = 0.0f;                           // Timer Value

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

        Tuple<string, float> prediction = player_object.GetComponent<JSONReader>().readEmotion();

        // Check for Target Emotion and Certainty Constraint Satisfaction
        if (String.Equals(prediction.Item1, target_emotion) && prediction.Item2 >= target_certainty || dev_mode || manual_emotion)
        {
            emotion_detected = true;                            // Set Emotion as Detected
        }

        timer_value = Time.time;                            // Update Timer Value
        timer_on = true;                                    // Enable Timer Functionality
    }

    // Update is called once per frame
    void Update()
    {
        // Timer - Prediction Section

        if (timer_on && (Time.time - timer_value) >= delay)
        {
            emotion_detected = false;                           // Reset Flag

            Tuple<string, float> prediction = player_object.GetComponent<JSONReader>().readEmotion();

            // Check for Target Emotion and Certainty Constraint Satisfaction
            if (String.Equals(prediction.Item1, target_emotion) && prediction.Item2 >= target_certainty || dev_mode)
            {
                emotion_detected = true;                            // Set Emotion as Detected
            }

            timer_value = Time.time;                            // Update Timer Value
        }

        // Shader Mixing Section

        if (emotion_detected)
        {
            float next_state = gameObject.GetComponent<MeshRenderer>().material.GetFloat("BlendOpacity");

            if (forwards && next_state < 1.0f)
            {
                next_state += transition_speed;

                // Edge Case
                if (next_state >= 1.0f)
                {
                    next_state = 1.0f;

                    forwards = false;

                    // Forwards Only Mode
                    if (forwards_only_mode)
                    {
                        emotion_detected = false;

                        Destroy(this);
                    }
                }
            }

            else if (!forwards && next_state > 0.0f)
            {
                next_state -= transition_speed;

                // Edge Case
                if (next_state <= 0.0f)
                {
                    next_state = 0.0f;

                    forwards = true;
                }
            }

            gameObject.GetComponent<MeshRenderer>().material.SetFloat("BlendOpacity", next_state);
        }
    }
}