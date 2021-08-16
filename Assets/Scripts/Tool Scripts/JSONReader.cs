using UnityEngine;
using System;
using System.Collections;

// ************************************************************************************
// JSON Object Emotion List Class
// ************************************************************************************

[System.Serializable]
public class EmotionList
{
    public Emotion[] Emotion;
}

// ************************************************************************************
// JSON Object Emotion Class
// ************************************************************************************

[System.Serializable]
public class Emotion
{
    public string emotion;
    public float certainty;
}

public class JSONReader : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public TextAsset json_location;                     // JSON File to Read
    public float lower_limit = 10.0f;                   // Lower Limit on Certainty

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private EmotionList deserialiazed_json;             // Deserialized JSON Objects

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Function to Read Emotion from File
    public Tuple<string, float> readEmotion()
    {
        deserialiazed_json =  JsonUtility.FromJson<EmotionList>(json_location.text);    // Get Deserialize JSON Objects

        string detected_emotion = "Unknown";                                            // Initialize Returned Emotion
        float detected_certainty = 0.0f;                                                // Initialize Returned Certainty

        if (deserialiazed_json.Emotion[0].certainty >= lower_limit)
        {
            detected_certainty = deserialiazed_json.Emotion[0].certainty;   // Set Certainty
            detected_emotion = deserialiazed_json.Emotion[0].emotion;       // Set Emotion
        }

        return new Tuple<string, float>(detected_emotion, detected_certainty);          // Return Information
    }
}