using UnityEngine;
using System.Collections;

// ************************************************************************************
// Script for Dynamic Music Based on Emotion
// ************************************************************************************

[RequireComponent(typeof(AudioSource))]
public class DynamicMusic : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public AudioSource main_source;

    public AudioClip default_clip;
    public AudioClip happiness_clip;
    public AudioClip sadness_clip;
    public AudioClip anger_clip;
    public AudioClip surprise_clip;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    // Effect Flags

    //private bool chorus_enable = false;
    //private bool distortion_enable = false;
    //private bool lowpass_enable = false;

    // Other Variables

    private int current_emotion = 0;            // Index of Current Detected Emotion (0: Default, 1: Happiness, 2: Sadness, 3: Anger, 4: Surprise)
    private int detected_emotion = 0;           // Index of Newly Detected Emotion (0: Default, 1: Happiness, 2: Sadness, 3: Anger, 4: Surprise)
    private int clip_time = 0;                  // Index of Precise Current Sample of Clip

    private GameObject player_object;

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Change Clip on Emotion Change
    private void changeClip(int emotion_index)
    {
        main_source.Stop();                     // Stop Clip Playing
        clip_time = main_source.timeSamples;    // Get Time

        // Choose New Clip Based on Emotion
        switch (emotion_index)
        {
            case 0:
                main_source.clip = default_clip;
                break;
            case 1:
                main_source.clip = happiness_clip;
                break;
            case 2:
                main_source.clip = sadness_clip;
                break;
            case 3:
                main_source.clip = anger_clip;
                break;
            case 4:
                main_source.clip = surprise_clip;
                break;
        }

        main_source.Play();         // Start Clip
    }

    // Detect Emotion Change
    private void detectEmotionChange()
    {
        // Check that New Emotion is Different than Previous One
        if (detected_emotion != current_emotion)
        {
            changeClip(detected_emotion);
        }
    }

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");       // Get Player GameObject
    }
}