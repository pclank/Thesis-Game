using UnityEngine;
using System.Collections;

// ************************************************************************************
// Script for Dynamic Music Based on Emotion
// ************************************************************************************

public class DynamicMusic : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public AudioClip happiness_clip;
    public AudioClip sadness_clip;
    public AudioClip anger_clip;
    public AudioClip surprise_clip;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    // Effect Flags

    private bool chorus_enable = false;
    private bool distortion_enable = false;
    private bool lowpass_enable = false;

    // Emotion Detection Flags

    private bool happiness_detected = false;
    private bool sadness_detected = false;
    private bool anger_detected = false;
    private bool surprise_detected = false;

    // Other Variables

    private GameObject player_object;

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }
}