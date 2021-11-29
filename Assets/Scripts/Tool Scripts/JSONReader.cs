﻿using UnityEngine;
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

    public string json_location;                        // JSON File to Read
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
        string text = System.IO.File.ReadAllText(@json_location);

        deserialiazed_json =  JsonUtility.FromJson<EmotionList>(text);                  // Get Deserialize JSON Objects

        string detected_emotion = "Unknown";                                            // Initialize Returned Emotion
        float detected_certainty = 0.0f;                                                // Initialize Returned Certainty

        if (deserialiazed_json.Emotion[0].certainty >= lower_limit)
        {
            detected_certainty = deserialiazed_json.Emotion[0].certainty;   // Set Certainty
            detected_emotion = deserialiazed_json.Emotion[0].emotion;       // Set Emotion
        }

        return new Tuple<string, float>(detected_emotion, detected_certainty);          // Return Information
    }

    // Function to Read Emotion Index from File
    public Tuple<int, float> readEmotionIndex()
    {
        string text = System.IO.File.ReadAllText(@json_location);

        deserialiazed_json = JsonUtility.FromJson<EmotionList>(text);                   // Get Deserialize JSON Objects

        int detected_index = 0;                                                         // Initialize Returned Index
        float detected_certainty = 0.0f;                                                // Initialize Returned Certainty

        if (deserialiazed_json.Emotion[0].certainty >= lower_limit)
        {
            detected_certainty = deserialiazed_json.Emotion[0].certainty;   // Set Certainty

            if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Happy"))
                detected_index = 0;
            else if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Sad"))
                detected_index = 1;
            else if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Angry"))
                detected_index = 2;
            else if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Surprised"))
                detected_index = 3;
        }

        return new Tuple<int, float>(detected_index, detected_certainty);               // Return Information
    }
}