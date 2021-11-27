using UnityEngine;
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

    [Header("Misc Options")]
    [Tooltip("Track Blend Speed.")]
    public float blend_speed = 0.5f;

    [Tooltip("Delay Between Emotion Reading.")]
    public float read_delay = 5.0f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private uint emotion_detected;

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Read Emotion
    private void readEmotion()
    {
        // TODO: Write Code!
    }

    // Enable Wwise Track
    private void enableTrack()
    {
        // TODO: Write Code!
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}