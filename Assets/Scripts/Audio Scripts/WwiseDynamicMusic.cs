using UnityEngine;
using System;
using System.Collections;

// ************************************************************************************
// Wwise Dynamic Music Control Script
// ************************************************************************************

public class WwiseDynamicMusic : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Header("Wwise Settings - References")]
    [Tooltip("Main Wwise Event.")]
    public AK.Wwise.Event main_wwise_event;

    [Tooltip("Enable Main Music Filter Wwise Event.")]
    public AK.Wwise.Event filter_wwise_event;

    [Tooltip("Reset Filter Wwise Event.")]
    public AK.Wwise.Event reset_filter_wwise_event;

    [Tooltip("Happy Wwise Event.")]
    public AK.Wwise.Event happy_wwise_event;

    [Tooltip("Sad Wwise Event.")]
    public AK.Wwise.Event sad_wwise_event;

    [Tooltip("Angry Wwise Event.")]
    public AK.Wwise.Event angry_wwise_event;

    [Header("Misc Options")]
    [Tooltip("Track Blend Speed.")]
    public float blend_speed = 0.5f;

    [Tooltip("Delay Between Emotion Reading.")]
    public float read_delay = 5.0f;

    [Tooltip("Lower Certainty Bound.")]
    public float lower_certainty = 60.0f;

    [Header("Development Mode Options")]
    [Tooltip("Enable Development Mode.")]
    public bool development_mode;

    public KeyCode happy_key = KeyCode.Alpha1;
    public KeyCode sad_key = KeyCode.Alpha2;
    public KeyCode angry_key = KeyCode.Alpha3;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private AK.Wwise.Event current_wwise_event;

    private bool timer_on = false;

    private float timer_value;

    private int emotion_detected;

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Read Emotion
    private bool readEmotion()
    {
        Tuple<int, float> results =  GetComponent<JSONReader>().readEmotionIndex();     // Get Emotion from JSONReader

        if (results.Item2 >= lower_certainty)
        {
            emotion_detected = results.Item1;

            return true;
        }
        else
            return false;
    }

    // Enable Wwise Track
    private void enableTrack()
    {
        AK.Wwise.Event selected_event;

        switch (emotion_detected)
        {
            case 0:
                selected_event = happy_wwise_event;
                break;
            case 1:
                selected_event = sad_wwise_event;
                break;
            case 2:
                selected_event = angry_wwise_event;
                break;
            default:
                selected_event = happy_wwise_event;
                break;
        }

        if (current_wwise_event == null || selected_event != current_wwise_event)
        {
            if (current_wwise_event != null)
                current_wwise_event.Stop(gameObject);
            else
                filter_wwise_event.Post(gameObject);

            selected_event.Post(gameObject);
            current_wwise_event = selected_event;
        }
    }

    // Use this for initialization
    void Start()
    {
        main_wwise_event.Post(gameObject);                          // Start Music
    }

    // Update is called once per frame
    void Update()
    {
        // Timer Section
        if ((!timer_on && !development_mode) || (timer_on && !development_mode && Time.time - timer_value >= read_delay))
        {
            if (readEmotion())
                enableTrack();

            timer_value = Time.time;

            if (!timer_on)
                timer_on = true;
        }

        // Development Mode Section
        if (development_mode)
        {
            if (Input.GetKeyUp(happy_key))
            {
                emotion_detected = 0;
                enableTrack();
            }
            else if (Input.GetKeyUp(sad_key))
            {
                emotion_detected = 1;
                enableTrack();
            }
            else if (Input.GetKeyUp(angry_key))
            {
                emotion_detected = 2;
                enableTrack();
            }
        }
    }
}